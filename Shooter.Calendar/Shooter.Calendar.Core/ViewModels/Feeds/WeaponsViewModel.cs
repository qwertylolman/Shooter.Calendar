using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Common.Extensions.RealmExtensions;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Core.ViewModels.EditorPages;

namespace Shooter.Calendar.Core.ViewModels.Feeds
{
    public class WeaponsViewModel : ListViewModel
    {
        public WeaponsViewModel()
        {
            AddWeaponCommand = new MvxAsyncCommand(AddWeapon);
        }

        public IMvxAsyncCommand AddWeaponCommand { get; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        protected override Task<IEnumerable<object>> GetItemsAsync(CancellationToken ct)
            => Task.FromResult<IEnumerable<object>>(RealmProvider.GetInstance().All<Weapon>().ToList());

        protected override async Task OnItemSelectedAsync(object item)
        {
            if (item is Weapon weapon)
            {
                var itemIndex = ObservableCollection.IndexOf(weapon);
                var changedWeapon =
                    await NavigationService.Navigate<WeaponEditViewModel, Weapon, Weapon>(param: weapon);

                if (changedWeapon == null)
                {
                    return;
                }

                ObservableCollection.RemoveAt(itemIndex);
                ObservableCollection.Insert(itemIndex, changedWeapon);
                return;
            }

            await base.OnItemSelectedAsync(item);
        }

        private async Task AddWeapon()
        {
            var weapon = await NavigationService.Navigate<WeaponEditViewModel, Weapon, Weapon>(param: null);
            if (weapon == null)
            {
                return;
            }

            ObservableCollection.Add(weapon);
        }
    }
}
