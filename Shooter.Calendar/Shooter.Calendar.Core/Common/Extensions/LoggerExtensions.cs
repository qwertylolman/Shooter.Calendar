using System;
using MvvmCross;
using Shooter.Calendar.Core.Logger;

namespace Shooter.Calendar.Core.Common.Extensions
{
    public static class LoggerExtensions
    {
        private static ILogger logger;

        public static ILogger Logger
            => logger ?? (logger = Mvx.IoCProvider.Resolve<ILogger>());

        public static void LogToConsole(this Exception e)
        {
            Error(e);
        }

        public static void Error(Exception e)
        {
            Error(e.ToString());
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Trace(string message)
        {
            Logger.Trace(message);
        }

        public static void Warning(string message)
        {
            Logger.Warning(message);
        }
    }
}
