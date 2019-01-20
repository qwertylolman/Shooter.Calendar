using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Views;

namespace Shooter.Calendar.Droid.Binder
{
    public static class ViewBinder
	{
		public static Unbinder Bind(View view, object taget)
		    => Bind(taget, (viewId) => view.FindViewById(viewId));

		public static Unbinder Bind(Activity activity)
		    => Bind(activity, (viewId) => activity.FindViewById(viewId));

		public static Unbinder Bind(object taget, Func<int, View> findById)
		{
			var unbinder = new Unbinder(taget);
			var activitType = taget.GetType();

			var fields = activitType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var properties = activitType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			var members = fields.Concat<MemberInfo>(properties);

			foreach (var member in members)
			{
				var attrs = Attribute.GetCustomAttributes(member, typeof(FindByIdAttribute));
				if (attrs.Length > 0)
				{
					member.SetValue(taget, findById(((FindByIdAttribute)attrs[0]).ViewId));
					unbinder.AddBinding(member);
				}

				var findManyAttrs = Attribute.GetCustomAttributes(member, typeof(FindManyAttribute));
				if (findManyAttrs.Length > 0)
				{
					var findManyAttribute = findManyAttrs[0] as FindManyAttribute;
					var ids = findManyAttribute.ViewIds;
					var views = new List<View>();
					foreach (var id in ids)
					{
						views.Add(findById(id));
					}

					member.SetValue(taget, views);
					unbinder.AddBinding(member);
				}
			}

			return unbinder;
		}
	}
}