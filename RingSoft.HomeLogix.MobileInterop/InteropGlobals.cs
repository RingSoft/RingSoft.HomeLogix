using System.Net;

namespace RingSoft.HomeLogix.MobileInterop
{
    public static class InteropGlobals
    {
        public static string GetWebText(string fileName, string guid = "")
        {
            var url = "https://ringsoft.site/HomeLogixData/";
            if (!string.IsNullOrEmpty(guid))
            {
                url += guid + "/";
            }

            url += fileName;

            var client = new WebClient();
            var text = client.DownloadString(url);
            return text;
        }

    }
}
