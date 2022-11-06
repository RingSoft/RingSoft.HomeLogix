using RingSoft.HomeLogix.Mobile.ViewModels;
using Xamarin.Forms;

namespace RingSoft.HomeLogix.Mobile
{
    public static class MobileGlobals
    {
        public static MainViewModel MainViewModel { get; set; }

        public static string GetProperty(string name)
        {
            var result = string.Empty;

            if (Application.Current.Properties.ContainsKey(name))
            {
                result = Application.Current.Properties[name].ToString();
            }

            return result;
        }
    }
}
