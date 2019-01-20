using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Core.Common.RealmExtensions.Extensions;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class ExerciseEditViewModel : ListViewModel<Exercise, Exercise>
    {
        private Exercise exercise;

        public ExerciseEditViewModel()
        {
            SaveCommand = new MvxAsyncCommand(Save, CanSave);
        }

        public IMvxAsyncCommand SaveCommand { get; }

        public IMvxCommand AddShotCommand { get; }

        public string Description { get; set; }

        public string Name { get; set; }

        protected override Task InitializeAsync()
            => Task.WhenAll(base.InitializeAsync(), LoadDataCommand.ExecuteAsync());

        public override void Prepare(Exercise parameter)
        {
            base.Prepare(parameter);

            exercise = parameter;
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
    }
}
