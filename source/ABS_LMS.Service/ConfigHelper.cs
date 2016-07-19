using System.Reflection;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace ABS_LMS.Service
{
    public static class ConfigHelper
    {
        private static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
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
        
    }
}
