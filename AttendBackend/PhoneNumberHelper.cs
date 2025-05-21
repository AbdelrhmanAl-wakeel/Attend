namespace AttendBackend
{
    public static class PhoneNumberHelper
    {
        public static string Normalize(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "";

            phone = phone.Trim().Replace(" ", "").Replace("-", "");

            if (phone.StartsWith("+966"))
                phone = phone.Substring(4);
            else if (phone.StartsWith("966"))
                phone = phone.Substring(3);

            if (!phone.StartsWith("0"))
                phone = "0" + phone;

            return phone;
        }
    }
}
