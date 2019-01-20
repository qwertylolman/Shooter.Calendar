using System;
using System.Linq;
using Realms;

namespace Shooter.Calendar.Core.Common.Extensions.RealmExtensions
{
    public static class BacklinksExtensions
    {
        public static TBacklinkItem GetBacklink<TBacklinkItem, TItem>(this TItem realmObject, params Func<TItem, IQueryable<TBacklinkItem>>[] backlinkGetters)
            where TItem : RealmObject
            where TBacklinkItem : RealmObject
        {
            if (realmObject == null
                || backlinkGetters.Any() == false)
            {
                return null;
            }

            foreach (var getter in backlinkGetters)
            {
                var backlink = getter(realmObject).FirstOrDefault();
                if (backlink == null)
                {
                    continue;
                }

                return backlink;
            }

            return null;
        }
    }
}
