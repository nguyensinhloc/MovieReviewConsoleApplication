namespace MovieReviewConsoleApplication
{
    internal static class Extensions
    {
        // Extension method to check if an integer is between a range
        public static bool Between(this int value, int start, int end)
        {
            return value >= start && value <= end;
        }

    }
}
