﻿using System;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RingSoft.App.Library
{
    public class RingSoftAppGlobals
    {
        public static string AppTitle { get; set; }

        public static string AppVersion { get; set; }

        public static string AppCopyright { get; set; }

        public static double CalculateMonthsInTimeSpan(DateTime startDate, DateTime endDate)
        {
            return startDate.Subtract(endDate).Days / (365.25 / 12);
        }
    }
}
