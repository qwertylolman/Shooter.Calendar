using System.Collections.Generic;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void AddRange<TItem>([NotNull] this IList<TItem> list, [NotNull] IEnumerable<TItem> items)
        {
            foreach(var item in items)
            {
                list.Add(item);
            }
        }
    }
}
