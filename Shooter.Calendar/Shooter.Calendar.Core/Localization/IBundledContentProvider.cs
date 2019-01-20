using System;
using System.IO;

namespace Shooter.Calendar.Core.Localization
{
    public interface IBundledContentProvider
    {
        string[] GetBundledFiles(string folder);
    }
}
