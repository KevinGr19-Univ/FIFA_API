namespace FIFA_API.Models.Utils
{
    public static class ModelUtils
    {
        public const string REGEX_CODEPOSTAL = "^([0-9]{2}|2[AB])[0-9]{3}$"; // A tester
        public const string REGEX_HEXACOLOR = "^[0-9A-Fa-f]{6}$"; // A tester
        public const string REGEX_TELEPHONE = "^0[1-9][0-9]{8}$";
        public const string REGEX_PASSWORD = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[-+!*$@%_])([-+!*$@%_\w]{12,20})$";
    }
}
