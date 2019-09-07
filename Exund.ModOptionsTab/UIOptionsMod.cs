using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Rewired;

namespace Exund.ModOptionsTab
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
		private bool m_Initialised;

		public override UIScreenOptions.SaveFailureType CanSave()
		{
			return UIScreenOptions.SaveFailureType.None;
		}

		public override void ClearSettings()
		{
			foreach (var option in m_Options)
			{
				Console.WriteLine($"\n{option.name} reset");
				Console.WriteLine(option.savedValue.ToString() + " " + option.value.ToString() + " " + option.defaultValue.ToString());
				option.savedValue = option.defaultValue;
				option.ResetValue();
				Console.WriteLine(option.savedValue.ToString() + " " + option.value.ToString() + " " + option.defaultValue.ToString());
			}
		}

		public override void OnCloseScreen() {
			m_PendingRequest = null;
		}

		public override void ResetSettings() { }

		public override void SaveSettings()
		{
			foreach (var option in m_Options)
			{
				Console.WriteLine($"\n{option.name} save");
				Console.WriteLine(option.savedValue.ToString() + " " + option.value.ToString() + " " + option.defaultValue.ToString());
				option.savedValue = option.value;
				option.defaultValue = option.savedValue;
				option.onValueSaved.Invoke();
				Console.WriteLine(option.savedValue.ToString() + " " + option.value.ToString() + " " + option.defaultValue.ToString());
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

				for (var i = 0; i < m_Options.Count; i++)
				{
					var option = m_Options[i];
					option.UIElement.transform.SetParent((i > m_Options.Count/2f ? Content2 : Content1).transform, false);
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
	}
}
