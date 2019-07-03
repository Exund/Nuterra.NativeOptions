using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace Exund.ModOptionsTab
{
	public class ModOptionsTabMod
	{
		public static void Load()
		{
			var harmony = HarmonyInstance.Create("Exund.ModOptionsTab");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}

	public class UIUtilities
	{
		public static string GetComponentTree(GameObject go, string tab = "")
		{
			StringBuilder tree = new StringBuilder(tab + go.name);
			tree.AppendLine();
			tree.AppendFormat("{0}\tActive: {1}", tab, go.activeSelf);
			tree.AppendFormat("\n{0}\tComponents :\n", tab);
			foreach (Component item in go.GetComponents<Component>())
			{
				tree.AppendFormat("{0}\t\t{1} {2}", tab, item.GetType().ToString(), item.name);
				try
				{
					if (item is RectTransform rect)
					{
						tree.AppendFormat("\n{0}\t\t\tanchoredPosition3D {1}", tab, rect.anchoredPosition3D.ToString());
						tree.AppendFormat("\n{0}\t\t\tanchorMin ({1}, {2})", tab, rect.anchorMin.x, rect.anchorMin.y);
						tree.AppendFormat("\n{0}\t\t\tanchorMax ({1}, {2})", tab, rect.anchorMax.x, rect.anchorMax.y);
						tree.AppendFormat("\n{0}\t\t\toffsetMin {1}", tab, rect.offsetMin.ToString());
						tree.AppendFormat("\n{0}\t\t\toffsetMax {1}", tab, rect.offsetMax.ToString());
						tree.AppendFormat("\n{0}\t\t\tpivot {1}", tab, rect.pivot.ToString());
						tree.AppendFormat("\n{0}\t\t\tsizeDelta {1}", tab, rect.sizeDelta.ToString());
						tree.AppendFormat("\n{0}\t\t\tlocalScale {1}", tab, rect.localScale.ToString());
					}
					if (item is HorizontalOrVerticalLayoutGroup lg)
					{
						tree.AppendFormat("\n{0}\t\t\tchildAlignment {1}", tab, lg.childAlignment.ToString());
						tree.AppendFormat("\n{0}\t\t\tchildControlWidth {1}", tab, lg.childControlWidth);
						tree.AppendFormat("\n{0}\t\t\tchildControlHeight {1}", tab, lg.childControlHeight);
						tree.AppendFormat("\n{0}\t\t\tchildForceExpandWidth {1}", tab, lg.childForceExpandWidth);
						tree.AppendFormat("\n{0}\t\t\tchildForceExpandHeight {1}", tab, lg.childForceExpandHeight);
						tree.AppendFormat("\n{0}\t\t\tlayoutPriority {1}", tab, lg.layoutPriority);
						tree.AppendFormat("\n{0}\t\t\tspacing {1}", tab, lg.spacing);
						tree.AppendFormat("\n{0}\t\t\tpadding {1}", tab, lg.padding);
						tree.AppendFormat("\n{0}\t\t\tminWidth {1}", tab, lg.minWidth);
						tree.AppendFormat("\n{0}\t\t\tminHeight {1}", tab, lg.minHeight);
						tree.AppendFormat("\n{0}\t\t\tpreferredWidth {1}", tab, lg.preferredWidth);
						tree.AppendFormat("\n{0}\t\t\tpreferredHeight {1}", tab, lg.preferredHeight);
						tree.AppendFormat("\n{0}\t\t\tflexibleWidth {1}", tab, lg.flexibleWidth);
						tree.AppendFormat("\n{0}\t\t\tflexibleHeight {1}", tab, lg.flexibleHeight);
					}
					if (item is Text t)
					{
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "text", t.text);
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "alignment", t.alignment.ToString());
						if(t.font) tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "font", t.font.name);
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "fontSize", t.fontSize);
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "fontStyle", t.fontStyle.ToString());
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "color", t.color.ToString());
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "lineSpacing", t.lineSpacing);
					}
					if (item is Shadow s)
					{
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "effectColor", s.effectColor.ToString());
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "effectColor", s.effectDistance.ToString());
					}
					if (item is Image i)
					{
						if (i.sprite) tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "sprite", i.sprite.name);
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "color", i.color);
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "type", i.type.ToString());
					}
					if (item is Selectable se)
					{
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "interactable", se.interactable);
						if (se.image)
						{
							tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "image", se.image.name);
							if (se.image.sprite) tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "sprite", se.image.sprite.name);
							tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "color", se.image.color);
							tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "type", se.image.type.ToString());
						}
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "transition", se.transition.ToString());
						switch (se.transition)
						{
							case Selectable.Transition.ColorTint:
								var colors = se.colors;
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "normalColor", colors.normalColor.ToString());
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "highlightedColor", colors.highlightedColor.ToString());
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "pressedColor", colors.pressedColor.ToString());
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "disabledColor", colors.disabledColor.ToString());
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "colorMultiplier", colors.colorMultiplier.ToString());
								tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "fadeDuration", colors.fadeDuration.ToString());
								break;
							case Selectable.Transition.SpriteSwap:
								var state = se.spriteState;
								if (state.highlightedSprite) tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "highlightedSprite", state.highlightedSprite.name);
								if (state.pressedSprite) tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "pressedSprite", state.pressedSprite.name);
								if (state.disabledSprite) tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "disabledSprite", state.disabledSprite.name);
								break;
							default: break;
						}
					}
					if(item is Dropdown dd)
					{
						
					}
					tree.AppendLine();
				} catch { }
			}
			tree.AppendLine();
			tree.AppendFormat("{0}\tChildrens :\n", tab);
			for (int i = 0; i < go.transform.childCount; i++)
			{
				if (!go.transform.GetChild(i).gameObject.name.Contains("ObjectHighlight"))
				{
					tree.Append(GetComponentTree(go.transform.GetChild(i).gameObject, tab + "\t"));
				}
			}
			return tree.ToString();
		}

		public static GameObject CreateOptionEntry(string Title, string Name)
		{
			return new GameObject();
		}
	}

	static class Patches
	{
		static FieldInfo m_OptionsTypeCount;
		static FieldInfo m_OptionsTabs;
		static FieldInfo m_OptionsElements;
		static FieldInfo BehaviourToggle_m_Target;
		static FieldInfo BehaviourSlider_m_Target;
		static FieldInfo BehaviourSlider_m_FSM;
		static MethodInfo BehaviourSlider_OnPool;
		//static FieldInfo BehaviourSeed_m_Target;
		static FieldInfo BehaviourInputField_m_Target;
		static FieldInfo BehaviourDropdown_m_Target;
		static FieldInfo BehaviourDropdown_m_FSM;
		static MethodInfo BehaviourDropdown_OnPool;
		static MethodInfo BehaviourDropdown_OnEnable;

		static Patches()
		{
			Type UIScreenOptionsType = typeof(UIScreenOptions);
			BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			m_OptionsTypeCount = UIScreenOptionsType.GetField("m_OptionsTypeCount", privateFlags);
			m_OptionsTabs = UIScreenOptionsType.GetField("m_OptionsTabs", privateFlags);
			m_OptionsElements = UIScreenOptionsType.GetField("m_OptionsElements", privateFlags);	
			BehaviourToggle_m_Target = typeof(UIOptionsBehaviourToggle).GetField("m_Target", privateFlags);
			Type BehaviourSliderType = typeof(UIOptionsBehaviourSlider);
			BehaviourSlider_m_Target = BehaviourSliderType.GetField("m_Target", privateFlags);
			BehaviourSlider_m_FSM = BehaviourSliderType.GetField("m_FSM", privateFlags);
			BehaviourSlider_OnPool = BehaviourSliderType.GetMethod("OnPool", privateFlags);
			//BehaviourSeed_m_Target = typeof(UIOptionsBehaviourSeedGenerator).GetField("m_Target", privateFlags);
			BehaviourInputField_m_Target = typeof(UIOptionsBehaviourInputField).GetField("m_Target", privateFlags);
			Type BehaviourDropdownType = typeof(UIOptionsBehaviourDropdown);
			BehaviourDropdown_m_Target = BehaviourDropdownType.GetField("m_Target", privateFlags);
			BehaviourDropdown_m_FSM = BehaviourDropdownType.GetField("m_FSM", privateFlags);
			BehaviourDropdown_OnPool = BehaviourDropdownType.GetMethod("OnPool", privateFlags);
			BehaviourDropdown_OnEnable = BehaviourDropdownType.GetMethod("OnEnable", privateFlags);
		}

		/*[HarmonyPatch(typeof(UIOptionsBehaviourSeedGenerator), "OnEnable")]
		internal static class UIOptionsBehaviourSeedGeneratorOnEnable
		{
			private static void Postfix(ref UIOptionsBehaviourSeedGenerator __instance)
			{
				var input = (InputField)BehaviourSeed_m_Target.GetValue(__instance);
				Console.WriteLine(UIUtilities.GetComponentTree(input.gameObject));
			}
		}*/

		[HarmonyPatch(typeof(UIOptionsBehaviourSlider), "OnEnable")]
		internal static class UIOptionsBehaviourSliderOnEnable
		{
			private static void Prefix(ref UIOptionsBehaviourSlider __instance)
			{
				if (BehaviourSlider_m_FSM.GetValue(__instance) == null) BehaviourSlider_OnPool.Invoke(__instance, new object[0]);
			}
		}

		/*[HarmonyPatch(typeof(UIOptionsBehaviourDropdown), "OnEnable")]
		internal static class UIOptionsBehaviourDropdownOnEnable
		{
			private static void Prefix(ref UIOptionsBehaviourDropdown __instance)
			{
				if (BehaviourDropdown_m_FSM.GetValue(__instance) == null) BehaviourDropdown_OnPool.Invoke(__instance, new object[0]);
			}
		}*/

		[HarmonyPatch(typeof(UIScreenOptions), "Awake")]
		internal static class UIScreenOptionsAwake
		{
			private static void Postfix(ref UIScreenOptions __instance)
			{
                // Add a tab to the settings
				int optionsCount = (int)m_OptionsTypeCount.GetValue(__instance) + 1;
				m_OptionsTypeCount.SetValue(__instance, optionsCount);
				var ratio = (float)(optionsCount - 1f) / (float)(optionsCount);

				// Get settings tabs to an array
				Toggle[] optionsTabs = new Toggle[optionsCount];
				((Toggle[])m_OptionsTabs.GetValue(__instance)).CopyTo(optionsTabs, 0);

                // Copy last tab to new tab
				Toggle modsToggle = GameObject.Instantiate<Toggle>(optionsTabs[optionsCount - 2]);
				modsToggle.transform.SetParent(optionsTabs[optionsCount - 2].transform.parent, false);
				modsToggle.gameObject.SetActive(true);

				Console.WriteLine(UIUtilities.GetComponentTree(optionsTabs[0].gameObject));
				Console.WriteLine(UIUtilities.GetComponentTree(modsToggle.gameObject));

				// Clear and assign name
				GameObject.DestroyImmediate(modsToggle.gameObject.GetComponentInChildren<UILocalisedText>());
				modsToggle.GetComponentInChildren<Text>().text = "MODS";

				optionsTabs[optionsCount - 1] = modsToggle;
				// Move tabs
				/*var ratioVector = new Vector2(ratio, 1f);
				float width = 1f;*/
				var x = 0f;
				foreach (var toggle in optionsTabs)
				{
					if (!toggle) continue;
					RectTransform toggleRect = toggle.GetComponent<RectTransform>();
					if (toggleRect)
					{
						/*//Vector2 anchorMin = toggleRect.anchorMin;
						//anchorMin.x *= 0.8f;
						toggleRect.anchorMin *= ratioVector;
						//Vector2 anchorMax = toggleRect.anchorMax;
						//anchorMax.x *= 0.8f;
						toggleRect.anchorMax *= ratioVector;
						toggleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, toggleRect.rect.width * ratio);
						width = toggleRect.rect.width * ratio;*/
						Vector2 anchorMin = toggleRect.anchorMin;
						anchorMin.x = x;
						toggleRect.anchorMin = anchorMin;
						x += 0.2f;
						Vector2 anchorMax = toggleRect.anchorMax;
						anchorMax.x = x;
						toggleRect.anchorMax = anchorMax;
					}
				}

                /*// Position new tab
				RectTransform modToggleRect = modsToggle.GetComponent<RectTransform>();
				var rect0 = optionsTabs[0].GetComponent<RectTransform>();
				var anchorMin = modToggleRect.anchorMin;// /= ratioVector;
				anchorMin.x = 0.815f;
				modToggleRect.anchorMin = anchorMin;
				var anchorMax = modToggleRect.anchorMax;// /= ratioVector;
				anchorMax.x = 0.965f;
				modToggleRect.anchorMax = anchorMax;

				modToggleRect.anchoredPosition = Vector2.zero;

				//modToggleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
				//modToggleRect.sizeDelta = optionsTabs[0].GetComponent<RectTransform>().sizeDelta;

				optionsTabs[optionsCount - 1] = modsToggle;*/

				Console.WriteLine(UIUtilities.GetComponentTree(optionsTabs[0].gameObject));
				Console.WriteLine(UIUtilities.GetComponentTree(modsToggle.gameObject));


				// Get the tab panels to an array
				UIOptions[] optionsElements = new UIOptions[optionsCount];
				((UIOptions[])m_OptionsElements.GetValue(__instance)).CopyTo(optionsElements, 0);

				Transform parent = optionsElements[0].gameObject.transform.parent;

				/*// Acquire resources
				var fonts = Resources.FindObjectsOfTypeAll<Font>();
				var ExoSemiBold = fonts.First(f => f.name == "Exo-SemiBold");
				var ExoRegular = fonts.First(f => f.name == "Exo-Regular");
				var sprites = Resources.FindObjectsOfTypeAll<Sprite>();
				var Option_BG = sprites.First(f => f.name == "Option_BG");
				var Option_Content_BG = sprites.First(f => f.name == "Option_Content_BG");
				var Options_Unticked = sprites.First(f => f.name == "Options_Unticked");
				var Options_Ticked = sprites.First(f => f.name == "Options_Ticked");
				var Option_Content_Highlight_BG = sprites.First(f => f.name == "Option_Content_Highlight_BG");
				var Slider_BG = sprites.First(f => f.name == "Slider_BG");
				var Slider_Fill_BG = sprites.First(f => f.name == "Slider_Fill_BG");
				var Knob = sprites.First(f => f.name == "Knob");*/

				// Give new tab a UIOptions panel and position to reference
				DefaultControls.Resources resources = default(DefaultControls.Resources);
				GameObject Mods = DefaultControls.CreatePanel(resources);
				Mods.name = "Mods";
				GameObject.DestroyImmediate(Mods.GetComponent<Image>());		
				RectTransform panel_rect = Mods.GetComponent<RectTransform>();
				RectTransform reference_rect = optionsElements[0].gameObject.GetComponent<RectTransform>();
				panel_rect.anchorMin = reference_rect.anchorMin;
				panel_rect.anchorMax = reference_rect.anchorMax;
				panel_rect.anchoredPosition3D = reference_rect.anchoredPosition3D;
				panel_rect.sizeDelta = reference_rect.sizeDelta;
				panel_rect.pivot = reference_rect.pivot;
				optionsElements[optionsCount - 1] = Mods.AddComponent<UIOptionsMod>();
				Mods.transform.SetParent(parent, false);

				// Allow tab to be activatable
				var instance = __instance;
				modsToggle.onValueChanged.AddListener(delegate (bool set)
				{
					if (set)
					{
						instance.ShowOptions((UIScreenOptions.OptionsType)(optionsCount - 1));
						Console.WriteLine(UIUtilities.GetComponentTree(optionsElements[0].gameObject));
						Console.WriteLine(UIUtilities.GetComponentTree(Mods));
					}
				});
				((UIOptionsMod)optionsElements[optionsCount - 1]).tab_toggle = modsToggle;

				// Create UI elements
				GameObject top_panel = DefaultControls.CreatePanel(resources);
				top_panel.name = "Top Panel";
				GameObject.DestroyImmediate(top_panel.GetComponent<Image>());
				var topRect = top_panel.GetComponent<RectTransform>();
				topRect.anchoredPosition3D = new Vector3(0, 285f, 0);
				topRect.anchorMin = Vector2.zero;
				topRect.anchorMax = Vector2.one;
				topRect.pivot = Vector2.one / 2;
				topRect.sizeDelta = new Vector2(-6.3f, -569.0f);
				var group = top_panel.AddComponent<HorizontalLayoutGroup>();
				group.childAlignment = TextAnchor.MiddleCenter;
				group.childControlWidth = true;
				group.childControlHeight = true;
				group.childForceExpandWidth = true;
				group.childForceExpandHeight = true;
				group.spacing = 0;
				group.padding = new RectOffset(0,0,0,0);

				top_panel.transform.SetParent(Mods.transform, false);

				GameObject category1 = DefaultControls.CreatePanel(resources);
				category1.name = "Category 1";
				GameObject.DestroyImmediate(category1.GetComponent<Image>());
				category1.transform.SetParent(top_panel.transform, false);

				GameObject category1_title = DefaultControls.CreateText(resources);
				category1_title.name = "Title";
				var title1 = category1_title.GetComponent<Text>();
				title1.text = "LEFT SIDE";
				title1.alignment = TextAnchor.MiddleCenter;
				title1.font = UIElements.ExoSemiBold;
				title1.fontSize = 16;
				title1.color = Color.white;
				var title1Rect = category1_title.GetComponent<RectTransform>();
				title1Rect.anchoredPosition3D = new Vector3(0, -2.1f, 0);
				title1Rect.pivot = title1Rect.anchorMin = title1Rect.anchorMax = Vector2.one / 2;
				title1Rect.sizeDelta = new Vector2(160f, 55.6f);
				var title1Shadow = category1_title.AddComponent<Shadow>();
				title1Shadow.effectColor = new Color(0, 0, 0, 0.5f);
				title1Shadow.effectDistance = new Vector2(1f, -1f);
				category1_title.transform.SetParent(category1.transform, false);

				GameObject category2 = DefaultControls.CreatePanel(resources);
				category2.name = "Category 2";
				GameObject.DestroyImmediate(category2.GetComponent<Image>());
				category2.transform.SetParent(top_panel.transform, false);

				GameObject category2_title = DefaultControls.CreateText(resources);
				category2_title.name = "Title";
				var title2 = category2_title.GetComponent<Text>();
				title2.text = "RIGHT SIDE";
				title2.alignment = TextAnchor.MiddleCenter;
				title2.font = UIElements.ExoSemiBold;
				title2.fontSize = 16;
				title2.color = Color.white;
				var title2Rect = category2_title.GetComponent<RectTransform>();
				title2Rect.anchoredPosition3D = new Vector3(0, -2.1f, 0);
				title2Rect.pivot = title2Rect.anchorMin = title2Rect.anchorMax = Vector2.one / 2;
				title2Rect.sizeDelta = new Vector2(160f, 55.6f);
				var title2Shadow = category2_title.AddComponent<Shadow>();
				title2Shadow.effectColor = new Color(0, 0, 0, 0.5f);
				title2Shadow.effectDistance = new Vector2(1f, -1f);
				category2_title.transform.SetParent(category2.transform, false);


				GameObject mid_panel = DefaultControls.CreatePanel(resources);
				mid_panel.name = "Mid Panel";
				var midImage = mid_panel.GetComponent<Image>();
				midImage.sprite = UIElements.Option_BG;
				midImage.type = Image.Type.Simple;
				midImage.color = Color.white;
				var midRect = mid_panel.GetComponent<RectTransform>();
				midRect.anchoredPosition3D = new Vector3(0, -21.6f, 0);
				midRect.anchorMin = Vector2.zero;
				midRect.anchorMax = Vector2.one;
				midRect.pivot = Vector2.one / 2;
				midRect.sizeDelta = new Vector2(-6.3f, -74.8f);
				var midGroup = mid_panel.AddComponent<HorizontalLayoutGroup>();
				midGroup.childAlignment = TextAnchor.UpperLeft;
				midGroup.childControlWidth = true;
				midGroup.childControlHeight = true;
				midGroup.childForceExpandWidth = true;
				midGroup.childForceExpandHeight = true;
				midGroup.spacing = 2f;
				midGroup.padding = new RectOffset(2, 2, 2, 2);
				mid_panel.transform.SetParent(Mods.transform, false);

				GameObject content1 = DefaultControls.CreatePanel(resources);
				content1.name = "Content 1";
				var content1Image = content1.GetComponent<Image>();
				content1Image.sprite = UIElements.Option_Content_BG;
				content1Image.type = Image.Type.Simple;
				content1Image.color = Color.white;
				var content1Group = content1.AddComponent<VerticalLayoutGroup>();
				content1Group.childAlignment = TextAnchor.UpperCenter;
				content1Group.childControlWidth = true;
				content1Group.childControlHeight = true;
				content1Group.childForceExpandWidth = true;
				content1Group.childForceExpandHeight = false;
				content1Group.spacing = 2f;
				content1Group.padding = new RectOffset(0, 0, 5, 0);
				content1.transform.SetParent(mid_panel.transform, false);

				#region Toggle
				// Checkbox element for testing
				GameObject CheckboxOption_Test = UIElements.CreateOptionEntry("Test", "CheckboxOption_Test");

				CheckboxOption_Test.AddComponent<UINavigationEntryPoint>();
				var CheckboxOption_TestBehaviourToggle = CheckboxOption_Test.AddComponent<UIOptionsBehaviourToggle>();

				CheckboxOption_Test.transform.SetParent(content1.transform, false);

				GameObject temp = DefaultControls.CreateToggle(new DefaultControls.Resources() {
					standard = UIElements.Options_Unticked,
					checkmark = UIElements.Options_Ticked
				});
				var CheckboxOption_Test_Toggle = temp.transform.Find("Background").gameObject;
				CheckboxOption_Test_Toggle.name = "TickBox";
				var CheckboxOption_Test_ToggleToggle = CheckboxOption_Test_Toggle.AddComponent<Toggle>();
				BehaviourToggle_m_Target.SetValue(CheckboxOption_TestBehaviourToggle, CheckboxOption_Test_ToggleToggle);
				var image = CheckboxOption_Test_Toggle.GetComponent<Image>();
				image.type = Image.Type.Simple;
				CheckboxOption_Test_ToggleToggle.image = image;
				CheckboxOption_Test_ToggleToggle.transition = Selectable.Transition.ColorTint;
				var CheckboxOption_Test_ToggleRect = CheckboxOption_Test_Toggle.GetComponent<RectTransform>();
				CheckboxOption_Test_ToggleRect.anchoredPosition3D = Vector3.zero;
				CheckboxOption_Test_ToggleRect.anchorMin = new Vector2(0.9245809f, 0.3063623f);
				CheckboxOption_Test_ToggleRect.anchorMax = new Vector2(0.9720991f, 0.6936374f);
				CheckboxOption_Test_ToggleRect.pivot = Vector2.one / 2;
				CheckboxOption_Test_ToggleRect.sizeDelta = Vector2.zero;
				CheckboxOption_Test_Toggle.transform.SetParent(CheckboxOption_Test.transform, false);

				var CheckboxOption_Test_Toggle_TickedSprite = CheckboxOption_Test_Toggle.transform.Find("Checkmark").gameObject;
				CheckboxOption_Test_Toggle_TickedSprite.name = "TickedSprite";
				var CheckboxOption_Test_Toggle_TickedSpriteRect = CheckboxOption_Test_Toggle_TickedSprite.GetComponent<RectTransform>();
				CheckboxOption_Test_Toggle_TickedSpriteRect.anchoredPosition3D = Vector3.zero;
				CheckboxOption_Test_Toggle_TickedSpriteRect.anchorMin = Vector2.zero;
				CheckboxOption_Test_Toggle_TickedSpriteRect.anchorMax = Vector2.one;
				CheckboxOption_Test_Toggle_TickedSpriteRect.pivot = Vector2.one / 2;
				CheckboxOption_Test_Toggle_TickedSpriteRect.sizeDelta = Vector2.zero;
				CheckboxOption_Test_ToggleToggle.graphic = CheckboxOption_Test_Toggle_TickedSprite.GetComponent<Image>();
				GameObject.DestroyImmediate(temp);
				#endregion Toggle

				GameObject content2 = DefaultControls.CreatePanel(resources);
				content2.name = "Content 2";
				var content2Image = content2.GetComponent<Image>();
				content2Image.sprite = UIElements.Option_Content_BG;
				content2Image.type = Image.Type.Simple;
				content2Image.color = Color.white;
				var content2Group = content2.AddComponent<VerticalLayoutGroup>();
				content2Group.childAlignment = TextAnchor.UpperCenter;
				content2Group.childControlWidth = true;
				content2Group.childControlHeight = true;
				content2Group.childForceExpandWidth = true;
				content2Group.childForceExpandHeight = false;
				content2Group.spacing = 2f;
				content2Group.padding = new RectOffset(0, 0, 5, 0);
				content2.transform.SetParent(mid_panel.transform, false);

				#region Slider
				GameObject SliderOption_Test2 = UIElements.CreateOptionEntry("Test2", "SliderOption_Test2");		

				var SliderOption_Test2Rect = SliderOption_Test2.GetComponent<RectTransform>();
				var SliderOption_Test2BehaviourSlider = SliderOption_Test2.AddComponent<UIOptionsBehaviourSlider>();

				SliderOption_Test2.transform.SetParent(content2.transform, false);

				var SliderOption_Test2_Slider = DefaultControls.CreateSlider(new DefaultControls.Resources()
				{
					background = UIElements.Slider_BG,
					standard = UIElements.Slider_Fill_BG,
					knob = UIElements.Knob
				});
				var SliderOption_Test2_SliderSlider = SliderOption_Test2_Slider.GetComponent<Slider>();
				BehaviourSlider_m_Target.SetValue(SliderOption_Test2BehaviourSlider, SliderOption_Test2_SliderSlider);
				SliderOption_Test2_SliderSlider.transition = Selectable.Transition.None;
				var SliderOption_Test2_SliderRect = SliderOption_Test2_Slider.GetComponent<RectTransform>();
				SliderOption_Test2_SliderRect.anchoredPosition3D = new Vector3(173.9f, 0, 0);
				SliderOption_Test2_SliderRect.sizeDelta = new Vector2(201.3f, 23f);

				SliderOption_Test2_Slider.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 4.8f);
				var SliderOption_Test2_Slider_FillArea = SliderOption_Test2_Slider.transform.Find("Fill Area").gameObject;
				var SliderOption_Test2_Slider_FillAreaRect = SliderOption_Test2_Slider_FillArea.GetComponent<RectTransform>();
				SliderOption_Test2_Slider_FillAreaRect.anchoredPosition3D = new Vector3(0.8f, 0, 0);
				SliderOption_Test2_Slider_FillAreaRect.sizeDelta = new Vector2(-1.5f, 4.8f);
				var SliderOption_Test2_Slider_FillArea_FillRect = SliderOption_Test2_Slider_FillArea.transform.Find("Fill").GetComponent<RectTransform>();
				SliderOption_Test2_Slider_FillArea_FillRect.anchoredPosition3D = new Vector3(-0.5f, 0, 0);
				SliderOption_Test2_Slider_FillArea_FillRect.anchorMax = new Vector2(0.2075338f, 1f);
				SliderOption_Test2_Slider_FillArea_FillRect.sizeDelta = new Vector2(0, -3f);
				var SliderOption_Test2_Slider_HandleSlideArea = SliderOption_Test2_Slider.transform.Find("Handle Slide Area").gameObject;
				SliderOption_Test2_Slider_HandleSlideArea.GetComponent<RectTransform>().sizeDelta = new Vector2(-5.8f, 0);
				/*var SliderOption_Test2_Slider_HandleSlideArea_HandleRect = SliderOption_Test2_Slider_HandleSlideArea.transform.Find("Handle").GetComponent<RectTransform>();
				SliderOption_Test2_Slider_HandleSlideArea_HandleRect.anchoredPosition3D = new Vector3(-1.9f, 0, 0);
				SliderOption_Test2_Slider_HandleSlideArea_HandleRect.anchorMin = new Vector2(0.2f, 0); //0.20753381f
				SliderOption_Test2_Slider_HandleSlideArea_HandleRect.anchorMax = new Vector2(0.2f, 1f);
				SliderOption_Test2_Slider_HandleSlideArea_HandleRect.sizeDelta = new Vector2(10.8f, 0);*/
				SliderOption_Test2_Slider_HandleSlideArea.transform.Find("Handle").gameObject.SetActive(false);

				SliderOption_Test2_Slider.transform.SetParent(SliderOption_Test2.transform, false);
				#endregion Slider

				#region InputField
				GameObject InputFieldOption_Test3 = UIElements.CreateOptionEntry("Test3", "InputFieldOption_Test3");

				var InputFieldOption_Test3Rect = InputFieldOption_Test3.GetComponent<RectTransform>();
				var InputFieldOption_Test3BehaviourInputField = InputFieldOption_Test3.AddComponent<UIOptionsBehaviourInputField>();

				InputFieldOption_Test3.transform.SetParent(content1.transform, false);

				var InputFieldOption_Test3_InputField = DefaultControls.CreateInputField(new DefaultControls.Resources()
				{
					inputField = UIElements.Options_Unticked
				});
				var InputFieldOption_Test3_InputFieldInputField = InputFieldOption_Test3_InputField.GetComponent<InputField>();
				BehaviourInputField_m_Target.SetValue(InputFieldOption_Test3BehaviourInputField, InputFieldOption_Test3_InputFieldInputField);
				InputFieldOption_Test3_InputFieldInputField.transition = Selectable.Transition.None;
				var InputFieldOption_Test3_InputFieldRect = InputFieldOption_Test3_InputField.GetComponent<RectTransform>();
				InputFieldOption_Test3_InputFieldRect.anchoredPosition3D = new Vector3(174f, 0, 0);
				InputFieldOption_Test3_InputFieldRect.sizeDelta = new Vector2(202f, 30f);

				var InputFieldOption_Test3_InputField_Placeholder = InputFieldOption_Test3_InputField.transform.Find("Placeholder").gameObject;
				var InputFieldOption_Test3_InputField_PlaceholderRect = InputFieldOption_Test3_InputField_Placeholder.GetComponent<RectTransform>();
				var InputFieldOption_Test3_InputField_PlaceholderText = InputFieldOption_Test3_InputField_Placeholder.GetComponent<Text>();
				InputFieldOption_Test3_InputField_PlaceholderText.text = "";
				InputFieldOption_Test3_InputField_PlaceholderText.alignment = TextAnchor.MiddleLeft;
				InputFieldOption_Test3_InputField_PlaceholderText.font = UIElements.ExoRegular;
				InputFieldOption_Test3_InputField_PlaceholderText.fontSize = 14;
				InputFieldOption_Test3_InputField_PlaceholderText.fontStyle = FontStyle.Normal;
				InputFieldOption_Test3_InputField_PlaceholderText.color = Color.white;
				InputFieldOption_Test3_InputField_PlaceholderText.lineSpacing = 1;

				var InputFieldOption_Test3_InputField_Text = InputFieldOption_Test3_InputField.transform.Find("Text").gameObject;
				var InputFieldOption_Test3_InputField_TextRect = InputFieldOption_Test3_InputField_Text.GetComponent<RectTransform>();
				var InputFieldOption_Test3_InputField_TextText = InputFieldOption_Test3_InputField_Text.GetComponent<Text>();
				InputFieldOption_Test3_InputField_TextText.text = "";
				InputFieldOption_Test3_InputField_TextText.alignment = TextAnchor.MiddleLeft;
				InputFieldOption_Test3_InputField_TextText.font = UIElements.ExoRegular;
				InputFieldOption_Test3_InputField_TextText.fontSize = 14;
				InputFieldOption_Test3_InputField_TextText.fontStyle = FontStyle.Normal;
				InputFieldOption_Test3_InputField_TextText.color = Color.white;
				InputFieldOption_Test3_InputField_TextText.lineSpacing = 1;

				InputFieldOption_Test3_InputField.transform.SetParent(InputFieldOption_Test3.transform, false);
				#endregion InputField

				#region Dropdown
				GameObject DropdownOption_Test4 = UIElements.CreateOptionEntry("Test4", "DropdownOption_Test4");

				var DropdownOption_Test4Rect = DropdownOption_Test4.GetComponent<RectTransform>();
				var DropdownOption_Test4BehaviourDropdown = DropdownOption_Test4.AddComponent<UIOptionsBehaviourDropdown>();

				DropdownOption_Test4.transform.SetParent(content2.transform, false);

				var DropdownOption_Test4_Dropdown = DefaultControls.CreateDropdown(new DefaultControls.Resources()
				{
					standard = UIElements.Dropdown_BG,
					mask = UIElements.UIMask,
					dropdown = UIElements.Profile_ArrowDown,
					background = UIElements.ScrollBar_Small_01
				});
				var DropdownOption_Test4_DropdownDropdown = DropdownOption_Test4_Dropdown.GetComponent<Dropdown>();
				BehaviourDropdown_m_Target.SetValue(DropdownOption_Test4BehaviourDropdown, DropdownOption_Test4_DropdownDropdown);
				BehaviourDropdown_OnPool.Invoke(DropdownOption_Test4BehaviourDropdown, new object[0]);
				BehaviourDropdown_OnEnable.Invoke(DropdownOption_Test4BehaviourDropdown, new object[0]);

				DropdownOption_Test4BehaviourDropdown.interactable = true;
				DropdownOption_Test4BehaviourDropdown.ClearOptions();
				DropdownOption_Test4BehaviourDropdown.AddOptions(new List<string>() { "1", "2", "3" });
				DropdownOption_Test4_DropdownDropdown.Show();

				DropdownOption_Test4_DropdownDropdown.transition = Selectable.Transition.SpriteSwap;
				DropdownOption_Test4_DropdownDropdown.spriteState = new SpriteState()
				{
					highlightedSprite = UIElements.Dropdown_Highlight_BG
				};
				var DropdownOption_Test4_DropdownRect = DropdownOption_Test4_Dropdown.GetComponent<RectTransform>();
				DropdownOption_Test4_DropdownRect.anchoredPosition3D = new Vector3(174f, 0, 0);
				DropdownOption_Test4_DropdownRect.sizeDelta = new Vector2(202f, 30f);

				var DropdownOption_Test4_Dropdown_Label = DropdownOption_Test4_Dropdown.transform.Find("Label").gameObject;
				var DropdownOption_Test4_Dropdown_LabelText = DropdownOption_Test4_Dropdown_Label.GetComponent<Text>();
				DropdownOption_Test4_Dropdown_LabelText.font = UIElements.ExoSemiBold;
				DropdownOption_Test4_Dropdown_LabelText.color = Color.white;
				var DropdownOption_Test4_Dropdown_LabelShadow = DropdownOption_Test4_Dropdown_Label.AddComponent<Shadow>();
				DropdownOption_Test4_Dropdown_LabelShadow.effectColor = new Color(0, 0, 0, 0.5f);
				var DropdownOption_Test4_Dropdown_Arrow = DropdownOption_Test4_Dropdown.transform.Find("Arrow").gameObject;
				var DropdownOption_Test4_Dropdown_ArrowRect = DropdownOption_Test4_Dropdown_Arrow.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_ArrowRect.sizeDelta = new Vector2(13.1f, 13.1f);
				var DropdownOption_Test4_Dropdown_ArrowShadow = DropdownOption_Test4_Dropdown_Arrow.AddComponent<Shadow>();
				DropdownOption_Test4_Dropdown_ArrowShadow.effectColor = new Color(0, 0, 0, 0.5f);
				var DropdownOption_Test4_Dropdown_Template = DropdownOption_Test4_Dropdown.transform.Find("Template").gameObject;
				var DropdownOption_Test4_Dropdown_TemplateRect = DropdownOption_Test4_Dropdown_Template.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_TemplateRect.anchoredPosition3D = Vector3.zero;
				var DropdownOption_Test4_Dropdown_TemplateImage = DropdownOption_Test4_Dropdown_Template.GetComponent<Image>();
				DropdownOption_Test4_Dropdown_TemplateImage.sprite = UIElements.Dropdown_Active_BG;
				var DropdownOption_Test4_Dropdown_TemplateController = DropdownOption_Test4_Dropdown_Template.AddComponent<UIAutoScrollItemController>();
				var DropdownOption_Test4_Dropdown_Template_Viewport = DropdownOption_Test4_Dropdown_Template.transform.Find("Viewport").gameObject;
				var DropdownOption_Test4_Dropdown_Template_ViewportRect = DropdownOption_Test4_Dropdown_Template_Viewport.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_ViewportRect.sizeDelta = new Vector2(-6.8f, 0);
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item = DropdownOption_Test4_Dropdown_Template_Viewport.transform.Find("Content").Find("Item").gameObject;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_ItemToggle = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item.GetComponent<Toggle>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_ItemToggle.image.sprite = UIElements.Option_Content_BG;
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_ItemToggle.transition = Selectable.Transition.SpriteSwap;
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_ItemToggle.spriteState = new SpriteState()
				{
					highlightedSprite = UIElements.Option_Content_Highlight_BG
				};
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_ItemController = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item.AddComponent<UIAutoScrollItem>();
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemBackground = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item.transform.Find("Item Background").gameObject;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemBackgroundImage = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemBackground.GetComponent<Image>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemBackgroundImage.sprite = UIElements.Option_Content_BG;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemCheckmark = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item.transform.Find("Item Checkmark").gameObject;
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemCheckmark.SetActive(false);
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemCheckmarkRect = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemCheckmark.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemCheckmarkRect.anchoredPosition3D = new Vector3(99.1f, 0, 0);
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabel = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item.transform.Find("Item Label").gameObject;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelRect = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabel.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelRect.anchoredPosition3D = Vector3.zero;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelText = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabel.GetComponent<Text>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelText.font = UIElements.ExoSemiBold;
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelText.color = Color.white;
				var DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelShadow = DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabel.AddComponent<Shadow>();
				DropdownOption_Test4_Dropdown_Template_Viewport_Content_Item_ItemLabelShadow.effectColor = new Color(0, 0, 0, 0.5f);
				var DropdownOption_Test4_Dropdown_Template_Scrollbar = DropdownOption_Test4_Dropdown_Template.transform.Find("Scrollbar");
				var DropdownOption_Test4_Dropdown_Template_ScrollbarRect = DropdownOption_Test4_Dropdown_Template_Scrollbar.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_ScrollbarRect.anchoredPosition3D = new Vector3(7.9f, -4.9f, 0);
				DropdownOption_Test4_Dropdown_Template_ScrollbarRect.sizeDelta = new Vector2(14.9f, -9.8f);
				var DropdownOption_Test4_Dropdown_Template_ScrollbarScrollbar = DropdownOption_Test4_Dropdown_Template_Scrollbar.GetComponent<Scrollbar>();
				DropdownOption_Test4_Dropdown_Template_ScrollbarScrollbar.image.sprite = UIElements.ScrollBar_Small_01;
				DropdownOption_Test4_Dropdown_Template_ScrollbarScrollbar.image.color = new Color(1f, 1f, 1f, 0.388f);
				var DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea = DropdownOption_Test4_Dropdown_Template_Scrollbar.transform.Find("Sliding Area").gameObject;
				var DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingAreaRect = DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingAreaRect.sizeDelta = Vector2.zero;
				var DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_Handle = DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea.transform.Find("Handle").gameObject;
				var DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_HandleRect = DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_Handle.GetComponent<RectTransform>();
				DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_HandleRect.anchorMax = Vector2.one;
				DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_HandleRect.sizeDelta = Vector2.zero;
				var DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_HandleImage = DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_Handle.GetComponent<Image>();
				DropdownOption_Test4_Dropdown_Template_Scrollbar_SlidingArea_HandleImage.sprite = UIElements.ScrollBar_Small_01;

				DropdownOption_Test4_Dropdown.transform.SetParent(DropdownOption_Test4.transform, false);
				#endregion Dropdown


				midGroup.CalculateLayoutInputHorizontal();
				midGroup.CalculateLayoutInputVertical();
				midGroup.SetLayoutHorizontal();
				midGroup.SetLayoutVertical();

				content2Group.CalculateLayoutInputHorizontal();
				content2Group.CalculateLayoutInputVertical();
				content2Group.SetLayoutHorizontal();
				content2Group.SetLayoutVertical();

				content1Group.CalculateLayoutInputHorizontal();
				content1Group.CalculateLayoutInputVertical();
				content1Group.SetLayoutHorizontal();
				content1Group.SetLayoutVertical();

				// Push the new tab to the arrays
				m_OptionsTabs.SetValue(__instance, optionsTabs);
				m_OptionsElements.SetValue(__instance, optionsElements);
			}
		}
	}
}
/*private void Awake()
	{
		this.m_OptionsTypeCount = Enum.GetNames(typeof(UIScreenOptions.OptionsType)).Length + 1;
		Array.Resize<Toggle>(ref this.m_OptionsTabs, this.m_OptionsTypeCount);
		Array.Resize<UIOptions>(ref this.m_OptionsElements, this.m_OptionsTypeCount);
		this.m_OptionsTabs[this.m_OptionsTypeCount - 1] = UnityEngine.Object.Instantiate<Toggle>(this.m_OptionsTabs[this.m_OptionsTypeCount - 2]);
		this.m_OptionsTabs[this.m_OptionsTypeCount - 1].transform.SetParent(this.m_OptionsTabs[this.m_OptionsTypeCount - 2].transform.parent, false);
		this.m_OptionsTabs[this.m_OptionsTypeCount - 1].enabled = true;
		this.m_OptionsTabs[this.m_OptionsTypeCount - 1].gameObject.SetActive(true);
		UnityEngine.Object.DestroyImmediate(this.m_OptionsTabs[this.m_OptionsTypeCount - 1].gameObject.GetComponentInChildren<UILocalisedText>());
		this.m_OptionsTabs[this.m_OptionsTypeCount - 1].GetComponentInChildren<Text>().text = "MODS";
		for (int i = 0; i < this.m_OptionsTypeCount; i++)
		{
			int index = i;
			this.m_OptionsTabs[i].onValueChanged.AddListener(delegate(bool set)
			{
				if (set)
				{
					this.ShowOptions((UIScreenOptions.OptionsType)index);
				}
			});
			RectTransform component = this.m_OptionsTabs[i].GetComponent<RectTransform>();
			if (component)
			{
				component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width * 4f / 5f);
				component.anchorMin *= 0.8f;
				Vector2 anchorMax = component.anchorMax;
				anchorMax.x *= 0.8f;
				component.anchorMax = anchorMax;
			}
		}
		RectTransform component2 = this.m_OptionsTabs[this.m_OptionsTypeCount - 1].GetComponent<RectTransform>();
		component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component2.rect.width * 4f / 5f);
		component2.anchorMin = new Vector2(0.8f, 0f);
		component2.anchorMax = new Vector2(1f, 0.45f);
		component2.anchoredPosition = Vector2.zero;
		component2.sizeDelta = this.m_OptionsTabs[0].GetComponent<RectTransform>().sizeDelta;
		component2.localScale = this.m_OptionsTabs[0].GetComponent<RectTransform>().localScale;
		component2.offsetMin = this.m_OptionsTabs[0].GetComponent<RectTransform>().offsetMin;
		component2.offsetMax = this.m_OptionsTabs[0].GetComponent<RectTransform>().offsetMax;
		this.OnChangeEvent.Subscribe(new Action(this.OnSettingsChanged));
		Transform parent = this.m_OptionsElements[0].gameObject.transform.parent;
		GameObject gameObject = DefaultControls.CreatePanel(new DefaultControls.Resources
		{
			background = Sprite.Create(Texture2D.whiteTexture, Rect.zero, Vector2.zero)
		});
		gameObject.transform.SetParent(parent, false);
		this.m_OptionsElements[this.m_OptionsTypeCount - 1] = gameObject.AddComponent<UIOptionsMod>();
		DefaultControls.Resources resources = default(DefaultControls.Resources);
		GameObject gameObject2 = DefaultControls.CreatePanel(resources);
		gameObject2.transform.SetParent(gameObject.transform, false);
		RectTransform component4 = gameObject2.GetComponent<RectTransform>();
		RectTransform component3 = this.m_OptionsElements[0].gameObject.GetComponent<RectTransform>();
		component4.anchorMin = component3.anchorMin;
		component4.anchorMax = component3.anchorMax;
		component4.anchoredPosition3D = component3.anchoredPosition3D;
		component4.sizeDelta = component3.sizeDelta;
		component4.pivot = component3.pivot;
		GameObject gameObject3 = DefaultControls.CreateButton(resources);
		gameObject3.transform.SetParent(gameObject2.transform, false);
		RectTransform component5 = gameObject3.GetComponent<RectTransform>();
		component5.pivot = new Vector2(0.5f, 0.5f);
		component5.anchorMin = new Vector2(0f, 1f);
		component5.anchorMax = new Vector2(0.5f, 1f);
		component5.anchoredPosition = new Vector2(0f, -25f);
		component5.sizeDelta = new Vector2(0f, 50f);
	}*/
