using System;
using System.Collections.Generic;
using Shooter.Calendar.Core.Attributes;
using Shooter.Calendar.Core.Common.Extensions;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class EntityListItemViewModel<TEntity> : ListItemViewModel
    {
        private TEntity entity;

        protected EntityListItemViewModel([NotNull] TEntity entity)
        {
            Entity = entity;
        }

        protected virtual bool CheckEntityForEqualityOnSetter
            => true;

        public TEntity Entity
        {
            get { return entity; }
            set
            {
                if (CheckEntityForEqualityOnSetter == true
                    && EqualityComparer.Equals(entity, value) == true)
                {
                    return;
                }

                var oldEntity = entity;
                entity = value;
                OnEntityChanged(oldEntity, value);
            }
        }

        protected virtual IEqualityComparer<TEntity> EqualityComparer
            => EqualityComparer<TEntity>.Default;

        protected virtual void OnEntityChanged(TEntity oldEntity, TEntity newEntity)
        {
            RaisePropertyChanged(nameof(Entity));
        }

        protected TResult GetFromEntity<TResult>([NotNull] Func<TEntity, TResult> getter)
        {
            var entity = Entity;
            entity.ThrowIfNull(nameof(entity));

            return getter(entity);
        }
    }
}
