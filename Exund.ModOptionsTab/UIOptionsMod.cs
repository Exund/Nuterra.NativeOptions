using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Exund.ModOptionsTab
{
	class UIOptionsMod : UIOptions
	{
		internal Toggle tab_toggle;
		public override UIScreenOptions.SaveFailureType CanSave()
		{
			return UIScreenOptions.SaveFailureType.None;
		}

		public override void ClearSettings()
		{

		}

		public override void OnCloseScreen()
		{

		}

		public override void ResetSettings()
		{

		}

		public override void SaveSettings()
		{

		}

		public override void Setup(EventNoParams OnChangeEvent)
		{

		}

		/*Rect r = new Rect(0, 0, 300, 500);
		public void OnGUI()
		{
			if(tab_toggle) r = GUI.Window(7999, r, DoWindow, "Debug");
		}

		private void DoWindow(int id)
		{
			
			var rect = tab_toggle.GetComponent<RectTransform>();

			float.TryParse(GUILayout.TextField(rect.anchorMin.x.ToString()), out float minx);
			float.TryParse(GUILayout.TextField(rect.anchorMin.y.ToString()), out float miny);
			rect.anchorMin = new Vector2(minx, miny);

			float.TryParse(GUILayout.TextField(rect.anchorMax.x.ToString()), out float maxx);
			float.TryParse(GUILayout.TextField(rect.anchorMax.y.ToString()), out float maxy);
			rect.anchorMax = new Vector2(maxx, maxy);

			GUI.DragWindow();
		}*/
	}
}
