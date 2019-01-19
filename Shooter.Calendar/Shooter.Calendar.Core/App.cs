using MvvmCross;
using MvvmCross.ViewModels;
using Shooter.Calendar.Core.Managers.KeyGenerator;
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
        }

        private void RegisterInIoc()
        {
            RegisterSingleton<IKeyGenerator, KeyGenerator>();
        }

        public static void RegisterSingleton<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            Mvx.IoCProvider.RegisterSingleton<TInterface>(() => Mvx.IoCProvider.Resolve<TService>());
        }
    }
}
