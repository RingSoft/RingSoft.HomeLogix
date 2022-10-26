using System;
using System.Globalization;
using System.Text;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.App.Library
{
    public static class ExtensionMethods
    {
        private const string AuthKey = "028e17f2-9045-402a-87ee-d5a9a013";
        private const string PasswordKey = "87c5ae03-5c6f-42bd-9bb7-cd0af84e2cdd";
        private const string DatabaseKey = "65a77c2b-e179-4540-9aa0-15d98aee3a62";
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

        public static string EncryptPassword(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            return EncryptString(text, PasswordKey);
        }

        public static bool IsValidPassword(this string password, string encryptedPassword)
        {
            if (password.IsNullOrEmpty() && !encryptedPassword.IsNullOrEmpty())
            {
                return false;
            }

            var decryptedPassword = DecryptString(encryptedPassword, PasswordKey);

            return decryptedPassword == password;
        }

        public static string EncryptDatabasePassword(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            var test = Guid.NewGuid().ToString();

            return EncryptString(text, DatabaseKey);
        }

        public static string DecryptDatabasePassword(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            return DecryptString(text, DatabaseKey);

        }

        private static string EncryptString(string text, string key)
        {
            byte[] key256 = new byte[32];
            for (int i = 0; i < 32; i++)
                key256[i] = Convert.ToByte(i % 256);

            byte[] nonSecretOrg = Encoding.UTF8.GetBytes(AuthKey);

            var result = AuthenticatedEncryption.Encryption.Encrypt(text, key256, nonSecretOrg);
            return result;
        }

        public static string Decrypt(this string text)
        {
            if (text.IsNullOrEmpty())
            {
                return text;
            }

            return DecryptString(text, AuthKey);
        }

        private static string DecryptString(string text, string key)
        {
            byte[] key256 = new byte[32];
            for (int i = 0; i < 32; i++)
                key256[i] = Convert.ToByte(i % 256);

            byte[] nonSecretOrg = Encoding.UTF8.GetBytes(AuthKey);

            var result = AuthenticatedEncryption.Encryption.Decrypt(text, key256, nonSecretOrg);

            return result;
        }
    }
}
