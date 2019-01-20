using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class ShotsCounterViewModel : PageViewModel<int, int>
    {
        public ShotsCounterViewModel()
        {
            SaveCommand = new MvxAsyncCommand(Save);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public int MinShotsCount
            => 1;

        public int MaxShotsCount
            => int.MaxValue;

        public int ShotsCount { get; set; } = 1;

        public override void Prepare(int parameter)
        {
            base.Prepare(parameter);

            Result = parameter;

            if (parameter > 0)
            {
                ShotsCount = parameter;
            }
        }

        private Task Save()
        {
            Result = ShotsCount;

            return CloseCommand.ExecuteAsync();
        }
    }
}
