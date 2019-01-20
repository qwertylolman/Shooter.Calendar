using Realms;
using System;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Common.RealmExtensions.Extensions
{
    public static class RealmProvider
    {
        public static Realm GetInstance()
            => Realm.GetInstance();

        public static void Write([NotNull] Action<Realm> writeAction)
        {
            var realm = GetInstance();
            realm.Write(() => writeAction(realm));
        }
    }
}
