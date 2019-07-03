using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Exund.ModOptionsTab
{
	public class UIElements
	{
		public static readonly Font[] Fonts = Resources.FindObjectsOfTypeAll<Font>();
		public static readonly Font ExoSemiBold = Fonts.First(f => f.name == "Exo-SemiBold");
		public static readonly Font ExoRegular = Fonts.First(f => f.name == "Exo-Regular");
		public static readonly Sprite[] Sprites = Resources.FindObjectsOfTypeAll<Sprite>();
		public static readonly Sprite Option_BG = Sprites.First(f => f.name == "Option_BG");
		public static readonly Sprite Option_Content_BG = Sprites.First(f => f.name == "Option_Content_BG");
		public static readonly Sprite Options_Unticked = Sprites.First(f => f.name == "Options_Unticked");
		public static readonly Sprite Options_Ticked = Sprites.First(f => f.name == "Options_Ticked");
		public static readonly Sprite Option_Content_Highlight_BG = Sprites.First(f => f.name == "Option_Content_Highlight_BG");
		public static readonly Sprite Slider_BG = Sprites.First(f => f.name == "Slider_BG");
		public static readonly Sprite Slider_Fill_BG = Sprites.First(f => f.name == "Slider_Fill_BG");
		public static readonly Sprite Knob = Sprites.First(f => f.name == "Knob");
		public static readonly Sprite Dropdown_BG = Sprites.First(f => f.name == "Dropdown_BG");
		public static readonly Sprite Dropdown_Active_BG = Sprites.First(f => f.name == "Dropdown_Active_BG");
		public static readonly Sprite Dropdown_Highlight_BG = Sprites.First(f => f.name == "Dropdown_Highlight_BG");
		public static readonly Sprite UIMask = Sprites.First(f => f.name == "UIMask");
		public static readonly Sprite Profile_ArrowDown = Sprites.First(f => f.name == "Profile_ArrowDown");
		public static readonly Sprite ScrollBar_Small_01 = Sprites.First(f => f.name == "ScrollBar_Small_01");
		
		public static GameObject CreateOptionEntry(string Title, string Name)
		{
			GameObject OptionEntry = DefaultControls.CreateButton(new DefaultControls.Resources());
			OptionEntry.name = Name;
			var OptionEntryImage = OptionEntry.GetComponent<Image>();
			OptionEntryImage.sprite = Option_Content_BG;
			OptionEntryImage.type = Image.Type.Simple;
			OptionEntryImage.color = Color.white;
			var OptionEntryButton = OptionEntry.GetComponent<Button>();
			OptionEntryButton.transition = Selectable.Transition.SpriteSwap;
			OptionEntryButton.spriteState = new SpriteState()
			{
				highlightedSprite = Option_Content_Highlight_BG,
				pressedSprite = Option_Content_Highlight_BG
			};

			GameObject OptionEntry_Label = OptionEntry.transform.Find("Text").gameObject;
			var OptionEntry_LabelText = OptionEntry_Label.GetComponent<Text>();
			OptionEntry_LabelText.text = Title;
			OptionEntry_LabelText.alignment = TextAnchor.MiddleLeft;
			OptionEntry_LabelText.font = ExoSemiBold;
			OptionEntry_LabelText.fontSize = 15;
			OptionEntry_LabelText.color = Color.white;
			var OptionEntry_LabelRect = OptionEntry_Label.GetComponent<RectTransform>();
			OptionEntry_LabelRect.anchoredPosition3D = new Vector3(-59.7f, 0, 0);
			OptionEntry_LabelRect.anchorMin = Vector2.zero;
			OptionEntry_LabelRect.anchorMax = Vector2.one;
			OptionEntry_LabelRect.pivot = Vector2.one / 2;
			OptionEntry_LabelRect.sizeDelta = new Vector2(-157.9f, 0);
			var OptionEntry_LabelShadow = OptionEntry_Label.AddComponent<Shadow>();
			OptionEntry_LabelShadow.effectColor = new Color(0, 0, 0, 0.5f);
			OptionEntry_LabelShadow.effectDistance = new Vector2(1f, -1f);

			return OptionEntry;
		}
	}
}
