using Realms;

namespace Shooter.Calendar.Core.Common.RealmExtensions.Extensions
{
    public static class RealmProvider
    {
        public static Realm GetInstance()
            => Realm.GetInstance();
    }
}
