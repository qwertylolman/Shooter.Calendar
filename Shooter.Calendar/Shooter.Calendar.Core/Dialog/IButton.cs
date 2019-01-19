namespace Shooter.Calendar.Core.Dialog
{
    public interface IButton
    {
        string Title { get; }

        ButtonType ButtonType { get; }
    }
}
