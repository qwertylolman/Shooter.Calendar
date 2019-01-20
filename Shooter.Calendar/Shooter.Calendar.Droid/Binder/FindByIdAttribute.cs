using System;

namespace Shooter.Calendar.Droid.Binder
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class FindByIdAttribute : Attribute
	{
		public FindByIdAttribute(int id)
		{
			ViewId = id;
		}

		public int ViewId { get; }
	}
}