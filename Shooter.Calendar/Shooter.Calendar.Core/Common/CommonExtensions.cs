using System;

namespace Shooter.Calendar.Core.Common
{
    public static class CommonExtensions
    {
        public static void ThrowIfNull(this object o, string paramName, string message = "")
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }
    }
}
