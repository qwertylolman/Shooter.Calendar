using Realms;
using System.ComponentModel;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class RealmItemViewModel<TItem> : EntityListItemViewModel<TItem>
        where TItem : RealmObject
    {
#pragma warning disable RECS0108 // Warns about static fields in generic types
        public static bool PassPropertyChangedFromRealmDefault = true;
#pragma warning restore RECS0108 // Warns about static fields in generic types

        protected RealmItemViewModel(TItem item)
            : base(item)
        {
        }

        protected virtual bool PassPropertyChangedFromRealm { get; } = PassPropertyChangedFromRealmDefault;

        protected override void OnEntityChanged(TItem oldEntity, TItem newEntity)
        {
            if (PassPropertyChangedFromRealm == true)
            {
                if (oldEntity != null)
                {
                    oldEntity.PropertyChanged -= OnRealmEntityPropertyChanged;
                }

                if (newEntity != null)
                {
                    newEntity.PropertyChanged += OnRealmEntityPropertyChanged;
                }
            }

            base.OnEntityChanged(oldEntity, newEntity);
        }

        private void OnRealmEntityPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            RaisePropertyChanged(ea);
        }
    }
}
