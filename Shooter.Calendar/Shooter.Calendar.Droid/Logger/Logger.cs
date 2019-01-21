using Shooter.Calendar.Core.Logger;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Droid.Logger
{
    public class Logger : ILogger
    {
        private readonly string tag;

        public Logger([NotNull] string tag)
        {
            this.tag = tag;
        }

        public void Error(string message)
        {
            Android.Util.Log.Error(tag, message);
        }

        public void Trace(string message)
        {
            Android.Util.Log.Debug(tag, message);
        }

        public void Warning(string message)
        {
            Android.Util.Log.Warn(tag, message);
        }
    }
}
