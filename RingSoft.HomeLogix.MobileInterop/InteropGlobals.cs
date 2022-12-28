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

            var client = new HttpClient();
            var text = client.GetStringAsync(url).Result;
            return text;
        }

    }
}
