using RingSoft.HomeLogix.Mobile.ViewModels;

namespace RingSoft.HomeLogix.Mobile
{
    public static class MobileGlobals
    {
        public static MainViewModel MainViewModel { get; set; }

        public static string GetProperty(string name)
        {
            var result = string.Empty;
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, $"{name}.txt");
            if (File.Exists(fullPath))
            {
                result = File.ReadAllText(fullPath);
            }

            return result;
        }

        public static bool PropertyExists(string name)
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, $"{name}.txt");
            return File.Exists(fullPath);
        }

        public static void SetProperty(string name, string value)
        {
            var path = FileSystem.Current.AppDataDirectory;
            var fullPath = Path.Combine(path, $"{name}.txt");
            File.WriteAllText(fullPath, value);
        }
    }
}
