using Realms;

namespace Shooter.Calendar.Core.Common.RealmExtensions
{
    public static class RealmProvider
    {
        public static Realm GetInstance()
            => Realm.GetInstance();
    }
}
