using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Nuterra.NativeOptions
{
	public class OptionRange : Option<float>
	{
		private float roundTo;
		private Slider slider;

		public OptionRange(string Name, string ModName, float DefaultValue = 0f, float MinValue = 0f, float MaxValue = 100f, float RoundTo = 1f) : base(Name, ModName, DefaultValue)
		{
			roundTo = RoundTo;
			UIElement = GameObject.Instantiate(UIElements.SliderOption_Prefab);
			UIElement.SetActive(true);
			UIElement.name = $"SliderOption_{ModName}-{Name}";
			UIElement.transform.Find("Text").GetComponent<Text>().text = Name;
			slider = UIElement.GetComponentInChildren<Slider>();
			slider.minValue = MinValue;
			slider.maxValue = MaxValue;
			Value = DefaultValue;

			slider.onValueChanged.AddListener((v) => { this.Value = v; });
		}

		public override float Value
		{
			get => slider.value;
            set
			{
				slider.value = Mathf.RoundToInt(value / roundTo) * roundTo;
				base.Value = slider.value;
			}
		}

		public override UnityEventBase onValueChanged => slider.onValueChanged;

		public override GameObject UIElement { get; protected set; }
	}
}
