using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common.RealmExtensions.Extensions;
using Shooter.Calendar.Core.Managers.KeyGenerator;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class ExerciseEditViewModel : ListViewModel<Exercise, Exercise>
    {
        private readonly IKeyGenerator keyGenerator;

        private Exercise exercise;

        public ExerciseEditViewModel([NotNull] IKeyGenerator keyGenerator)
        {
            this.keyGenerator = keyGenerator;

            SaveCommand = new MvxAsyncCommand(Save, CanSave);
            AddNewShotCommand = new MvxAsyncCommand(AddNewShot);
            CloneLastShotCommand = new MvxAsyncCommand(CloneLastShot);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public IMvxAsyncCommand AddNewShotCommand { get; }

        public IMvxAsyncCommand CloneLastShotCommand { get; }

        public string Description { get; set; }

        public string Name { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(Exercise parameter)
        {
            base.Prepare(parameter);

            exercise = parameter;
        }

        protected override Task LoadDataAsync(CancellationToken ct)
        {
            if (exercise != null)
            {
                Name = exercise.Name;
                Description = exercise.Description;
            }

            return base.LoadDataAsync(ct);
        }

        protected override Task<IEnumerable<object>> GetItemsAsync(CancellationToken cancellationToken)
        {
            var list = new List<object>();
            if (exercise != null)
            {
                list.AddRange(exercise.Shots);
            }

            return Task.FromResult<IEnumerable<object>>(list);
        }

        private Task Save()
        {
            if (exercise == null)
            {
                exercise = RealmObjectBuilder.Build<Exercise>();
            }

            exercise.Name = Name;
            exercise.Description = Description;

            RealmProvider.Write(r => r.Add(exercise, update: true));

            Result = exercise;

            return CloseCommand.ExecuteAsync();
        }

        private bool CanSave()
            => true;

        private async Task AddNewShot()
        {
            var result = await NavigationService.Navigate<ShotEditViewModel, Shot, Shot>(param: null);
            if (result == null)
            {
                return;
            }

            ObservableCollection.Add(result);
        }

        private async Task CloneLastShot()
        {
            var lastShot = ObservableCollection.OfType<Shot>().LastOrDefault();
            if (lastShot == null)
            {
                await AddNewShotCommand.ExecuteAsync();
                return;
            }

            lastShot = lastShot.CopyItem();
            lastShot.Id = keyGenerator.GenerateKey();

            var shotsCount = await NavigationService.Navigate<ShotsCounterViewModel, int, int>(lastShot.BulletsCount);
            if (shotsCount > 0)
            {
                lastShot.BulletsCount = shotsCount;
            }

            if (lastShot == null)
            {
                return;
            }

            ObservableCollection.Add(lastShot);
        }
    }
}
