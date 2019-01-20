using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Common.RealmExtensions.Extensions;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class GaugeEditViewModel : PageViewModel<Gauge, Gauge>
    {
        private Gauge gauge;

        public GaugeEditViewModel()
        {
            SaveCommand = new MvxAsyncCommand(Save, CanSave);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public string Description { get; set; }

        public string Name { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(Gauge parameter)
        {
            base.Prepare(parameter);

            gauge = parameter;
        }

        private Task Save()
        {
            if (gauge == null)
            {
                gauge = RealmObjectBuilder.Build<Gauge>();
            }

            gauge.Name = Name;
            gauge.Description = Description;

            RealmProvider.Write(r => r.Add(gauge, update: true));

            Result = gauge;

            return CloseCommand.ExecuteAsync();
        }

        private bool CanSave()
            => true;

        protected override Task LoadDataAsync(CancellationToken ct)
        {
            if (gauge != null)
            {
                Name = gauge.Name;
                Description = gauge.Description;
            }

            return base.LoadDataAsync(ct);
        }
    }
}
