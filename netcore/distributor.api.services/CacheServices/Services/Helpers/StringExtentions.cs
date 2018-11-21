namespace CacheServices.Services.Helpers
{
    public static class StringExtentions
    {
        public static string RemoveEscapeCharactersAndQuotes(this string input)
        {
            var result = input.Trim('"');
            result = result.Replace(@"\", @"");
            
            return result;
        }
    }
}