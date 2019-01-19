using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class PageViewModel<TParameter> : PageViewModel, IMvxViewModel<TParameter>
    {
        public virtual void Prepare(TParameter parameter)
        {
        }
    }

    public abstract class PageViewModel<TParameter, TResult> : PageViewModel, IMvxViewModel<TParameter, TResult>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public virtual void Prepare(TParameter parameter)
        {
        }

        protected virtual TResult Result { get; set; }

        protected override Task<bool> PerformCloseAsync()
            => PerformCloseAsync(Result);

        protected virtual async Task<bool> PerformCloseAsync(TResult result)
        {
            await NavigationService.Close(this, Result);

            return true;
        }
    }

    public abstract class PageViewModel : SimpleViewModel
    {
        private bool isCloseAlreadyExecuted;

        protected PageViewModel()
        {
            LoadDataCommand = new MvxAsyncCommand(LoadData);
            CloseCommand = new MvxAsyncCommand(Close);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }

        public virtual void ViewLoaded()
        {
        }

        private async Task LoadData(CancellationToken ct)
        {
            await LoadDataAsync(ct);
            await RaiseAllPropertiesChanged();
        }

        protected virtual async Task LoadDataAsync(CancellationToken ct)
        {
            await Task.Yield();
        }

        private async Task Close()
        {
            if (isCloseAlreadyExecuted)
            {
                return;
            }

            if (await PerformCloseAsync() == false)
            {
                return;
            }

            DoUnsubscribe();

            isCloseAlreadyExecuted = true;
        }

        protected virtual async Task<bool> PerformCloseAsync()
        {
            await NavigationService.Close(this);

            return true;
        }

        public sealed override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            DoUnsubscribe();
            OnViewDestroy(viewFinishing);
        }

        protected virtual void OnViewDestroy(bool viewFinishing)
        {
        }
    }
}
