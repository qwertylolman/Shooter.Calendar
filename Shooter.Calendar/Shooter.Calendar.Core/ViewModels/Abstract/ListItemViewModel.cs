using System.Threading.Tasks;
using MvvmCross.Commands;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class ListItemViewModel : SimpleViewModel, IListItem
    {
        protected ListItemViewModel()
        {
            SelectCommand = new MvxAsyncCommand(SelectAsync, () => CanSelect);
        }

        public IMvxAsyncCommand SelectCommand { get; }

        protected virtual async Task SelectAsync()
        {
            await Task.Yield();
        }

        protected virtual bool CanSelect
            => true;
    }
}
