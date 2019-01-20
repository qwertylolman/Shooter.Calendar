using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using MvvmCross.Droid.Support.V7.AppCompat;
using Shooter.Calendar.Core.ViewModels.Abstract;
using Shooter.Calendar.Droid.Binder;
using MvvmCross;
using MvvmCross.IoC;

namespace Shooter.Calendar.Droid.Views.Activities
{
	public abstract class ActivityBase<TViewModel> : MvxAppCompatActivity<TViewModel>
		where TViewModel : PageViewModel
	{
		public const int DefaultResourceId = -1;
		public const int HomeButtonId = Android.Resource.Id.Home;

		private readonly int contentResourceId = DefaultResourceId;

		private Unbinder viewUnbinder;
        private bool upNavigationEnabled;

		protected ActivityBase()
		{
		}
		
		protected ActivityBase(IntPtr ptr, JniHandleOwnership owner)
			: base(ptr, owner)
		{
		}

		protected ActivityBase(int contentResourceId)
		{
			this.contentResourceId = contentResourceId;
		}

        protected Toolbar Toolbar { get; private set; }

        protected virtual int GetToolbarId() => DefaultResourceId;

		protected virtual int GetHomeAsUpIndicatorDrawableId() => DefaultResourceId;

		protected virtual bool GetUpNavigationEnabled() => true;

		protected virtual bool ToolbarHasElevation() => false;

		protected virtual int GetMenuResourceId() => DefaultResourceId;

		protected virtual int GetToolbarTitleStringId() => DefaultResourceId;

		protected virtual string GetToolbarTitle() => string.Empty;

		public virtual string ToolbarTitle
		{
			get 
			{ 
				return SupportActionBar.Title; 
			}
			set 
			{ 
				SupportActionBar.Title = value; 
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			if (contentResourceId > DefaultResourceId)
			{
				SetContentView(contentResourceId);
				OnCreateContentView();
				viewUnbinder = ViewBinder.Bind(this);
			}

            InitFieldsFromContainer(Mvx.IoCProvider);

            InitViewProperties(bundle);

			var toolbarId = GetToolbarId();
            Toolbar = FindViewById<Toolbar>(toolbarId);

			if (Toolbar != null)
			{
				SetupToolbar(Toolbar);
			}

			DoBind();

			ViewModel?.ViewLoaded();
		}

		protected virtual void OnCreateContentView()
		{
		}

		protected virtual void SetupToolbar(Toolbar toolbar)
		{
			SetSupportActionBar(toolbar);

			SetToolbarTitle();

			upNavigationEnabled = GetUpNavigationEnabled();
			var actionBar = SupportActionBar;
			if (upNavigationEnabled == true)
			{
				actionBar.SetDisplayHomeAsUpEnabled(true);
				actionBar.SetHomeButtonEnabled(true);

				var homeAsUpIndicatorId = GetHomeAsUpIndicatorDrawableId();
				if (homeAsUpIndicatorId != DefaultResourceId)
				{
					actionBar.SetHomeAsUpIndicator(homeAsUpIndicatorId);
				}
			}
			else
			{
				actionBar.SetDisplayHomeAsUpEnabled(false);
				actionBar.SetHomeButtonEnabled(false);
			}

			if (ToolbarHasElevation() == false)
			{
				actionBar.Elevation = 0;
			}
		}

		public override void OnBackPressed()
		{
			ViewModel?.CloseCommand.Execute(null);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing == true)
			{
				DoDispose();
				viewUnbinder?.Unbind();
			}

			base.Dispose(disposing);
		}

		protected virtual void DoBind()
		{
		}

		protected virtual void InitFieldsFromContainer(IMvxIoCProvider container)
		{
		}

		protected virtual void InitViewProperties(Bundle bundle)
		{
		}

		protected virtual void DoDispose()
		{
		}

		public void GoToAndroidHomeLauncher()
		{
			var intent = new Intent(Intent.ActionMain);
			intent.AddCategory(Intent.CategoryHome);
			intent.SetFlags(ActivityFlags.NewTask);
			var intentRunner = Container.Resolve<IIntentRunner>();

			intentRunner.Run(intent);
		}

		public bool CloseKeyboard() 
            => ActivityExtensions.CloseKeyboard(this);

		public bool OpenKeyboad(View view) 
            => ActivityExtensions.OpenKeyboard(this, view);

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


		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			var menuResourceId = GetMenuResourceId();
			if (menuResourceId != DefaultResourceId)
			{
				MenuInflater.Inflate(menuResourceId, menu);
			}

			var flag = base.OnCreateOptionsMenu(menu);

			return flag;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			// this will allow up button navigate like back button
			if (upNavigationEnabled == true && item.ItemId == HomeButtonId)
			{
				OnBackPressed();
				if (ParentActivityIntent != null)
				{
					NavUtils.NavigateUpFromSameTask(this);
				}

				return true;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}