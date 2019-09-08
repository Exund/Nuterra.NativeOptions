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
				/*for (var i = 0; i < m_Options.Count; i++)
				{
					var option = m_Options[i];
					option.UIElement.transform.SetParent((i > m_Options.Count/2f ? Content2 : Content1).transform, false);
					////Console.WriteLine($"{option.modName}-{option.name} UI added");
				}*/



				List<Page> pages = new List<Page>();

				Page cpage = new Page()
				{
					column1 = new Column()
					{
						modOptions = new List<ModOptions>()
					},
					column2 = new Column()
					{
						modOptions = new List<ModOptions>()
					}
				};
				Column ccolumn = new Column();
				ccolumn.modOptions = new List<ModOptions>();

				try
				{
					foreach (var group in m_Options.GroupBy(o => o.modName))
					{
						ModOptions mo = new ModOptions();
						mo.options = group.ToList();

						if (mo.options.Count >= 16)
						{
							while (mo.options.Count >= 16)
							{
								if (cpage.column1.modOptions.Count == 0) cpage.column1 = ccolumn;
								else if (cpage.column2.modOptions.Count == 0) cpage.column2 = ccolumn;
								else
								{
									pages.Add(cpage);
									cpage = new Page()
									{
										column1 = new Column()
										{
											modOptions = new List<ModOptions>()
										},
										column2 = new Column()
										{
											modOptions = new List<ModOptions>()
										}
									};
								}
								ccolumn = new Column();
								ccolumn.modOptions = new List<ModOptions>()
								{
									new ModOptions()
									{
										options = mo.options.GetRange(0, 16)
									}
								};

								mo.options.RemoveRange(0, 16);
							}
						}
						else
						{
							if (ccolumn.modOptions.Sum(m => m.options.Count + 1) + mo.options.Count > 9)
							{
								if (cpage.column1.modOptions.Count == 0) cpage.column1 = ccolumn;
								else if (cpage.column2.modOptions.Count == 0) cpage.column2 = ccolumn;
								else
								{
									pages.Add(cpage);
									cpage = new Page()
									{
										column1 = new Column()
										{
											modOptions = new List<ModOptions>()
										},
										column2 = new Column()
										{
											modOptions = new List<ModOptions>()
										}
									};
								}
								ccolumn = new Column();
								ccolumn.modOptions = new List<ModOptions>();
							}
							ccolumn.modOptions.Add(mo);
						}
					}
					if (cpage.column1.modOptions.Count == 0) cpage.column1 = ccolumn;
					else if (cpage.column2.modOptions.Count == 0) cpage.column2 = ccolumn;
					pages.Add(cpage);
				}
				catch (Exception e)
				{
					Console.WriteLine("Separation");
					Console.WriteLine(e.ToString());
				}

				try
				{
					MidPanel.transform.parent.Find("Top Panel/Category 1/Title").GetComponent<Text>().text = pages[0].column1.modOptions[0].options[0].modName;
					foreach (var modOption in pages[0].column1.modOptions)
					{
						if (modOption.options[0] != pages[0].column1.modOptions[0].options[0])
						{
							UIElements.CreateCategoryEntry(modOption.options[0].modName).transform.SetParent(Content1.transform, false);
						}
						foreach (var option in modOption.options)
						{
							option.UIElement.transform.SetParent(Content1.transform, false);
						}
					}

					MidPanel.transform.parent.Find("Top Panel/Category 2/Title").GetComponent<Text>().text = pages[0].column2.modOptions[0].options[0].modName;
					foreach (var modOption in pages[0].column2.modOptions)
					{
						if (modOption.options[0] != pages[0].column2.modOptions[0].options[0])
						{
							UIElements.CreateCategoryEntry(modOption.options[0].modName).transform.SetParent(Content2.transform, false);
						}
						foreach (var option in modOption.options)
						{
							option.UIElement.transform.SetParent(Content2.transform, false);
						}
					}
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
			}
		}

		struct ModOptions
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
		}
	}
}
