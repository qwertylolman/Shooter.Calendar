using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Common.Extensions.DialogExtensions;
using Shooter.Calendar.Core.Common.Extensions.RealmExtensions;
using Shooter.Calendar.Core.Localization;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class ShotEditViewModel : PageViewModel<Shot, Shot>
    {
        private Shot shot;

        public ShotEditViewModel()
        {
            SaveCommand = new MvxAsyncCommand(Save, CanSave);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public int NumberOfShots { get; set; }

        public string Weapon { get; set; }

        public string ShotZone { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(Shot parameter)
        {
            base.Prepare(parameter);

            shot = parameter;
        }

        protected override Task LoadDataAsync(CancellationToken ct)
        {
            if (shot != null)
            {
                Weapon = shot.Weapon?.Name;
                ShotZone = shot.ShotZone?.Name;
                NumberOfShots = shot.BulletsCount;
            }

            return base.LoadDataAsync(ct);
        }

        private async Task Save()
        {
            if (shot == null)
            {
                shot = RealmObjectBuilder.Build<Shot>();
            }

            var realm = RealmProvider.GetInstance();

            var weaponName = Weapon;
            if (string.IsNullOrEmpty(weaponName) == true)
            {
                var weapon = realm.All<Weapon>().FirstOrDefault(g => string.Equals(g.Name, weaponName, StringComparison.OrdinalIgnoreCase));
                if (weapon == null)
                {
                    var result =
                        await NavigationService.ShowOkCancelDialogAsync(
                            string.Empty,
                            LocalizationExtensions.Get("shot_edit_weapon_not_found"));

                    if (result.IsNegative() == true)
                    {
                        return;
                    }

                    weapon = RealmObjectBuilder.Build<Weapon>();
                    weapon.Name = weaponName;
                }

                shot.Weapon = weapon;
            }

            var shotZoneName = ShotZone;
            if (string.IsNullOrEmpty(shotZoneName) == true)
            {
                var shotZone = realm.All<ShotZone>().FirstOrDefault(g => string.Equals(g.Name, shotZoneName, StringComparison.OrdinalIgnoreCase));
                if (shotZone == null)
                {
                    var result =
                        await NavigationService.ShowOkCancelDialogAsync(
                            string.Empty,
                            LocalizationExtensions.Get("shot_edit_shotzone_not_found"));

                    if (result.IsNegative() == true)
                    {
                        return;
                    }

                    shotZone = RealmObjectBuilder.Build<ShotZone>();
                    shotZone.Name = weaponName;
                }

                shot.ShotZone = shotZone;
            }

            realm.Write(() => realm.Add(shot, update: true));

            Result = shot;

            await CloseCommand.ExecuteAsync();
        }

        private bool CanSave()
            => true;

    }
}
