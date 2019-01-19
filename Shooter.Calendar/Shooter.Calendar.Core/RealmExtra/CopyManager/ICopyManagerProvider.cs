using Realms;

namespace Shooter.Calendar.Core.RealmExtra.CopyManager
{
    public interface ICopyManagerProvider
    {
        bool CanHandle(RealmObject realmObject);

        RealmObject MakeACopy(RealmObject realmObject);
    }
}
