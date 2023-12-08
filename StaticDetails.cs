namespace HolbertonExample
{
    public static class StaticDetails
    {
        public static Dictionary<string, string> userCredentials = new Dictionary<string, string>
        {
            { "Alice", "p@ssw0rd1" },
            { "Bob", "securePwd123" },
            { "Eve", "mySecretPwd!" }
        };

        public static Dictionary<string, string> userRoles = new Dictionary<string, string>
        {
            { "Alice", "admin" },
            { "Bob", "user" },
            { "Eve", "guest" }
        };
    }
}
