namespace LoLTournaments.Shared.Utilities
{

    public static class StringExtensions
    {
        public static int ConvertVersion(this string value)
        {
            return !string.IsNullOrEmpty(value)
                ? Convert.ToInt32(string.Join("", value.Where(char.IsDigit)))
                : int.MinValue;
        }
    }

}