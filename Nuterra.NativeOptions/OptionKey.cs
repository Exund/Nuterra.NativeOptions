using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Nuterra.NativeOptions
{
	public class OptionKey : Option<KeyCode>
	{
		private KeyCode m_Value;
		internal OnChangeEvent m_OnValueChanged = new OnChangeEvent();

		internal Button Button1;
		internal Text Text1;
		
		public OptionKey(string Name, string ModName, KeyCode DefaultValue) : base(Name, ModName, DefaultValue)
		{
			UIElement = GameObject.Instantiate(UIElements.KeybindingPrefab_TurnCamera);
			UIElement.SetActive(true);
			UIElement.name = $"KeybindingOption_{ModName}-{Name}";
			var text = UIElement.transform.Find("Text");
			text.GetComponent<Text>().text = Name;
			

			var KeybindButton1 = UIElement.transform.Find("Buttons Panel/Button").gameObject;
			Button1 = KeybindButton1.GetComponent<Button>();
			Text1 = KeybindButton1.GetComponentInChildren<Text>();

			Button1.onClick.AddListener(() =>
			{
				if(UIOptionsMods.RequestKeyAssign(this))
				{
					Singleton.Manager<ManSFX>.inst.PlayUISFX(ManSFX.UISfxType.Select);
					Text1.text = "?";
				}
			});

			Value = DefaultValue;
		}

		internal void KeyAssigned(KeyCode keyCode)
		{
			Singleton.Manager<ManSFX>.inst.PlayUISFX(ManSFX.UISfxType.Select);
			Value = keyCode;
		}

		internal void Reset()
		{
			Text1.text = m_Value == KeyCode.None ? " " : m_Value.ToString();
		}

		public override KeyCode Value
		{
			get { return m_Value; }
			set
			{
				if (value == KeyCode.Backspace) value = KeyCode.None;
				m_Value = value;
				Text1.text = m_Value == KeyCode.None ? " " : m_Value.ToString();
				m_OnValueChanged.Invoke(m_Value);
				base.Value = m_Value;
			}
		}

		public override UnityEventBase onValueChanged => m_OnValueChanged;

		public override GameObject UIElement { get; protected set; }

		public class OnChangeEvent : UnityEvent<KeyCode> { }
	}
}
