using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ATagsCounter.Models;

namespace ATagsCounter.Core;

public static class FileParser
{
    public static ReadOnlyCollection<UrlItem> ParseUriFromTxt(string path)
    {
        var sr = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read));
        var res = new List<UrlItem>();
        
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;

            res.Add(new UrlItem(new Uri(line)));
        }
        
        return res.AsReadOnly();
    }
}