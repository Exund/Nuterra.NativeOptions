using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Exund.ModOptionsTab
{
	public class OptionText : Option<string>
	{
		InputField input;
		public OptionText(string Name, string ModName, string DefaultValue = "", int MaxLength = int.MaxValue, InputField.ContentType ContentType = InputField.ContentType.Standard) : base(Name, ModName, DefaultValue)
		{
			UIElement = GameObject.Instantiate(UIElements.InputFieldOption_Prefab);
			UIElement.SetActive(true);
			UIElement.name = $"InputFieldOption_{ModName}-{Name}";
			UIElement.transform.Find("Text").GetComponent<Text>().text = Name;
			input = UIElement.GetComponentInChildren<InputField>();
			input.characterLimit = MaxLength;
			input.contentType = ContentType;
			Value = DefaultValue;

			input.onValueChanged.AddListener((v) => { if (v != base.Value) this.Value = v; });
		}

		public override string Value
		{
			get
			{
				return input.text;
			}
			set
			{
				input.text = value;
				base.Value = value;
			}
		}

		public override UnityEventBase onValueChanged => input.onValueChanged;

		public override GameObject UIElement { get; protected set; }
	}
}
