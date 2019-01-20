using System;
using System.Collections.Generic;
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
    public class WeaponEditViewModel : PageViewModel<Weapon, Weapon>
    {
        private readonly List<Gauge> gauges;

        private Weapon weapon;

        public WeaponEditViewModel()
        {
            gauges = new List<Gauge>();

            SaveCommand = new MvxAsyncCommand(Save, CanSave);
        }

        public IEnumerable<Gauge> Gauges
            => gauges;

        public IMvxAsyncCommand SaveCommand { get; }

        public string Gauge { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(Weapon parameter)
        {
            base.Prepare(parameter);

            weapon = parameter;
        }

        private async Task Save()
        {
            if (weapon == null)
            {
                weapon = RealmObjectBuilder.Build<Weapon>();
            }

            var realm = RealmProvider.GetInstance();
            var gaugeName = Gauge;
            if (string.IsNullOrEmpty(gaugeName) == false)
            {
                var gauge = realm.All<Gauge>().FirstOrDefault(g => string.Equals(g.Name, gaugeName, StringComparison.OrdinalIgnoreCase));
                if (gauge == null)
                {
                    var result =
                        await NavigationService.ShowOkCancelDialogAsync(
                            string.Empty,
                            LocalizationExtensions.Get("weapon_edit_gauge_not_found"));

                    if (result.IsNegative() == true)
                    {
                        return;
                    }

                    gauge = RealmObjectBuilder.Build<Gauge>();
                    gauge.Name = gaugeName;
                }

                weapon.Gauge = gauge;
            }

            weapon.Name = Name;
            weapon.Description = Description;

            realm.Write(() => realm.Add(weapon, update: true));

            Result = weapon;

            await CloseCommand.ExecuteAsync();
        }

        public bool CanSave()
            => true;

        protected override Task LoadDataAsync(CancellationToken ct)
        {
            var realm = RealmProvider.GetInstance();

            gauges.AddRange(realm.All<Gauge>());

            if (weapon != null)
            {
                Gauge = weapon.Gauge?.Name;
                Name = weapon.Name;
                Description = weapon.Description;
            }

            return base.LoadDataAsync(ct);
        }
    }
}
