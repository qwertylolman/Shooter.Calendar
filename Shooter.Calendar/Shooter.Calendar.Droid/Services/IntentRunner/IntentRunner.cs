using Android.Content;
using MvvmCross.Platforms.Android;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common.Extensions;

namespace Shooter.Calendar.Droid.Services.IntentRunner
{
    public class IntentRunner : IIntentRunner
    {
        private readonly IMvxAndroidGlobals androidGlobals;

        public IntentRunner([NotNull] IMvxAndroidGlobals androidGlobals)
        {
            this.androidGlobals = androidGlobals;
        }

        /// <summary>
        /// Runs intent. All occured exceptions will be logged and suppressed
        /// </summary>
        /// <param name="intent">Intent to run</param>
        public void RunSafe(Intent intent)
        {
            try
            {
                Run(intent);
            }
            catch (ActivityNotFoundException e)
            {
                e.LogToConsole();
            }
        }

        /// <summary>
        /// Runs intent
        /// </summary>
        /// <param name="intent">Intent to run</param>
        public void Run(Intent intent)
        {
            androidGlobals.ApplicationContext.StartActivity(intent);
        }

        /// <summary>
        /// Runs intent. ActivityNotFound exceptions will be logged and suppressed, Activity will be run as new Task
        /// </summary>
        /// <param name="intent">Intent to run</param>
        public void RunSafeAsNewTask(Intent intent)
        {
            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.NewTask);
            RunSafe(intent);
        }
    }
}
