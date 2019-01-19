using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.Dialog
{
    public abstract class DialogBaseViewModel<TParameter> : ListViewModel<TParameter, IButton>
        where TParameter : DialogParameterArgs
    {
        private readonly List<IButton> buttons;

        protected DialogBaseViewModel()
        {
            buttons = new List<IButton>();
        }

        public string Title { get; set; }

        public string Text { get; set; }

        public override void Prepare(TParameter parameter)
        {
            buttons.AddRange(parameter.Buttons);

            Text = parameter.Text;
            Title = parameter.Title;
        }

        protected override Task<IEnumerable<object>> GetItemsAsync(CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<object>>(buttons);

        protected override Task OnItemSelectedAsync(object item)
        {
            if (item is IButton button)
            {
                Result = button;
                return CloseCommand.ExecuteAsync();
            }

            return base.OnItemSelectedAsync(item);
        }
    }
}
