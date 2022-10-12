using System;
using System.Globalization;

namespace RingSoft.App.Library
{
    public static class ExtensionMethods
    {
        public static string GetArticle(this string text)
        {
            var result = "a";

            if (text.Length > 0)
            {
                var test = text.ToUpper();
                var firstChar = test[0];

                switch (firstChar)
                {
                    case 'A':
                    case 'E':
                    case 'I':
                    case 'O':
                    case 'U':
                        return "an";
                }
            }
            return result;
        }

        public static decimal RoundCurrency(this decimal value)
        {
            return Math.Round(value, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        }

        public static DateTime MinDate(this DateTime date)
        {
            return new DateTime(1980, 01, 01);
        }

        public static string PlatformText(this DbPlatforms platform)
        {
            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    return "Sqlite";
                case DbPlatforms.SqlServer:
                    return "Microsoft SQL Server";
                case DbPlatforms.MySql:
                    return "MySQL";
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }
    }
}
