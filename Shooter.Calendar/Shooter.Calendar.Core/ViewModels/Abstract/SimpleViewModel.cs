using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class SimpleViewModel : MvxViewModel
    {
        protected SimpleViewModel()
        {
            InitializeCommand = new MvxAsyncCommand(InitializeAsync);
        }

        protected IMvxMessenger Messenger 
            => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        protected IMvxNavigationService NavigationService 
            => Mvx.IoCProvider.Resolve<IMvxNavigationService>();

        public IMvxAsyncCommand InitializeCommand { get; }

        /// <summary>
        /// Make any your custom logic in InitializeAsync
        /// </summary>
        /// <returns></returns>
        public sealed override async Task Initialize()
        {
            await base.Initialize();

            Subscribe();

            await InitializeCommand.ExecuteAsync();
        }

        protected virtual async Task InitializeAsync()
        {
            await Task.Yield();
        }

        public void Subscribe()
        {
            DoSubscribe();
        }

        protected virtual void DoSubscribe()
        {
        }

        public void Unsubscribe()
        {
            DoUnsubscribe();
        }

        protected virtual void DoUnsubscribe()
        {
        }
    }
}