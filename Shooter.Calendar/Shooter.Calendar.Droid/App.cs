using MvvmCross;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Logger;
using Shooter.Calendar.Droid.Services.IntentRunner;

namespace Shooter.Calendar.Droid
{
    public class App : Core.App
    {
        public override void Initialize()
        {
            base.Initialize();

            IoCExtensions.RegisterSingleton<IIntentRunner, IntentRunner>();

            Mvx.IoCProvider.RegisterSingleton<ILogger>(() => new Logger.Logger("Shooter.Calendar"));
        }
    }
}
