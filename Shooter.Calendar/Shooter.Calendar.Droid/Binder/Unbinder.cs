using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shooter.Calendar.Droid.Binder
{
	public class Unbinder
	{
		private readonly List<MemberInfo> memberBindings;
		private readonly WeakReference<object> target;

		public Unbinder(object targetObject)
		{
			memberBindings = new List<MemberInfo>();
			target = new WeakReference<object>(targetObject);
		}

		public void AddBinding(MemberInfo field)
		{
			memberBindings.Add(field);
		}

		public void Unbind()
		{
			foreach (var memberInfo in memberBindings)
			{
				object targetObject;
				if (target.TryGetTarget(out targetObject))
				{
					memberInfo.SetValue(targetObject, null);
				}
			}
		}
	}
}