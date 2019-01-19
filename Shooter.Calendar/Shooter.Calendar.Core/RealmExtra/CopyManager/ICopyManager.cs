using Realms;

namespace Shooter.Calendar.Core.RealmExtra.CopyManager
{
    public interface ICopyManager
    {
        RealmObject MakeACopy(RealmObject realmObject);
    }
}
