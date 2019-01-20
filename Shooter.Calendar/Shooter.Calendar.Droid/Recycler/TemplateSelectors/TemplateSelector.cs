using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Droid.Recycler.Adapters.TemplateSelectors
{
    public class TemplateSelector : ITemplateSelector
    {
        public static int ItemTemplateIdDefault = Resource.Layout.view_item_lists_default;

        private readonly Dictionary<Type, int> ItemTypeToViewTypeIdMappings = new Dictionary<Type, int>();
        private readonly Dictionary<int, Type> ViewTypeIdToViewHolderTypeMappings = new Dictionary<int, Type>();
        private readonly Dictionary<int, int> ViewTypeIdToResourceIdMappings = new Dictionary<int, int>();

        public TemplateSelector()
        {
        }

        public int ItemTemplateId { get; set; } = ItemTemplateIdDefault;

        public TemplateSelector([NotNull] IEnumerable<TemplateSelectorItem> items)
        {
            if (items.Any() == false)
            {
                throw new ArgumentException("items cannot be null or empty");
            }

            foreach (var item in items)
            {
                AddElement(item);
            }
        }

        public TemplateSelector(TemplateSelectorItem item) 
			: this(new TemplateSelectorItem[] { item })
        {
        }

        public TemplateSelector(int resourceId, Type itemType, Type viewHolderType) 
			: this(new TemplateSelectorItem[] { new TemplateSelectorItem(resourceId, itemType, viewHolderType) })
        {
        }

        public TemplateSelector AddElement<TItemType, TViewHolderType>(int resourceId)
            where TViewHolderType : ViewHolders.CardViewHolder
        {
            AddElement(TemplateSelectorItem.Produce<TItemType, TViewHolderType>(resourceId));

            return this;
        }

        public TemplateSelector AddDefaultElement<TItemType>()
        {
            AddElement(TemplateSelectorItem.Produce<TItemType, ViewHolders.CardViewHolder>(ItemTemplateId));

            return this;
        }

        public void AddElement([NotNull] TemplateSelectorItem item)
        {
            var viewTypeId = $"{item.ItemType.Name}.{item.ViewHolderType}".GetHashCode();

            ItemTypeToViewTypeIdMappings.TryAdd(item.ItemType, viewTypeId);
            ViewTypeIdToViewHolderTypeMappings.TryAdd(viewTypeId, item.ViewHolderType);
            ViewTypeIdToResourceIdMappings.TryAdd(viewTypeId, item.ResourceId);
        }

        public virtual int GetItemLayoutId(int fromViewType)
        {
            int resourceId;
            if (ViewTypeIdToResourceIdMappings.TryGetValue(fromViewType, out resourceId) == true)
            {
                return resourceId;
            }

            throw new ArgumentOutOfRangeException($"ViewTypeIdToResourceIdMappings doesn't contain key {fromViewType}");
        }

        public virtual int GetItemViewType(object forItemObject)
        {
            int viewTypeId;

            if (ItemTypeToViewTypeIdMappings.TryGetValue(forItemObject.GetType(), out viewTypeId) == true)
            {
                return viewTypeId;
            }

            throw new ArgumentOutOfRangeException($"ItemTypeToViewTypeIdMappings doesn't contain key {forItemObject.GetType()}");
        }

        public virtual Type GetItemViewHolderType(int fromViewType)
        {
            Type viewHolderType;

            if (ViewTypeIdToViewHolderTypeMappings.TryGetValue(fromViewType, out viewHolderType) == true)
            {
                return viewHolderType;
            }

            throw new ArgumentOutOfRangeException($"ViewTypeIdToViewHolderTypeMappings doesn't contain key {fromViewType}");
        }

        Type ITemplateSelector.GetItemViewHolderType(int templateId)
        {
            throw new NotImplementedException();
        }

        int IMvxTemplateSelector.GetItemViewType(object forItemObject)
        {
            throw new NotImplementedException();
        }

        int IMvxTemplateSelector.GetItemLayoutId(int fromViewType)
        {
            throw new NotImplementedException();
        }
    }
}
