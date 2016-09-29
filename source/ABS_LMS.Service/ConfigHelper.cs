using System.Reflection;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace ABS_LMS.Service
{
    public static class ConfigHelper
    {
        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }
        public static string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["smtpserver"]; }
        }
        public static string SmtpUsername
        {
            get { return ConfigurationManager.AppSettings["smtpusername"]; }
        }
        public static string SmtpPassword
        {
            get { return ConfigurationManager.AppSettings["smtppassword"]; }
        }
        public static string FromAddress
        {
            get { return ConfigurationManager.AppSettings["smtpfrom"]; }
        }
        public static string SmtpPort
        {
            get { return ConfigurationManager.AppSettings["smtpport"]; }
        }

        public static string SqlConnectionString
        {
            get { return ConfigurationManager.AppSettings[Environment + "ConnectionString"]; }
        }

        public static string ConnectionString
        {
            get
            {
                var entityString = new EntityConnectionStringBuilder
                {

                    Provider = "System.Data.SqlClient",
                    Metadata = "res://*/ABSLMSModel.csdl|res://*/ABSLMSModel.ssdl|res://*/ABSLMSModel.msl",
                    ProviderConnectionString = SqlConnectionString

                };
                return entityString.ConnectionString;

            }
        }
        
        public static string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static string PortalUrl => ConfigurationManager.AppSettings["PortalURL"];
        public static string HrEmail => ConfigurationManager.AppSettings["HrEmail"];

    }
}
