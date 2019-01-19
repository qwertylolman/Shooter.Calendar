using MvvmCross.Commands;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public interface IListItem
    {
        IMvxAsyncCommand SelectCommand { get; }
    }
}
