using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Droid.Binder;
using Shooter.Calendar.Droid.Extensions;
using Shooter.Calendar.Droid.Listeners;

namespace Shooter.Calendar.Droid.Views.Abstract
{
    public class FragmentBase<TViewModel> : MvxFragment<TViewModel>, Toolbar.IOnMenuItemClickListener
    where TViewModel : PageViewModel
    {
        public const int DefaultResourceId = -1;

        private readonly int contentResourceId = DefaultResourceId;
        private Unbinder viewUnbinder;

        #region Constructor

        protected FragmentBase()
        {
        }

        protected FragmentBase(int contentResourceId)
        {
            this.contentResourceId = contentResourceId;
        }

        public FragmentBase(IntPtr jRef, JniHandleOwnership jho)
            : base(jRef, jho)
        {
        }

        #endregion

        #region Overrides

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            if (contentResourceId > DefaultResourceId)
            {
                view = this.BindingInflate(contentResourceId, null);
                OnCreateContentView(view);
                viewUnbinder = ViewBinder.Bind(view, this);
            }

            InitFieldsFromContainer(Mvx.IoCProvider);

            InitViewProperties(view, savedInstanceState);
            InitTransitionNames(view, Activity);

            if (savedInstanceState != null)
            {
                DoOnRestoreInstanceState(savedInstanceState);
            }

            var toolbarId = GetToolbarId();
            Toolbar = view.FindViewById<Toolbar>(toolbarId);

            if (Toolbar != null)
            {
                SetupToolbar(Toolbar);
            }

            DoBind();

            return view;
        }

        protected virtual void OnCreateContentView(View view)
        {
        }

        protected virtual void SetupToolbar(Toolbar toolbar)
        {
            HasOptionsMenu = GetMenuResourceId() != DefaultResourceId;

            toolbar.SetOnMenuItemClickListener(this);
            toolbar.SetNavigationOnClickListener(new ViewOnClickListener(v => OnNavigationIconClick()));

            if (ToolbarHasElevation() == false)
            {
                toolbar.Elevation = 0;
            }

            if (HasNavigationIcon() == true)
            {
                toolbar.SetNavigationIcon(GetNavigationIconId());
            }

            SetToolbarTitle();
        }

        public override void OnDestroyView()
        {
            viewUnbinder?.Unbind();
            base.OnDestroyView();
        }

        public override void OnDestroy()
        {
            ViewModel?.Unsubscribe();
            base.OnDestroy();
        }

        #endregion

        #region Fragment config methods

        protected virtual void InitFieldsFromContainer(IMvxIoCProvider container)
        {
        }

        protected virtual void InitViewProperties(View view, Bundle bundle)
        {
        }

        protected virtual void InitTransitionNames(View view, Context context)
        {
        }

        protected virtual void InitPropertiesFromViewModelBundle(Bundle bundle)
        {
        }

        protected virtual void DoOnRestoreInstanceState(Bundle savedInstanceState)
        {
        }

        protected virtual void DoBind()
        {
        }

        #endregion

        private void OnViewFakeClick(View v)
        {
        }

        public void CloseKeyboard()
        {
            ActivityExtensions.CloseKeyboard(Activity);
        }

        public void OpenKeyboad(View view)
        {
            ActivityExtensions.OpenKeyboard(Activity, view);
        }

        public const int HomeButtonId = Android.Resource.Id.Home;

        public Toolbar Toolbar { get; private set; }

        public string ToolbarTitle
        {
            get { return Toolbar.Title; }
            set { Toolbar.Title = value; }
        }

        protected virtual int GetToolbarId() => DefaultResourceId;

        protected virtual bool ToolbarHasElevation() => false;

        protected virtual bool HasNavigationIcon() => true;

        protected virtual int GetNavigationIconId() => DefaultResourceId;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            int menuResoruceId;
            if (GetMenuResourceId(out menuResoruceId) == false)
            {
                return;
            }

            inflater.Inflate(menuResoruceId, menu);
        }

        protected virtual bool GetMenuResourceId(out int menuResosurceId)
        {
            menuResosurceId = GetMenuResourceId();
            var isValidResourceId = menuResosurceId != DefaultResourceId;

            return isValidResourceId;
        }

        protected virtual int GetMenuResourceId() => DefaultResourceId;

        protected virtual void SetToolbarTitle()
        {
            var titleStringResourceId = GetToolbarTitleStringId();
            string toolbarTitle;
            if (titleStringResourceId == DefaultResourceId)
            {
                toolbarTitle = GetToolbarTitle();
            }
            else
            {
                toolbarTitle = Resources.GetString(titleStringResourceId);
            }

            ToolbarTitle = toolbarTitle;
        }

        protected virtual int GetToolbarTitleStringId()
            => DefaultResourceId;

        protected virtual string GetToolbarTitle()
            => string.Empty;

        public virtual bool OnMenuItemClick(IMenuItem item)
        {
            var viewModel = ViewModel;
            if (item.ItemId == HomeButtonId && viewModel != null)
            {
                viewModel.CloseCommand.Execute();

                return true;
            }

            return false;
        }

        protected virtual void OnNavigationIconClick()
        {
            ViewModel.CloseCommand.Execute();
        }
    }
}
