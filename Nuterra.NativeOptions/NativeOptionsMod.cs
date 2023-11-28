using System;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using HarmonyLib;

namespace Nuterra.NativeOptions
{
	public class NativeOptionsMod : ModBase
	{
		public static UnityEvent onOptionsSaved = new UnityEvent();
		private const string HarmonyID = "Nuterra.NativeOptions";
		internal static Harmony harmony = new Harmony(HarmonyID);
		internal static bool patched = false;

		public override void DeInit()
		{
			harmony.UnpatchAll(HarmonyID);
			patched = false;
		}

		public override void Init()
		{
			if (!patched)
			{
				harmony.PatchAll(Assembly.GetExecutingAssembly());
				patched = true;
			}
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
			BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
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
				var uiOptionsMods = Mods.AddComponent<UIOptionsMods>();
                optionsElements[optionsCount - 1] = uiOptionsMods;
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
                uiOptionsMods.tab_toggle = modsToggle;

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
				GameObject.DestroyImmediate(prev_page.GetComponent<UIButtonGoToGauntletAttract>());
				GameObject.DestroyImmediate(prev_page.GetComponent<UIButtonGoToScreen>());
				GameObject.DestroyImmediate(prev_page.GetComponent<UIButtonGoToScreenOrCheckPremium>());
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
				GameObject.DestroyImmediate(next_page.GetComponent<UIButtonGoToGauntletAttract>());
				GameObject.DestroyImmediate(next_page.GetComponent<UIButtonGoToScreen>());
				GameObject.DestroyImmediate(next_page.GetComponent<UIButtonGoToScreenOrCheckPremium>());
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

				UIOptionsMods.PageInfo = page_info.transform;
				UIOptionsMods.PrevPage = prev_page.transform;
				UIOptionsMods.NextPage = next_page.transform;

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
                title1.text = "";//"LEFT SIDE";
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
                title2.text = "";//"RIGHT SIDE";
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
				
				UIOptionsMods.MidPanel = mid_panel.transform;
				UIOptionsMods.Content1 = content1.transform;
				UIOptionsMods.Content2 = content2.transform;

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