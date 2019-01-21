using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Shooter.Calendar.Droid
{
    [Application]
    public class Application : MvxAppCompatApplication<MvxAppCompatSetup<App>, App>
    {
        public Application()
        {
        }

        public Application(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }
    }
}
