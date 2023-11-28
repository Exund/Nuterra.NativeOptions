using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Rewired;

namespace Nuterra.NativeOptions
{
    internal class UIOptionsMods : UIOptions
	{
		private static List<Option> m_Options = new List<Option>();
		private static List<Option> m_PendingOptions = new List<Option>();
		private static OptionKey m_PendingRequest;

		internal static Transform Content1;
		internal static Transform Content2;
		internal static Transform MidPanel;

		internal static Transform PrevPage;
		internal static Transform NextPage;
		internal static Transform PageInfo;

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

        private void Update()
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
            m_PendingRequest?.Reset();
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

			NativeOptionsMod.onOptionsSaved.Invoke();
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Entry OptToEntry(Option o) => new Entry("", o);

		public override void Setup(EventNoParams OnChangeEvent)
		{
			this.ChangesMadeEventToCall = OnChangeEvent;

            if (m_PendingOptions.Count == 0)
            {
                return;
            }

            HandlePandingOptions();

			try
            {
                GeneratePages();
            }
			catch (Exception e)
			{
				Console.WriteLine("Pagination");
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

        private void HandlePandingOptions()
        {
            foreach (var option in m_PendingOptions)
            { 
                switch (option.onValueChanged)
                {
                    case Dropdown.DropdownEvent dde:
                        dde.AddListener(e => { ChangesMadeEventToCall.Send(); });
                        break;
                    case InputField.OnChangeEvent oce:
                        oce.AddListener(e => { ChangesMadeEventToCall.Send(); });
                        break;
                    case Slider.SliderEvent sle:
                        sle.AddListener(e => { ChangesMadeEventToCall.Send(); });
                        break;
                    case Toggle.ToggleEvent tge:
                        tge.AddListener(e => { ChangesMadeEventToCall.Send(); });
                        break;
                    case OptionKey.OnChangeEvent koce:
                        koce.AddListener(e => { ChangesMadeEventToCall.Send(); });
                        break;
                }
            }

            m_Options.AddRange(m_PendingOptions);
            m_PendingOptions.Clear();

            m_Options = m_Options.OrderBy(o => o.modName).ToList();
        }

        private void GeneratePages()
        {
			var columns = new List<Column>();
			var columnIndex = 0;
			foreach (var group in m_Options.GroupBy(o => o.modName))
			{
				if (columns.Count == columnIndex) {
                    columns.Add(new Column() { entries = new List<Entry>() });
                }

                var title = new Entry(group.Key);
				var options = group.ToList();
				var optsCount = options.Count + 1;
				if (optsCount < 11)
				{
					var currentColumn = columns[columnIndex];
					if (currentColumn.entries.Count + optsCount < 11)
					{
						currentColumn.entries.Add(title);
						currentColumn.entries.AddRange(options.Select(OptToEntry));
					}
					else
					{
						columnIndex++;
						var column = new Column()
						{
							entries = new List<Entry>() { title }
						};
						column.entries.AddRange(options.Select(OptToEntry));
						columns.Add(column);
					}
				}
				else if (optsCount < 17)
				{
					columnIndex++;
					var column = new Column()
					{
						entries = new List<Entry>() { title }
					};
					column.entries.AddRange(options.Select(OptToEntry));

					if (columns[0].entries.Count != 0) {
                        columns.Add(column);
                    }
                    else
                    {
                        columns[0] = column;
                    }
				}
				else
				{
					var slice = options.GetRange(0, options.Count);
					while (slice.Count > 0)
					{
						columnIndex++;
						var column = new Column()
						{
							entries = new List<Entry>() { title }
						};

                        var count = Math.Min(16, slice.Count);
						column.entries.AddRange(slice.GetRange(0, count).Select(OptToEntry));

                        if (columns[0].entries.Count != 0)
                        {
                            columns.Add(column);
                        }
                        else
                        {
                            columns[0] = column;
                        }

						slice.RemoveRange(0, count);
					}
				}
				//Console.WriteLine(columns.Select(c => c.entries.Count).ToArray());
			}

			/*pages = new List<Page>();
			var pageIndex = -1;
			for (var i = 0; i < columns.Count; i++)
			{
                if (i % 2 == 0)
                {
                    pageIndex++;
                }

                if (pages.Count == pageIndex)
                {
                    pages.Add(new Page() { columns = new List<Column>() });
                }

				pages[pageIndex].columns.Add(columns[i]);
			}*/

            pages = columns.Select((v, i) => new { v, i })
                .GroupBy(v => v.i / 2, v => v.v)
                .Select(c => new Page() { columns = c.ToList() })
                .ToList();
        }

        private void SetPage(int index)
		{
            if (index < 0 || index >= pages.Count)
            {
                return;
            }

			while(Content1.childCount > 0)
			{
				Content1.GetChild(0).SetParent(null, false);
			}
			while (Content2.childCount > 0)
			{
				Content2.GetChild(0).SetParent(null, false);
			}

			page_index = index;
			
			try
			{
				MidPanel.parent.Find("Top Panel/Category 1/Title").GetComponent<Text>().text = pages[page_index].columns[0].entries[0].title.ToUpper();
				foreach (var entry in pages[page_index].columns[0].entries)
				{
					if (entry.title != "" && entry.title != pages[page_index].columns[0].entries[0].title)
					{
						UIElements.CreateCategoryEntry(entry.title.ToUpper()).transform.SetParent(Content1, false);
					}
					else
                    {
                        entry.option?.UIElement.transform.SetParent(Content1, false);
                    }
                }

				if (pages[page_index].columns.Count > 1)
				{
					MidPanel.parent.Find("Top Panel/Category 2/Title").GetComponent<Text>().text = pages[page_index].columns[1].entries[0].title.ToUpper();
					foreach (var entry in pages[page_index].columns[1].entries)
					{
						if (entry.title != "" && entry.title != pages[page_index].columns[1].entries[0].title)
						{
							UIElements.CreateCategoryEntry(entry.title.ToUpper()).transform.SetParent(Content2, false);
						}
						else
                        {
                            entry.option?.UIElement.transform.SetParent(Content2, false);
                        }
                    }
				}
				else MidPanel.parent.Find("Top Panel/Category 2/Title").GetComponent<Text>().text = "";
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

        private struct Entry
		{
			public string title;
			public Option option;

			public Entry(string Title = "", Option opt = null)
			{
				title = Title;
				option = opt;
			}
		}

        private struct Column
		{
			public List<Entry> entries;
		}

        private struct Page
		{
			public List<Column> columns;
		}
	}
}
