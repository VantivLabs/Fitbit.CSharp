using System.Web.Configuration;

namespace FitbitInvoicer.Models
{
    public class ConfigData
    {
        public static string GetClientId()
        {
            return WebConfigurationManager.AppSettings["clientId"];
        }

        public static string GetClientSecret()
        {
            return WebConfigurationManager.AppSettings["clientSecret"];
        }

        public static string GetAuthorizationRedirect()
        {
            return WebConfigurationManager.AppSettings["authorizationRedirect"];
        }

        public static string GetAuthorizationUrl()
        {
            return WebConfigurationManager.AppSettings["authorizeUrl"];
        }

        public static string GetTokenUrl()
        {
            return WebConfigurationManager.AppSettings["tokenUrl"];
        }        
    }
}
