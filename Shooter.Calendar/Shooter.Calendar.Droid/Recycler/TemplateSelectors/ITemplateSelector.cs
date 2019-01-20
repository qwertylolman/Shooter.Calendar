using System;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Shooter.Calendar.Droid.Recycler.Adapters.TemplateSelectors
{
	public interface ITemplateSelector : IMvxTemplateSelector
    {
        Type GetItemViewHolderType(int templateId);
    }
}
