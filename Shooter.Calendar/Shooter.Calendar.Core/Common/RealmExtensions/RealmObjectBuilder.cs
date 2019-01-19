using System;
using Shooter.Calendar.Core.Managers.KeyGenerator;
using MvvmCross;
using Realms;

namespace Shooter.Calendar.Core.Common.RealmExtensions
{
    public static class RealmObjectBuilder
    {
        private static IKeyGenerator keyGenerator;

        private static IKeyGenerator KeyGenerator
            => keyGenerator ?? (keyGenerator = Mvx.IoCProvider.Resolve<IKeyGenerator>());

        public static TItem Build<TItem>(bool autoSetPrimaryKey = true)
            where TItem : RealmObject, new()
        {
            var key = autoSetPrimaryKey == true
                ? KeyGenerator.GenerateKey()
                : string.Empty;

            return BuildWithKey<TItem>(key);
        }

        public static TItem BuildWithKey<TItem>(string primaryKey = null)
            where TItem : RealmObject, new()
        {
            var item = new TItem();

            if (string.IsNullOrEmpty(primaryKey) == false)
            {
                item.ThrowIfNoPrimaryKey();
                item.SetPrimaryKey(primaryKey);
            }

            return item;
        }
    }
}
