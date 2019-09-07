using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Exund.ModOptionsTab
{
	/*public class OptionList : Option<int>
	{
		Dropdown dropdown;
		public OptionList(string Name, string ModName, List<string> Items, int DefaultValue = 0) : base(Name, ModName, DefaultValue)
		{
			UIElement = GameObject.Instantiate(UIElements.DropdownOption_Language);
			UIElement.SetActive(true);
			UIElement.name = $"DropdownOption_{ModName}-{Name}";
			var text = UIElement.transform.Find("Text");
			GameObject.DestroyImmediate(text.GetComponent<UILocalisedText>());
			text.GetComponent<Text>().text = Name;
			dropdown = UIElement.GetComponentInChildren<Dropdown>();
			dropdown.ClearOptions();
			dropdown.AddOptions(Items);
			Value = DefaultValue;
		}

		public string Selected
		{
			get => dropdown.options[dropdown.value].text;
		}

		public override int Value
		{
			get => dropdown.value;
			set
			{
				dropdown.value = value;
				base.Value = dropdown.value;
			}
		}

		public override UnityEventBase onValueChanged => dropdown.onValueChanged;

		public override GameObject UIElement { get; protected set; }
	}*/

	public class OptionListEnum<T> : Option<T> where T : Enum, IConvertible
	{
		/*public OptionListEnum(string Name, string ModName) : base(Name, ModName, ((T[])Enum.GetValues(typeof(T))).ToList()) { }

		public T Value {
			get => (T)Enum.ToObject(typeof(T), base.Value);
			set => base.Value = value.ToInt32(null);
		}*/

		
		Dropdown dropdown;

		public OptionListEnum(string Name, string ModName) : this(Name, ModName, (T)Enum.ToObject(typeof(T), 0)) { }
		public OptionListEnum(string Name, string ModName, T DefaultValue) : base(Name, ModName, DefaultValue)
		{
			UIElement = GameObject.Instantiate(UIElements.DropdownOption_Language);
			UIElement.SetActive(true);
			UIElement.name = $"DropdownOption_{ModName}-{Name}";
			var text = UIElement.transform.Find("Text");
			GameObject.DestroyImmediate(text.GetComponent<UILocalisedText>());
			text.GetComponent<Text>().text = Name;
			dropdown = UIElement.GetComponentInChildren<Dropdown>();
			dropdown.ClearOptions();
			dropdown.AddOptions(Enum.GetNames(typeof(T)).ToList());
			Value = DefaultValue;

			dropdown.onValueChanged.AddListener((v) => {
				this.Value = (T)Enum.ToObject(typeof(T), v);
			});
		}

		public override T Value
		{
			get => (T)Enum.ToObject(typeof(T), dropdown.value);
			set
			{
				dropdown.value = value.ToInt32(null);
				base.Value = value;
			}
		}

		public override UnityEventBase onValueChanged => dropdown.onValueChanged;

		public override GameObject UIElement { get; protected set; }
		
	}

	public class OptionList<T> : Option<int>
	{
		private List<T> items;

		Dropdown dropdown;
		public OptionList(string Name, string ModName, List<T> Items, int DefaultValue = 0) : base(Name, ModName, DefaultValue)
		{
			UIElement = GameObject.Instantiate(UIElements.DropdownOption_Language);
			UIElement.SetActive(true);
			UIElement.name = $"DropdownOption_{ModName}-{Name}";
			var text = UIElement.transform.Find("Text");
			GameObject.DestroyImmediate(text.GetComponent<UILocalisedText>());
			text.GetComponent<Text>().text = Name;
			dropdown = UIElement.GetComponentInChildren<Dropdown>();
			dropdown.ClearOptions();
			dropdown.AddOptions(Items.Select(i => i.ToString()).ToList());
			items = Items;
			Value = DefaultValue;

			dropdown.onValueChanged.AddListener((v) => { this.Value = v; });
		}

		public T Selected
		{
			get => items[base.Value];
		}

		public override int Value
		{
			get => dropdown.value;
			set
			{
				dropdown.value = value;
				base.Value = dropdown.value;
			}
		}

		public override UnityEventBase onValueChanged => dropdown.onValueChanged;

		public override GameObject UIElement { get; protected set; }
	}
}
