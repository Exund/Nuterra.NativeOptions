using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Rewired;

namespace Nuterra.NativeOptions
{
	class UIOptionsMods : UIOptions
	{
		private static List<Option> m_Options = new List<Option>();
		private static List<Option> m_PendingOptions = new List<Option>();
		private static OptionKey m_PendingRequest;

		internal static GameObject Content1;
		internal static GameObject Content2;
		internal static GameObject MidPanel;

		internal static GameObject PrevPage;
		internal static GameObject NextPage;
		internal static GameObject PageInfo;

		private int page_index = 0;
		private List<Page> pages = new List<Page>();

		public static void AddOption(Option option)
		{
			m_PendingOptions.Add(option);
		}

		public static bool RequestKeyAssign(OptionKey option)
		{
			var result = false;
			if(m_PendingRequest == null)
			{
				m_PendingRequest = option;
				result = true;
			}
			return result;
		}

		void Update()
		{
			if(m_PendingRequest != null)
			{
				var pollingInfo = ReInput.controllers.Keyboard.PollForFirstKey();
				if(pollingInfo.keyboardKey != KeyCode.None)
				{
					m_PendingRequest.KeyAssigned(pollingInfo.keyboardKey);
					m_PendingRequest = null;
				}
			}
		}

		internal Toggle tab_toggle;
		private EventNoParams ChangesMadeEventToCall;

		public override UIScreenOptions.SaveFailureType CanSave()
		{
			return UIScreenOptions.SaveFailureType.None;
		}

		public override void ClearSettings()
		{
			foreach (var option in m_Options)
			{
				option.savedValue = option.defaultValue;
				option.ResetValue();
			}
		}

		public override void OnCloseScreen() {
			if (m_PendingRequest != null) m_PendingRequest.Reset();
			m_PendingRequest = null;
		}

		public override void ResetSettings() { }

		public override void SaveSettings()
		{
			foreach (var option in m_Options)
			{
				option.savedValue = option.value;
				option.defaultValue = option.savedValue;
				option.onValueSaved.Invoke();
			}
		}

