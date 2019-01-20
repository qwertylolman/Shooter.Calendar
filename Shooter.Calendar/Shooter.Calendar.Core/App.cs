using MvvmCross.ViewModels;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Managers.KeyGenerator;
using Shooter.Calendar.Core.RealmExtra.CopyManager;
using Shooter.Calendar.Core.RealmExtra.CopyManager.PredefinedProviders;
using Shooter.Calendar.Core.ViewModels.MainPage;

namespace Shooter.Calendar.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<MainViewModel>();

            RegisterInIoc();

            Localization.InitializationExtensions.Initialize();
        }

        private void RegisterInIoc()
        {
            IoCExtensions.RegisterSingleton<IKeyGenerator, KeyGenerator>();

            IoCExtensions.RegisterSingleton<ICopyManager, CopyManager>();

            IoCExtensions.RegisterManyAsSingleton<ICopyManagerProvider, ReflectionCopyProvider>();
        }
    }
}
