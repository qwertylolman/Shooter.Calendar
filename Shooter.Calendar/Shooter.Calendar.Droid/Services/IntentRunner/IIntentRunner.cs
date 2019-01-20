using Android.Content;

namespace Shooter.Calendar.Droid.Services.IntentRunner
{
    public interface IIntentRunner
    {
        void Run(Intent intent);

        void RunSafe(Intent intent);

        void RunSafeAsNewTask(Intent intent);
    }
}
