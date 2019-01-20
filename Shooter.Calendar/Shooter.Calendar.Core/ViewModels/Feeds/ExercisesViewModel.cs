using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common.Extensions;
using Shooter.Calendar.Core.Common.Extensions.RealmExtensions;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Core.ViewModels.EditorPages;

namespace Shooter.Calendar.Core.ViewModels.Feeds
{
    public class ExercisesViewModel : ListViewModel
    {
        public ExercisesViewModel()
        {
            AddExerciseCommand = new MvxAsyncCommand(AddExercise);
            DeleteExerciseCommand = new MvxAsyncCommand<Exercise>(DeleteExercise);
        }

        public IMvxAsyncCommand AddExerciseCommand { get; }

        public IMvxAsyncCommand<Exercise> DeleteExerciseCommand { get; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        protected override Task<IEnumerable<object>> GetItemsAsync(CancellationToken ct)
            => Task.FromResult<IEnumerable<object>>(RealmProvider.GetInstance().All<Exercise>().ToList());

        protected override async Task OnItemSelectedAsync(object item)
        {
            if (item is Exercise exercise)
            {
                var itemIndex = ObservableCollection.IndexOf(exercise);
                var changedExercise = 
                    await NavigationService.Navigate<ExerciseEditViewModel, Exercise, Exercise>(param: exercise);

                if (changedExercise == null)
                {
                    return;
                }

                ObservableCollection.RemoveAt(itemIndex);
                ObservableCollection.Insert(itemIndex, changedExercise);
                return;
            }

            await base.OnItemSelectedAsync(item);
        }

        private async Task AddExercise()
        {
            var exercise = await NavigationService.Navigate<ExerciseEditViewModel, Exercise, Exercise>(param: null);
            if (exercise == null)
            {
                return;
            }

            ObservableCollection.Add(exercise);
        }

        private async Task DeleteExercise([NotNull] Exercise exercise)
        {
            if (await NavigationService.ShowAreYouSureDialog() == false)
            {
                return;
            }

            if (exercise.IsManaged == true)
            {
                RealmProvider.Write(r =>
                {
                    foreach(var shot in exercise.Shots)
                    {
                        r.Remove(shot);
                    }

                    r.Remove(exercise);
                });
            }

            ObservableCollection.Remove(exercise);
        }
    }
}
