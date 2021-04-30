using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace Nuterra.NativeOptions
{
	public class NativeOptionsMod
	{
		public static void Load()
		{
			var harmony = HarmonyInstance.Create("Nuterra.NativeOptions");
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
					if (item is LayoutGroup lg)
					{
						tree.AppendFormat("\n{0}\t\t\tchildAlignment {1}", tab, lg.childAlignment.ToString());
						tree.AppendFormat("\n{0}\t\t\tlayoutPriority {1}", tab, lg.layoutPriority);
						tree.AppendFormat("\n{0}\t\t\tpadding {1}", tab, lg.padding);
						tree.AppendFormat("\n{0}\t\t\tminWidth {1}", tab, lg.minWidth);
						tree.AppendFormat("\n{0}\t\t\tminHeight {1}", tab, lg.minHeight);
						tree.AppendFormat("\n{0}\t\t\tpreferredWidth {1}", tab, lg.preferredWidth);
						tree.AppendFormat("\n{0}\t\t\tpreferredHeight {1}", tab, lg.preferredHeight);
						tree.AppendFormat("\n{0}\t\t\tflexibleWidth {1}", tab, lg.flexibleWidth);
						tree.AppendFormat("\n{0}\t\t\tflexibleHeight {1}", tab, lg.flexibleHeight);
					}
					if (item is HorizontalOrVerticalLayoutGroup hvlg)
					{	
						tree.AppendFormat("\n{0}\t\t\tchildControlWidth {1}", tab, hvlg.childControlWidth);
						tree.AppendFormat("\n{0}\t\t\tchildControlHeight {1}", tab, hvlg.childControlHeight);
						tree.AppendFormat("\n{0}\t\t\tchildForceExpandWidth {1}", tab, hvlg.childForceExpandWidth);
						tree.AppendFormat("\n{0}\t\t\tchildForceExpandHeight {1}", tab, hvlg.childForceExpandHeight);						
						tree.AppendFormat("\n{0}\t\t\tspacing {1}", tab, hvlg.spacing);		
					}
					if(item is GridLayoutGroup glg)
					{
						tree.AppendFormat("\n{0}\t\t\tcellSize {1}", tab, glg.cellSize.ToString());
						tree.AppendFormat("\n{0}\t\t\tconstraint {1}", tab, glg.constraint.ToString());
						tree.AppendFormat("\n{0}\t\t\tconstraintCount {1}", tab, glg.constraintCount);
						tree.AppendFormat("\n{0}\t\t\tspacing {1}", tab, glg.spacing.ToString());
						tree.AppendFormat("\n{0}\t\t\tstartAxis {1}", tab, glg.startAxis.ToString());
						tree.AppendFormat("\n{0}\t\t\tstartCorner {1}", tab, glg.startCorner.ToString());
					}
					if (item is LayoutElement le)
					{
						tree.AppendFormat("\n{0}\t\t\tminWidth {1}", tab, le.minWidth);
						tree.AppendFormat("\n{0}\t\t\tminHeight {1}", tab, le.minHeight);
						tree.AppendFormat("\n{0}\t\t\tpreferredWidth {1}", tab, le.preferredWidth);
						tree.AppendFormat("\n{0}\t\t\tpreferredHeight {1}", tab, le.preferredHeight);
						tree.AppendFormat("\n{0}\t\t\tflexibleWidth {1}", tab, le.flexibleWidth);
						tree.AppendFormat("\n{0}\t\t\tflexibleHeight {1}", tab, le.flexibleHeight);
					}
					if (item is ScrollRect sr)
					{
						tree.AppendFormat("\n{0}\t\t\tdecelerationRate {1}", tab, sr.decelerationRate);
						tree.AppendFormat("\n{0}\t\t\telasticity {1}", tab, sr.elasticity);
						tree.AppendFormat("\n{0}\t\t\tinertia {1}", tab, sr.inertia);
						tree.AppendFormat("\n{0}\t\t\tscrollSensitivity {1}", tab, sr.scrollSensitivity);
						tree.AppendFormat("\n{0}\t\t\tvelocity ({1}, {2})", tab, sr.velocity.x, sr.velocity.y);
						tree.AppendFormat("\n{0}\t\t\tmovementType {1}", tab, sr.movementType.ToString());
						tree.AppendFormat("\n{0}\t\t\thorizontal {1}", tab, sr.horizontal);
						tree.AppendFormat("\n{0}\t\t\thorizontalScrollbarSpacing {1}", tab, sr.horizontalScrollbarSpacing);
						tree.AppendFormat("\n{0}\t\t\thorizontalScrollbarVisibility {1}", tab, sr.horizontalScrollbarVisibility.ToString());
						tree.AppendFormat("\n{0}\t\t\tvertical {1}", tab, sr.vertical);
						tree.AppendFormat("\n{0}\t\t\tverticalScrollbarSpacing {1}", tab, sr.verticalScrollbarSpacing);
						tree.AppendFormat("\n{0}\t\t\tverticalScrollbarVisibility {1}", tab, sr.verticalScrollbarVisibility.ToString());
					}
					if(item is ContentSizeFitter csf)
					{
						tree.AppendFormat("\n{0}\t\t\thorizontalFit {1}", tab, csf.horizontalFit.ToString());
						tree.AppendFormat("\n{0}\t\t\tverticalFit {1}", tab, csf.verticalFit.ToString());
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
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "effectDistance", s.effectDistance.ToString());
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
						tree.AppendFormat("\n{0}\t\t\t{1} {2}", tab, "navigation", se.navigation.ToString());
						tree.AppendFormat("\n{0}\t\t\t\t{1} {2}", tab, "mode", se.navigation.mode.ToString());
						if(se.navigation.mode == Navigation.Mode.Explicit)
						{
							if(se.navigation.selectOnUp) tree.AppendFormat("\n{0}\t\t\t\t\t{1} {2}", tab, "selectOnUp", se.navigation.selectOnUp.name);
							if(se.navigation.selectOnDown) tree.AppendFormat("\n{0}\t\t\t\t\t{1} {2}", tab, "selectOnDown", se.navigation.selectOnDown.name);
							if(se.navigation.selectOnLeft) tree.AppendFormat("\n{0}\t\t\t\t\t{1} {2}", tab, "selectOnLeft", se.navigation.selectOnLeft.name);
							if(se.navigation.selectOnRight) tree.AppendFormat("\n{0}\t\t\t\t\t{1} {2}", tab, "selectOnRight", se.navigation.selectOnRight.name);
						}

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
	}

	static class Patches
	{
		private static readonly FieldInfo m_OptionsTypeCount;
		private static readonly FieldInfo m_OptionsTabs;
		private static readonly FieldInfo m_OptionsElements;
		private static readonly FieldInfo BehaviourSlider_m_FSM;
        private static readonly MethodInfo BehaviourSlider_OnPool;
        private static readonly FieldInfo BehaviourDropdown_m_Target;

		static Patches()
		{
			Type UIScreenOptionsType = typeof(UIScreenOptions);
			BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			m_OptionsTypeCount = UIScreenOptionsType.GetField("m_OptionsTypeCount", privateFlags);
			m_OptionsTabs = UIScreenOptionsType.GetField("m_OptionsTabs", privateFlags);
			m_OptionsElements = UIScreenOptionsType.GetField("m_OptionsElements", privateFlags);	
			Type BehaviourSliderType = typeof(UIOptionsBehaviourSlider);
			BehaviourSlider_m_FSM = BehaviourSliderType.GetField("m_FSM", privateFlags);
			BehaviourSlider_OnPool = BehaviourSliderType.GetMethod("OnPool", privateFlags);
			Type BehaviourDropdownType = typeof(UIOptionsBehaviourDropdown);
			BehaviourDropdown_m_Target = BehaviourDropdownType.GetField("m_Target", privateFlags);
		}

		[HarmonyPatch(typeof(UIOptionsBehaviourSlider), "OnEnable")]
		internal static class UIOptionsBehaviourSliderOnEnable
		{
			private static void Prefix(ref UIOptionsBehaviourSlider __instance)
			{
				if (BehaviourSlider_m_FSM.GetValue(__instance) == null) BehaviourSlider_OnPool.Invoke(__instance, new object[0]);
			}
		}

		[HarmonyPatch(typeof(UIOptionsBehaviourDropdown), "OnEnable")]
		internal static class UIOptionsBehaviourDropdownOnEnable
		{
			private static void Postfix(ref UIOptionsBehaviourDropdown __instance)
			{
				var m_Target = (Dropdown)BehaviourDropdown_m_Target.GetValue(__instance);
				if (m_Target && m_Target.template)
				{
					var c = m_Target.template.gameObject.GetComponent<Canvas>() ?? m_Target.template.gameObject.AddComponent<Canvas>();
					c.sortingLayerName = "UI";
					c.overrideSorting = false;
				}
			}
		}

		[HarmonyPatch(typeof(UIScreenOptions), "Awake")]
		internal static class UIScreenOptionsAwake
		{
			private static void Postfix(ref UIScreenOptions __instance)
			{
				// Add a tab to the settings
				int optionsCount = (int)m_OptionsTypeCount.GetValue(__instance) + 1;
				m_OptionsTypeCount.SetValue(__instance, optionsCount);

				// Get settings tabs to an array
				Toggle[] optionsTabs = new Toggle[optionsCount];
				((Toggle[])m_OptionsTabs.GetValue(__instance)).CopyTo(optionsTabs, 0);

                // Copy last tab to new tab
				Toggle modsToggle = GameObject.Instantiate<Toggle>(optionsTabs[optionsCount - 2]);
				modsToggle.transform.SetParent(optionsTabs[optionsCount - 2].transform.parent, false);
				modsToggle.gameObject.SetActive(true);

				// Clear and assign name
				GameObject.DestroyImmediate(modsToggle.gameObject.GetComponentInChildren<UILocalisedText>());
				modsToggle.GetComponentInChildren<Text>().text = "MODS";

				optionsTabs[optionsCount - 1] = modsToggle;

				// Move tabs
				var x = 0f;
				foreach (var toggle in optionsTabs)
				{
					if (!toggle) continue;
					RectTransform toggleRect = toggle.GetComponent<RectTransform>();
					if (toggleRect)
					{
						Vector2 anchorMin = toggleRect.anchorMin;
						anchorMin.x = x;
						toggleRect.anchorMin = anchorMin;
						x += 0.2f;
						Vector2 anchorMax = toggleRect.anchorMax;
						anchorMax.x = x;
						toggleRect.anchorMax = anchorMax;
					}
				}


				// Get the tab panels to an array
				UIOptions[] optionsElements = new UIOptions[optionsCount];
				((UIOptions[])m_OptionsElements.GetValue(__instance)).CopyTo(optionsElements, 0);


				//Console.WriteLine(UIUtilities.GetComponentTree(optionsElements[1].gameObject));

				// Give new tab a UIOptions panel and position to reference
				DefaultControls.Resources resources = default(DefaultControls.Resources);
				GameObject Mods = DefaultControls.CreatePanel(resources);
				Mods.name = "Mods";
				GameObject.DestroyImmediate(Mods.GetComponent<Image>());		
				RectTransform panel_rect = Mods.GetComponent<RectTransform>();
				RectTransform reference_rect = optionsElements[0].gameObject.GetComponent<RectTransform>();
				panel_rect.anchoredPosition3D = reference_rect.anchoredPosition3D;
				panel_rect.anchorMin = reference_rect.anchorMin;
				panel_rect.anchorMax = reference_rect.anchorMax;
				panel_rect.pivot = reference_rect.pivot;
				panel_rect.sizeDelta = reference_rect.sizeDelta;
				optionsElements[optionsCount - 1] = Mods.AddComponent<UIOptionsMods>();
				Mods.transform.SetParent(optionsElements[0].gameObject.transform.parent, false);

				// Allow tab to be activatable
				var instance = __instance;
				modsToggle.onValueChanged.AddListener(delegate (bool set)
				{
					if (set)
					{
						instance.ShowOptions((UIScreenOptions.OptionsType)(optionsCount - 1));
					}
				});
				((UIOptionsMods)optionsElements[optionsCount - 1]).tab_toggle = modsToggle;

				// Create UI elements
				GameObject bottom_panel = DefaultControls.CreatePanel(resources);
				bottom_panel.name = "Bottom Panel";
				GameObject.DestroyImmediate(bottom_panel.GetComponent<Image>());
				var bottomRect = bottom_panel.GetComponent<RectTransform>();
				bottomRect.anchoredPosition3D = new Vector3(0, -289.6f, 0);
				bottomRect.anchorMin = Vector2.zero;
				bottomRect.anchorMax = Vector2.one;
				bottomRect.pivot = Vector2.one * 0.5f;
				bottomRect.sizeDelta = new Vector2(-60f, -579.2f);
				var bottomGroup = bottom_panel.AddComponent<GridLayoutGroup>();
				bottomGroup.childAlignment = TextAnchor.MiddleLeft;
				bottomGroup.padding = new RectOffset(0, 0, 0, 0);
				bottomGroup.cellSize = new Vector2(536f, 40f);
				bottomGroup.constraint = GridLayoutGroup.Constraint.Flexible;
				//bottomGroup.constraintCount = 2;
				bottomGroup.spacing = new Vector2(60f, 15f);
				bottomGroup.startAxis =	GridLayoutGroup.Axis.Vertical;
				bottomGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;

				bottom_panel.transform.SetParent(Mods.transform, false);

				GameObject page_info = DefaultControls.CreateText(resources);
				var page_infoRect = page_info.GetComponent<RectTransform>();
				page_infoRect.anchoredPosition3D = new Vector3(268f, -25.3f, 0);
				page_infoRect.anchorMin = Vector2.up;
				page_infoRect.anchorMax = Vector2.up;
				page_infoRect.offsetMin = new Vector2(0, -45.3f);
				page_infoRect.offsetMax = new Vector2(536f, -5.3f);
				page_infoRect.pivot = Vector2.one * 0.5f;
				//page_infoRect.sizeDelta(536.0, 40.0)
				Text text = page_info.GetComponent<Text>();
				text.text = "";
				text.font = UIElements.ExoSemiBold;
				text.fontSize = 25;
				text.color = Color.white;
				page_info.transform.SetParent(bottom_panel.transform, false);


				GameObject buttons_panel = DefaultControls.CreatePanel(resources);
				buttons_panel.name = "Page Panel";
				GameObject.DestroyImmediate(buttons_panel.GetComponent<Image>());
				var button_group = buttons_panel.AddComponent<HorizontalLayoutGroup>();
				button_group.childAlignment = TextAnchor.MiddleRight;
				button_group.childControlWidth = false;
				button_group.childControlHeight = true;
				button_group.childForceExpandWidth = false;
				button_group.childForceExpandHeight = true;
				button_group.spacing = 0;
				button_group.padding = new RectOffset(0, 0, 0, 0);
				buttons_panel.transform.SetParent(bottom_panel.transform, false);

				GameObject prev_page = GameObject.Instantiate(UIElements.Button_Back);
				prev_page.SetActive(true);
				GameObject.DestroyImmediate(prev_page.GetComponent<UIButtonGoBack>());
				var prevRect = prev_page.GetComponent<RectTransform>();
				prevRect.anchoredPosition3D = Vector3.zero;
				prev_page.GetComponent<Button>().onClick.RemoveAllListeners();
				var prevState = prev_page.GetComponent<Button>().spriteState;
				prevState.disabledSprite = UIElements.Button_Disabled_BG;
				prev_page.GetComponent<Button>().spriteState = prevState;
				prev_page.transform.SetParent(buttons_panel.transform, false);

				GameObject next_page = GameObject.Instantiate(UIElements.Button_Back);
				next_page.SetActive(true);
				GameObject.DestroyImmediate(next_page.GetComponent<UIButtonGoBack>());
				var nextRect = next_page.GetComponent<RectTransform>();
				nextRect.anchoredPosition3D = Vector3.zero;
				next_page.GetComponent<Button>().onClick.RemoveAllListeners();
				var nextState = next_page.GetComponent<Button>().spriteState;
				nextState.disabledSprite = UIElements.Button_Disabled_BG;
				next_page.GetComponent<Button>().spriteState = nextState;
				var next_iconRect = next_page.transform.Find("Icon").GetComponent<RectTransform>();
				next_iconRect.localEulerAngles = new Vector3(0, 180f, 0);
				next_page.transform.SetParent(buttons_panel.transform, false);

				UIOptionsMods.PageInfo = page_info;
				UIOptionsMods.PrevPage = prev_page;
				UIOptionsMods.NextPage = next_page;


				GameObject top_panel = DefaultControls.CreatePanel(resources);
				top_panel.name = "Top Panel";
				GameObject.DestroyImmediate(top_panel.GetComponent<Image>());
				var topRect = top_panel.GetComponent<RectTransform>();
				topRect.anchoredPosition3D = new Vector3(0, 285f, 0);
				topRect.anchorMin = Vector2.zero;
				topRect.anchorMax = Vector2.one;
				topRect.pivot = Vector2.one * 0.5f;
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
				title1Rect.pivot = title1Rect.anchorMin = title1Rect.anchorMax = Vector2.one * 0.5f;
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
				title2Rect.pivot = title2Rect.anchorMin = title2Rect.anchorMax = Vector2.one * 0.5f;
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
				midRect.anchoredPosition3D = new Vector3(0, -4.1f, 0);
				midRect.anchorMin = Vector2.zero;
				midRect.anchorMax = Vector2.one;
				midRect.offsetMin = new Vector2(3.2f, 50.7f);
				midRect.offsetMax = new Vector2(-3.2f, -59.0f);
				midRect.pivot = Vector2.one * 0.5f;
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
				
				UIOptionsMods.MidPanel = mid_panel;
				UIOptionsMods.Content1 = content1;
				UIOptionsMods.Content2 = content2;

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