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
    }
}
