using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Nuterra.NativeOptions
{
	public class OptionToggle : Option<bool>
	{
		private Toggle toggle;

		public OptionToggle(string Name, string ModName, bool DefaultValue = false) : base(Name, ModName, DefaultValue) {
			UIElement = GameObject.Instantiate(UIElements.CheckboxOption_Prefab);
			UIElement.SetActive(true);
			UIElement.name = $"CheckboxOption_{ModName}-{Name}";
			UIElement.transform.Find("Text").GetComponent<Text>().text = Name;
			toggle = UIElement.GetComponentInChildren<Toggle>();
			Value = DefaultValue;

			toggle.onValueChanged.AddListener((v) => { this.Value = v; });
		}

		public override bool Value {
			get => toggle.isOn;
            set
			{
				toggle.isOn = value;
				base.Value = value;
			}
		}

		public override UnityEventBase onValueChanged => toggle.onValueChanged;

		public override GameObject UIElement { get; protected set; }
	}
}
