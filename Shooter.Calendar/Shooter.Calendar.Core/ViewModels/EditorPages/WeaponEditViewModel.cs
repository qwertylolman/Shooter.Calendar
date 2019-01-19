using MvvmCross.Commands;
using Shooter.Calendar.Core.POCO.Entities;
using Shooter.Calendar.Core.ViewModels.Abstract;

namespace Shooter.Calendar.Core.ViewModels.EditorPages
{
    public class WeaponEditViewModel : PageViewModel<Weapon, Weapon>
    {
        public WeaponEditViewModel()
        {
            SaveCommand = new MvxCommand(Save, CanSave);
        }

        public IMvxCommand SaveCommand { get; set; }

        public override void Prepare(Weapon parameter)
        {
            base.Prepare(parameter);

            Result = parameter;
        }

        public void Save()
        {

        }

        public bool CanSave()
        {
            return true;
        }
    }
}
