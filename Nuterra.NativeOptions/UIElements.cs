using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Nuterra.NativeOptions
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
        public static readonly Sprite Icon_BACK = Sprites.First(f => f.name == "Icon_BACK");
        public static readonly Sprite Button_BACK = Sprites.First(f => f.name == "Button_BACK");
        public static readonly Sprite Button__BACK_Highlight_BG = Sprites.First(f => f.name == "Button__BACK_Highlight_BG");
        public static readonly Sprite Button_Disabled_BG = Sprites.First(f => f.name == "Button_Disabled_BG");

        public static readonly GameObject[] GameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        public static readonly GameObject DropdownOption_Language = GameObjects.First(f => f.name == "DropdownOption_Language");
        public static readonly GameObject CheckboxOption_PauseOnFocusLost = GameObjects.First(f => f.name == "CheckboxOption_PauseOnFocusLost");
        public static readonly GameObject SliderOption_HorizontalSensivity = GameObjects.First(f => f.name == "SliderOption_HorizontalSensivity");
        public static readonly GameObject KeybindingPrefab_TurnCamera = GameObjects.First(f => f.name == "KeybindingPrefab_TurnCamera");
        public static readonly GameObject Controls_KeybindingOption_FireWeapons;

        public static readonly GameObject InputFieldOption_Prefab;
        public static readonly GameObject SliderOption_Prefab;
        public static readonly GameObject DropdownOption_Prefab;
        public static readonly GameObject CheckboxOption_Prefab;
        public static readonly GameObject KeyOption_Prefab;
        public static readonly GameObject OptionEntryPrefab;
        public static readonly GameObject CategoryPrefab;
        public static readonly GameObject Button_Back = GameObjects.First(f => f.name == "Button Back");

        public static readonly bool Pre1481 = new Version(SKU.DisplayVersion) < new Version("1.4.8.1");

        static UIElements()
        {
            OptionEntryPrefab = GameObject.Instantiate(CheckboxOption_PauseOnFocusLost);
            var EntryText = OptionEntryPrefab.transform.Find("Text").gameObject;
            EntryText.GetComponent<Text>().text = "Option Entry";
            GameObject.DestroyImmediate(EntryText.GetComponent<UILocalisedText>());
            for (var i = 0; i < OptionEntryPrefab.transform.childCount; i++)
            {
                var child = OptionEntryPrefab.transform.GetChild(i).gameObject;
                if (child != EntryText)
                {
                    GameObject.DestroyImmediate(child);
                }
            }

            InputFieldOption_Prefab = UIElements.CreateOptionEntry("", "InputFieldOption_Prefab");

            var InputFieldOption_PrefabRect = InputFieldOption_Prefab.GetComponent<RectTransform>();
            var InputFieldOption_PrefabBehaviourInputField = InputFieldOption_Prefab.AddComponent<UIOptionsBehaviourInputField>();

            var InputFieldOption_Prefab_InputField = DefaultControls.CreateInputField(new DefaultControls.Resources()
            {
                inputField = UIElements.Options_Unticked
            });
            var InputFieldOption_Prefab_InputFieldInputField = InputFieldOption_Prefab_InputField.GetComponent<InputField>();
            typeof(UIOptionsBehaviourInputField).GetField("m_Target", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(InputFieldOption_PrefabBehaviourInputField, InputFieldOption_Prefab_InputFieldInputField);
            InputFieldOption_Prefab_InputFieldInputField.transition = Selectable.Transition.None;
            InputFieldOption_Prefab_InputFieldInputField.navigation = new Navigation()
            {
                mode = Navigation.Mode.None
            };
            var InputFieldOption_Prefab_InputFieldRect = InputFieldOption_Prefab_InputField.GetComponent<RectTransform>();
            InputFieldOption_Prefab_InputFieldRect.anchoredPosition3D = new Vector3(174f, 0, 0);
            InputFieldOption_Prefab_InputFieldRect.sizeDelta = new Vector2(202f, 30f);

            var InputFieldOption_Prefab_InputField_Placeholder = InputFieldOption_Prefab_InputField.transform.Find("Placeholder").gameObject;
            var InputFieldOption_Prefab_InputField_PlaceholderRect = InputFieldOption_Prefab_InputField_Placeholder.GetComponent<RectTransform>();
            var InputFieldOption_Prefab_InputField_PlaceholderText = InputFieldOption_Prefab_InputField_Placeholder.GetComponent<Text>();
            InputFieldOption_Prefab_InputField_PlaceholderText.text = "";
            InputFieldOption_Prefab_InputField_PlaceholderText.alignment = TextAnchor.MiddleLeft;
            InputFieldOption_Prefab_InputField_PlaceholderText.font = UIElements.ExoRegular;
            InputFieldOption_Prefab_InputField_PlaceholderText.fontSize = 14;
            InputFieldOption_Prefab_InputField_PlaceholderText.fontStyle = FontStyle.Normal;
            InputFieldOption_Prefab_InputField_PlaceholderText.color = Color.white;
            InputFieldOption_Prefab_InputField_PlaceholderText.lineSpacing = 1;

            var InputFieldOption_Prefab_InputField_Text = InputFieldOption_Prefab_InputField.transform.Find("Text").gameObject;
            var InputFieldOption_Prefab_InputField_TextRect = InputFieldOption_Prefab_InputField_Text.GetComponent<RectTransform>();
            var InputFieldOption_Prefab_InputField_TextText = InputFieldOption_Prefab_InputField_Text.GetComponent<Text>();
            InputFieldOption_Prefab_InputField_TextText.text = "";
            InputFieldOption_Prefab_InputField_TextText.alignment = TextAnchor.MiddleLeft;
            InputFieldOption_Prefab_InputField_TextText.font = UIElements.ExoRegular;
            InputFieldOption_Prefab_InputField_TextText.fontSize = 14;
            InputFieldOption_Prefab_InputField_TextText.fontStyle = FontStyle.Normal;
            InputFieldOption_Prefab_InputField_TextText.color = Color.white;
            InputFieldOption_Prefab_InputField_TextText.lineSpacing = 1;
            InputFieldOption_Prefab_InputField.transform.SetParent(InputFieldOption_Prefab.transform, false);

            if (Pre1481)
            {
                KeyOption_Prefab = GameObject.Instantiate(KeybindingPrefab_TurnCamera);
                KeyOption_Prefab.SetActive(true);
                var KeybindEntryText = KeyOption_Prefab.transform.Find("Text").gameObject;
                GameObject.DestroyImmediate(KeybindEntryText.GetComponent<UILocalisedText>());
                KeybindEntryText.GetComponent<Text>().text = "";
                var KeybindButton1 = KeyOption_Prefab.transform.Find("Buttons Panel/Button").gameObject;
                KeybindButton1.GetComponent<Button>().interactable = true;
                /*var KeybindButton2 = KeybindingPrefab_TurnCamera.transform.Find("Buttons Panel/Button (2)").gameObject;
				KeybindButton2.SetActive(true);
				KeybindButton2.GetComponent<Button>().interactable = true;*/

                CategoryPrefab = GameObjects.First(f => f.name.StartsWith("_EmptySpace_ (1)"));
            }
            else
            {
                Controls_KeybindingOption_FireWeapons = GameObjects.First(f => f.name == "Controls_KeybindingOption FireWeapons");
                UIElements.KeyOption_Prefab = GameObject.Instantiate(Controls_KeybindingOption_FireWeapons.transform.Find("FirstRow (1)").gameObject);

                var rect = KeyOption_Prefab.GetComponent<RectTransform>();
                rect.pivot = Vector2.up;
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 590f);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
                GameObject.DestroyImmediate(KeyOption_Prefab.transform.Find("AxisName").GetComponent<UILocalisedText>());
                GameObject.DestroyImmediate(KeyOption_Prefab.transform.Find("Name").gameObject);
                GameObject.DestroyImmediate(KeyOption_Prefab.transform.Find("AxisDescription").gameObject);

                var btn1 = KeyOption_Prefab.transform.Find("ButtonPos1").gameObject;
                var btn1Rect = btn1.GetComponent<RectTransform>();
                btn1Rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, btn1Rect.sizeDelta.x, btn1Rect.sizeDelta.x);
                GameObject.DestroyImmediate(btn1.GetComponent<UIKeyBindButton>());
                btn1.GetComponent<Button>().onClick.RemoveAllListeners();
                btn1.SetActive(false);

                var btn2 = KeyOption_Prefab.transform.Find("ButtonPos2");
                var btn2Rect = btn2.GetComponent<RectTransform>();
                btn2Rect.pivot = btn2Rect.anchorMin = btn2Rect.anchorMax = new Vector2(1, 0.5f);
                btn2Rect.anchoredPosition3D = Vector3.zero;
                btn2Rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, btn2Rect.sizeDelta.x);
                btn2.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.DestroyImmediate(btn2.GetComponent<UIKeyBindButton>());

                CategoryPrefab = GameObject.Instantiate(GameObjects.First(f => f.name == "Heading_Movement"));
                var catRect = CategoryPrefab.GetComponent<RectTransform>();
                catRect.sizeDelta = new Vector2(590.9f, 50.0f);
                GameObject.DestroyImmediate(CategoryPrefab.GetComponentInChildren<UILocalisedText>());
            }

            SliderOption_Prefab = GameObject.Instantiate(SliderOption_HorizontalSensivity);
            var SliderOption_Prefab_Text = SliderOption_Prefab.transform.Find("Text");
            GameObject.DestroyImmediate(SliderOption_Prefab_Text.GetComponent<UILocalisedText>());
            var SliderOption_Prefab_TextText = SliderOption_Prefab_Text.GetComponent<Text>();
            SliderOption_Prefab_TextText.fontSize = 14;
            SliderOption_Prefab_TextText.font = UIElements.ExoRegular;
            SliderOption_Prefab_TextText.enabled = true;

            DropdownOption_Prefab = GameObject.Instantiate(DropdownOption_Language);
            var DropdownOption_Prefab_Text = DropdownOption_Prefab.transform.Find("Text");
            GameObject.DestroyImmediate(DropdownOption_Prefab_Text.GetComponent<UILocalisedText>());
            DropdownOption_Prefab.GetComponentInChildren<Dropdown>().ClearOptions();

            CheckboxOption_Prefab = GameObject.Instantiate(CheckboxOption_PauseOnFocusLost);
            var CheckboxOption_Prefab_Text = CheckboxOption_Prefab.transform.Find("Text");
            GameObject.DestroyImmediate(CheckboxOption_Prefab_Text.GetComponent<UILocalisedText>());
        }

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
            OptionEntry_LabelRect.pivot = Vector2.one * 0.5f;
            OptionEntry_LabelRect.sizeDelta = new Vector2(-157.9f, 0);
            var OptionEntry_LabelShadow = OptionEntry_Label.AddComponent<Shadow>();
            OptionEntry_LabelShadow.effectColor = new Color(0, 0, 0, 0.5f);
            OptionEntry_LabelShadow.effectDistance = new Vector2(1f, -1f);

            return OptionEntry;
        }

        public static GameObject CreateCategoryEntry(string Title)
        {
            GameObject CategoryEntry = GameObject.Instantiate(CategoryPrefab);
            CategoryEntry.SetActive(true);
            CategoryEntry.GetComponentInChildren<Text>().text = Title;

            return CategoryEntry;
        }
    }
}
