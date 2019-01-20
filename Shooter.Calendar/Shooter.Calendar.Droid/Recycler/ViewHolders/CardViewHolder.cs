using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Shooter.Calendar.Droid.Binder;

namespace Shooter.Calendar.Droid.Recycler.Adapters.ViewHolders
{
    public class CardViewHolder : MvxRecyclerViewHolder
    {
        private Unbinder unbinder;

        public CardViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
            Init(view);
        }

        private void Init(View view)
        {
			unbinder = GetUnbinder(view, this);
            DoInit(view);
            SetData(DataContext);

			this.DelayBind(BindData);
        }

		protected virtual Unbinder GetUnbinder(View view, object target)
		    => ViewBinder.Bind(view, target);

        protected virtual void DoInit(View view)
        {
        }

        public virtual void ClearAnimation()
        {
            ItemView.ClearAnimation();
        }

        public override void OnViewRecycled()
        {
            base.OnViewRecycled();

            ClearAnimation();
        }

        public virtual void BindData()
        {
        }

        public virtual void SetData(object dataContext)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            unbinder.Unbind();
        }
    }
}
