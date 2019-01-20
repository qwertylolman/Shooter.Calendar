using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Common.Extensions.RealmExtensions;
using Shooter.Calendar.Core.Localization;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class ShotZoneEditorViewModel : PageViewModel<ShotZone, ShotZone>
    {
        private ShotZone shotZone;

        public ShotZoneEditorViewModel()
        {
            SaveCommand = new MvxAsyncCommand(Save, CanSave);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public string Description { get; set; }

        public string Name { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(ShotZone parameter)
        {
            base.Prepare(parameter);

            shotZone = parameter;
        }

        private async Task Save()
        {
            if (shotZone == null)
            {
                shotZone = RealmObjectBuilder.Build<ShotZone>();
            }

            var shotZoneName = Name;

            var realm = RealmProvider.GetInstance();
            if (realm.All<ShotZone>().Any(z => string.Equals(z.Name, shotZoneName, StringComparison.OrdinalIgnoreCase)) == true)
            {
                await NavigationService.ShowDialogAsync(
                    string.Empty,
                    LocalizationExtensions.Get("shotzone_edit_exists"));

                return;
            }

            shotZone.Name = shotZoneName;
            shotZone.Description = Description;

            realm.Write(() => realm.Add(shotZone, update: true));

            Result = shotZone;

            await CloseCommand.ExecuteAsync();
        }

        private bool CanSave()
            => true;

        protected override Task LoadDataAsync(CancellationToken ct)
        {
            if (shotZone != null)
            {
                Name = shotZone.Name;
                Description = shotZone.Description;
            }

            return base.LoadDataAsync(ct);
        }
    }
}
