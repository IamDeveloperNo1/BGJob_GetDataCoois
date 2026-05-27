using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTDATABASE
{
    internal class DestinationConfiguration : IDestinationConfiguration
    {
        public static string appName = "xxxxx";
        public static string npgConnString = "Host=xxxxx;Port=xxxx;Username=xxxx;Password=xxxxxx;Database=xxxxx";
        
        public bool ChangeEventsSupported()
        {
            return true;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destionationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

            try
            {
                parms.Add(RfcConfigParameters.AppServerHost, "xxxxx");
                parms.Add(RfcConfigParameters.SystemNumber, "xxx");
                parms.Add(RfcConfigParameters.SystemID, "xxxx");
                parms.Add(RfcConfigParameters.User, "xxxx");
                parms.Add(RfcConfigParameters.Password, "xxxxx");
                parms.Add(RfcConfigParameters.Client, "xxxxx");
                parms.Add(RfcConfigParameters.Language, "EN");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return parms;
        }
    }
}
