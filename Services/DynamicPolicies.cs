

namespace WebApp.Services
{

    public static class DynamicPolicies
    {
        /*  used to always allow access to Admin group members and others if explicitly defined  */
        public  const string DynamicAdmin = "DynamicAdmin";
        
        /*  used to always allow access if explicitly defined  */
        public  const string Dynamic = "Dynamic";

        public static bool Exists(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return value == DynamicAdmin || value == Dynamic;
        }
    }
}