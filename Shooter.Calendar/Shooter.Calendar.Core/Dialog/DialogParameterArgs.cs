using System.Collections.Generic;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Dialog
{
    public class DialogParameterArgs
    {
        public DialogParameterArgs([NotNull] IEnumerable<IButton> buttons, string title, string text)
        {
            Buttons = buttons;
            Title = title;
            Text = text;
        }

        public IEnumerable<IButton> Buttons { get; }

        public string Title { get; }

        public string Text { get; }

        public static DialogParameterArgs Build(string title, string text, params IButton[] buttons)
            => new DialogParameterArgs(buttons, title, text);
    }
}
