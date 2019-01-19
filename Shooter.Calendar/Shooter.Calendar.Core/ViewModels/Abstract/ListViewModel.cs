using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class ListViewModel<TParameter, TResult> : ListViewModel<TParameter>, IMvxViewModel<TParameter, TResult>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        protected virtual TResult Result { get; set; }

        protected sealed override Task<bool> PerformCloseAsync()
            => PerformCloseAsync(Result);

        protected virtual Task<bool> PerformCloseAsync(TResult result)
            => NavigationService.Close(this, result);
    }

    public abstract class ListViewModel<TParameter> : ListViewModel, IMvxViewModel<TParameter>
    {
        public virtual void Prepare(TParameter parameter)
        {
        }
    }

    public abstract class ListViewModel : GenericListViewModel<object>, IListViewModel
    {
    }
}
