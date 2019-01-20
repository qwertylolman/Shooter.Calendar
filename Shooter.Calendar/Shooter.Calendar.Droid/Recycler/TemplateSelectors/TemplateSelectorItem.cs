using System;
using Java.Lang;

namespace Shooter.Calendar.Droid.Recycler.Adapters.TemplateSelectors
{
	public class TemplateSelectorItem
    {
        public TemplateSelectorItem(int resourceId, Type itemType, Type viewHolderType)
        {
            if (viewHolderType.IsSubclassOf(typeof(ViewHolders.CardViewHolder)) == false
                 && viewHolderType != typeof(ViewHolders.CardViewHolder))
            {
                throw new IllegalStateException("viewHolderType must derived from CardViewHolder");
            }

            ResourceId = resourceId;
            ItemType = itemType;
            ViewHolderType = viewHolderType;
        }

		public int ResourceId { get; }

        public Type ItemType { get; }

        public Type ViewHolderType { get; }

        public static TemplateSelectorItem Produce<TItemType, TViewHolderType>(int resourceId)
            where TViewHolderType : ViewHolders.CardViewHolder
            => new TemplateSelectorItem(resourceId, typeof(TItemType), typeof(TViewHolderType));
    }
}
