using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Exund.ModOptionsTab
{
	public class OptionRange : Option<float>
	{
		private float roundTo;
		private Slider slider;
		public OptionRange(string Name, string ModName, float DefaultValue = 0f, float MinValue = 0f, float MaxValue = 100f, float RoundTo = 1f) : base(Name, ModName, DefaultValue)
		{
			roundTo = RoundTo;
			UIElement = GameObject.Instantiate(UIElements.SliderOption_HorizontalSensivity);
			UIElement.SetActive(true);
			UIElement.name = $"SliderOption_{ModName}-{Name}";
			var text = UIElement.transform.Find("Text");
			GameObject.DestroyImmediate(text.GetComponent<UILocalisedText>());
			text.GetComponent<Text>().text = Name;			
			slider = UIElement.GetComponentInChildren<Slider>();
			slider.minValue = MinValue;
			slider.maxValue = MaxValue;
			Value = DefaultValue;
		}

		public override float Value
		{
			get { return slider.value; }
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
