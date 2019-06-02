using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exund.ModOptionsTab
{
	class UIOptionsMod : UIOptions
	{
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
	}
}
