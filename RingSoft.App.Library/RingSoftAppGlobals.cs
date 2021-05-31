using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RingSoft.App.Library
{
    public class RingSoftAppGlobals
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AppTitle { get; set; }

        public static string AppVersion { get; set; }

        public static string AppCopyright { get; set; }

        public static double CalculateMonthsInTimeSpan(DateTime startDate, DateTime endDate)
        {
            var result = endDate.Subtract(startDate).Days / (365.25 / 12);
            return result;
        }
    }
}
