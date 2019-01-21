namespace Shooter.Calendar.Core.Logger
{
    public interface ILogger
    {
        void Error(string message);

        void Trace(string message);

        void Warning(string message);
    }
}
