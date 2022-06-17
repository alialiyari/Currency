namespace Extensions
{
    public static class BoolExtensions
    {
        public static string ToHave(this bool value)
        {
            if (value == true) return "دارد";
            return "ندارد";
        }

        public static string ToHave(this bool? value)
        {
            if (value.HasValue == false) return "نا مشخص";
            return ToHave(value.Value);
        }
    }
}
