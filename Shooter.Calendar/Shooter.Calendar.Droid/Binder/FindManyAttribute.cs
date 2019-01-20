using System;

namespace Shooter.Calendar.Droid.Binder
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class FindManyAttribute : Attribute
	{
		public FindManyAttribute(params int[] ids)
		{
			ViewIds = ids;
		}

		public int[] ViewIds { get; }
	}
}