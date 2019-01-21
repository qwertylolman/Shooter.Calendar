using System;
using Android.Views;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Droid.Listeners
{
	public class ViewOnClickListener : Java.Lang.Object, View.IOnClickListener
	{
		private readonly Action<View> onClick;

		public ViewOnClickListener([NotNull] Action<View> onClick)
		{
			this.onClick = onClick;
		}

		public void OnClick(View v)
		{
			onClick(v);
		}
	}
}