using MvvmCross.ViewModels;
using Shooter.Calendar.Core.Common;
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
            IoCExtensions.RegisterSingleton<IKeyGenerator, KeyGenerator>();
        }
    }
}
