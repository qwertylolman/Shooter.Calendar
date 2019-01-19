using System.Collections;
using System.Windows.Input;
using MvvmCross.Commands;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public interface IListViewModel
    {
        IEnumerable ItemsCollection { get; }

        ICommand ItemSelectedCommand { get; }

        IMvxAsyncCommand LoadDataCommand { get; }
    }
}
