using System;
using System.IO;

namespace Localization
{
    public interface IBundledContentProvider
    {
        string[] GetBundledFiles(string folder);
    }
}
