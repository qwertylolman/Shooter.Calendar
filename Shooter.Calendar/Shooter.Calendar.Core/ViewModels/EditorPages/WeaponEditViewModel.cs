using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Core.Common.RealmExtensions;
using System.Linq;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class WeaponEditViewModel : PageViewModel<Weapon, Weapon>
    {
        private readonly List<Gauge> gauges;

        private Weapon weapon;

        public WeaponEditViewModel()
        {
            gauges = new List<Gauge>();

            SaveCommand = new MvxCommand(Save, CanSave);
        }

        public IEnumerable<Gauge> Gauges
            => gauges;

        public IMvxCommand SaveCommand { get; }

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

        public void Save()
        {
            if (weapon == null)
            {
                weapon = RealmObjectBuilder.Build<Weapon>();
            }

            var realm = RealmProvider.GetInstance();

            if (string.IsNullOrEmpty(Gauge) == false)
            {
                var gauges = realm.All<Gauge>().FirstOrDefault(g => string.Equals(g.Name, Gauge));
            }

            realm.Add(weapon, update: true);
        }

        public bool CanSave()
        {
            return true;
        }

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
