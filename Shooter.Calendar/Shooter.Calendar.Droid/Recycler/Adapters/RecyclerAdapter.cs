using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Shooter.Calendar.Droid.Recycler.Adapters.TemplateSelectors;

namespace Shooter.Calendar.Droid.Recycler.Adapters
{
    public class RecyclerAdapter : MvxRecyclerAdapter
    {
        public RecyclerAdapter(IMvxBindingContext bindingContext)
            : this((IMvxAndroidBindingContext)bindingContext)
        {
        }

        public RecyclerAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        public RecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            if (!(ItemTemplateSelector is ITemplateSelector))
            {
                throw new Exception("ItemTemplateSelector must implement ITemplateSelector");
            }

            var templateSelector = (ITemplateSelector)ItemTemplateSelector;

            var viewHolder =
                Activator.CreateInstance(templateSelector.GetItemViewHolderType(viewType),
                                         itemBindingContext.BindingInflate(templateSelector.GetItemLayoutId(viewType),
                                                                           parent,
                                                                           false),
                                         itemBindingContext)
                         as MvxRecyclerViewHolder;

            viewHolder.Click = ItemClick;
            viewHolder.LongClick = ItemLongClick;

            return viewHolder;
        }
    }
}
