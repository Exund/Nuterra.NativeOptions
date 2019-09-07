using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Exund.ModOptionsTab
{
	public abstract class Option
	{
		internal string name;
		internal string modName;
		internal object defaultValue;
		internal object savedValue;
		internal object value;

		public abstract UnityEventBase onValueChanged { get; }
		public abstract GameObject UIElement { get; protected set; }

		public abstract void ResetValue();

		public UnityEvent onValueSaved = new UnityEvent();
	}

	public abstract class Option<T> : Option
	{	
		public virtual T Value { get => (T)value; set => this.value = value; }

		public T SavedValue { get => (T)savedValue; }

		public Option(string Name, string ModName, T DefaultValue)
		{
			this.name = Name;
			this.modName = ModName;
			this.value = this.savedValue = this.defaultValue = DefaultValue;

			UIOptionsMods.AddOption(this);
		}

		public override void ResetValue()
		{
			Value = (T)defaultValue;
		}
	}
}