		public override void Setup(EventNoParams OnChangeEvent)
		{
			this.ChangesMadeEventToCall = OnChangeEvent;

			if(m_PendingOptions.Count > 0)
			{
				foreach (var option in m_PendingOptions)
				{ 
					if (option.onValueChanged is Dropdown.DropdownEvent dde)
					{
						dde.AddListener((e) => { ChangesMadeEventToCall.Send(); });
					}
					if (option.onValueChanged is InputField.OnChangeEvent oce)
					{
						oce.AddListener((e) => { ChangesMadeEventToCall.Send(); });
					}
					if (option.onValueChanged is Slider.SliderEvent sle)
					{
						sle.AddListener((e) => { ChangesMadeEventToCall.Send(); });
					}
					if (option.onValueChanged is Toggle.ToggleEvent tge)
					{
						tge.AddListener((e) => { ChangesMadeEventToCall.Send(); });
					}
					if (option.onValueChanged is OptionKey.OnChangeEvent koce)
					{
						koce.AddListener((e) => { ChangesMadeEventToCall.Send(); });
					}
				}

				m_Options.AddRange(m_PendingOptions);
				m_PendingOptions.Clear();

				m_Options = m_Options.OrderBy(o => o.modName).ToList();


				/*
				Entries per side :
					without compression : 10
					with compression : 16
				*/


				try
				{
					var columns = new List<Column>();
					var ci = 0;
					foreach (var group in m_Options.GroupBy(o => o.modName))
					{
						if (columns.Count == ci) columns.Add(new Column() { entries = new List<Entry>()});
						var opts = group.ToList();
						var num = opts.Count + 1;
						if (num < 11)
						{
							var cc = columns[ci];
							if (cc.entries.Count + num < 11)
							{
								cc.entries.Add(new Entry(group.Key));
								cc.entries.AddRange(opts.Select(o => new Entry("", o)));
							}
							else
							{
								ci++;
								var ccc = new Column()
								{
									entries = new List<Entry>()
									{
										new Entry(group.Key)
									}
								};
								ccc.entries.AddRange(opts.Select(o => new Entry("", o)));
								columns.Add(ccc);	
							}
						}
						else if (num < 17)
						{
							ci++;
							var ccc = new Column()
							{
								entries = new List<Entry>()
									{
										new Entry(group.Key)
									}
							};
							ccc.entries.AddRange(opts.Select(o => new Entry("", o)));
							columns.Add(ccc);
						}
						else
						{
							var ii = opts.GetRange(0, opts.Count);
							while (ii.Count > 0)
							{
								ci++;
								var ccc = new Column()
								{
									entries = new List<Entry>()
									{
										new Entry(group.Key)
									}
								};
								ccc.entries.AddRange(ii.GetRange(0, Math.Min(16, ii.Count)).Select(o => new Entry("", o)));
								columns.Add(ccc);

								ii.RemoveRange(0, Math.Min(16, ii.Count));
							}
						}
					}

					pages = new List<Page>();
					var pi = -1;
					for (var i = 0; i < columns.Count; i++)
					{
						if (i % 2 == 0) pi++;
						if (pages.Count == pi) pages.Add(new Page() { columns = new List<Column>() });
						pages[pi].columns.Add(columns[i]);

						Console.WriteLine($"Page {pi}");
						foreach (var e in columns[i].entries)
						{
							Console.Write($"Column {i} ");
							if(e.title != "")
							{
								Console.WriteLine("title " + e.title);
							}
							else
							{
								Console.WriteLine("name " +e.option.name);
							}
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Separation");
					Console.WriteLine(e.ToString());
				}

				var prev = PrevPage.GetComponent<Button>();
				prev.onClick.RemoveAllListeners();
				prev.onClick.AddListener(() =>
				{
					SetPage(page_index - 1);
				});

				var next = NextPage.GetComponent<Button>();
				next.onClick.RemoveAllListeners();
				next.onClick.AddListener(() =>
				{
					SetPage(page_index + 1);
				});

				SetPage(0);
			}
		}

		void SetPage(int index)
		{
			if (index < 0 || index >= pages.Count) return;

			while(Content1.transform.childCount > 0)
			{
				Content1.transform.GetChild(0).SetParent(null, false);
			}
			while (Content2.transform.childCount > 0)
			{
				Content2.transform.GetChild(0).SetParent(null, false);
			}

			page_index = index;
			
			try
			{
				MidPanel.transform.parent.Find("Top Panel/Category 1/Title").GetComponent<Text>().text = pages[page_index].columns[0].entries[0].title;
				foreach (var entry in pages[page_index].columns[0].entries)
				{
					if (entry.title != "" && entry.title != pages[page_index].columns[0].entries[0].title)
					{
						UIElements.CreateCategoryEntry(entry.title).transform.SetParent(Content1.transform, false);
					}
					else if(entry.option != null)
					{
						entry.option.UIElement.transform.SetParent(Content1.transform, false);
					}
				}

				if (pages[page_index].columns.Count > 1)
				{
					MidPanel.transform.parent.Find("Top Panel/Category 2/Title").GetComponent<Text>().text = pages[page_index].columns[1].entries[0].title;
					foreach (var entry in pages[page_index].columns[1].entries)
					{
						if (entry.title != "" && entry.title != pages[page_index].columns[1].entries[0].title)
						{
							UIElements.CreateCategoryEntry(entry.title).transform.SetParent(Content2.transform, false);
						}
						else if (entry.option != null)
						{
							entry.option.UIElement.transform.SetParent(Content2.transform, false);
						}
					}
				}
				else MidPanel.transform.parent.Find("Top Panel/Category 2/Title").GetComponent<Text>().text = "";
			}
			catch (Exception e)
			{
				Console.WriteLine("UI");
				Console.WriteLine(e.ToString());
			}

			var midGroup = MidPanel.GetComponent<HorizontalLayoutGroup>();
			midGroup.CalculateLayoutInputHorizontal();
			midGroup.CalculateLayoutInputVertical();
			midGroup.SetLayoutHorizontal();
			midGroup.SetLayoutVertical();

			var content2Group = Content2.GetComponent<VerticalLayoutGroup>();
			content2Group.CalculateLayoutInputHorizontal();
			content2Group.CalculateLayoutInputVertical();
			content2Group.SetLayoutHorizontal();
			content2Group.SetLayoutVertical();

			var content1Group = Content1.GetComponent<VerticalLayoutGroup>();
			content1Group.CalculateLayoutInputHorizontal();
			content1Group.CalculateLayoutInputVertical();
			content1Group.SetLayoutHorizontal();
			content1Group.SetLayoutVertical();

			var prev = PrevPage.GetComponent<Button>();
			prev.interactable = page_index - 1 >= 0;

			var next = NextPage.GetComponent<Button>();
			next.interactable = page_index + 1 < pages.Count;

			var text = PageInfo.GetComponent<Text>();
			text.text = $"Page {page_index + 1}/{pages.Count}";
		}

		/*struct ModOptions
		{
			public List<Option> options;
		}

		struct Column
		{
			public List<ModOptions> modOptions;
		}

		struct Page
		{
			public Column column1;
			public Column column2;
		}*/

		struct Entry
		{
			public string title;
			public Option option;

			public Entry(string Title = "", Option opt = null)
			{
				title = Title;
				option = opt;
			}
		}

		struct Column
		{
			public List<Entry> entries;
		}

		struct Page
		{
			public List<Column> columns;
		}
	}
}
