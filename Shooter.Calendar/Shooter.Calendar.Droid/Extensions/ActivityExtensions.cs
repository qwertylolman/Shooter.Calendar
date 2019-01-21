using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;

namespace Shooter.Calendar.Droid.Extensions
{
    public static class ActivityExtensions
    {
        public static bool CloseKeyboard(this Activity activity)
            => TryToChangeSoftKeyboardState(activity, (m, t) => m.HideSoftInputFromWindow(t, 0));

        public static bool OpenKeyboard(this Activity activity, View view)
            => TryToChangeSoftKeyboardState(activity, (m, t) => m.ShowSoftInput(view, ShowFlags.Implicit));

        private static bool TryToChangeSoftKeyboardState(Activity activity, Func<InputMethodManager, IBinder, bool> func)
        {
            var windowToken = activity?.CurrentFocus?.WindowToken;
            var systemService = activity?.GetSystemService(Context.InputMethodService);
            var inputMethodManager = systemService as InputMethodManager;

            if (windowToken == null || inputMethodManager == null)
            {
                return false;
            }

            return func(inputMethodManager, windowToken);
        }

        public static Point GetScreenSize(this Activity activity)
        {
            var size = new Point();
            activity.WindowManager.DefaultDisplay.GetSize(size);

            return size;
        }
    }
}
