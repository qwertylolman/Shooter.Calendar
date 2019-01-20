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
    public class ShotZonesViewModel : ListViewModel
    {
        public ShotZonesViewModel()
        {
            AddShotZoneCommand = new MvxAsyncCommand(AddShotZone);
        }

        public IMvxAsyncCommand AddShotZoneCommand { get; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        protected override Task<IEnumerable<object>> GetItemsAsync(CancellationToken ct)
            => Task.FromResult<IEnumerable<object>>(RealmProvider.GetInstance().All<ShotZone>().ToList());

        protected override async Task OnItemSelectedAsync(object item)
        {
            if (item is ShotZone shotZone)
            {
                var itemIndex = ObservableCollection.IndexOf(shotZone);
                var changedWeapon =
                    await NavigationService.Navigate<ShotZoneEditorViewModel, ShotZone, ShotZone>(param: shotZone);

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

        private async Task AddShotZone()
        {
            var weapon = await NavigationService.Navigate<ShotZoneEditorViewModel, ShotZone, ShotZone>(param: null);
            if (weapon == null)
            {
                return;
            }

            ObservableCollection.Add(weapon);
        }
    }
}
