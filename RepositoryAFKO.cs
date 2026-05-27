using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTDATABASE
{
    internal class RepositoryAFKO
    {
        public static Task<List<ModelsAFKO>> GetInfo(string order)
        {
            try
            {
                Console.WriteLine("Function Check Confirm Start...");

                DestinationConfiguration config = new DestinationConfiguration();
                RfcDestinationManager.RegisterDestinationConfiguration(config);

                RfcDestination destination = RfcDestinationManager.GetDestination(DestinationConfiguration.appName);
                IRfcFunction readTable = destination.Repository.CreateFunction("BBP_RFC_READ_TABLE");

                readTable.SetValue("QUERY_TABLE", ModelsAFKO.satablename);
                readTable.SetValue("DELIMITER", ";");

                IRfcTable fieldTable = readTable.GetTable("FIELDS");

                string[] field_name = ModelsAFKO.safieldname.Split(",".ToCharArray());

                foreach (string field in field_name)
                {
                    fieldTable.Append();
                    fieldTable.SetValue("FIELDNAME",field);
                }
                IRfcTable optsTable = readTable.GetTable("OPTIONS");

                string[] orderList = order.Split(",".ToCharArray());
                List<string> orderAddCondition = new List<string>();
                bool isFirst = true;

                foreach(string idx in orderList)
                {
                    if (orderList.Length==1)
                    {
                        orderAddCondition.Add($@"AUFNR eq '{idx}' ");
                    }else if (orderList.Length > 1)
                    {
                        if (isFirst)
                        {
                            orderAddCondition.Add($@"(AUFNR eq '{idx}' ");
                            isFirst = false;
                        }else if (idx == orderList.Last())
                        {
                            orderAddCondition.Add($@"OR AUFNR eq '{idx}' )");
                        }
                        else
                        {
                            orderAddCondition.Add($@"OR AUFNR eq '{idx}' ");
                        }
                    }
                }

                foreach(string idx in orderAddCondition)
                {
                    optsTable.Append();
                    optsTable.SetValue("TEXT", idx);
                }

                //optsTable.Append();
                //optsTable.SetValue("TEXT", $@"AUFNR eq '{order}' ");

                readTable.Invoke(destination);

                var rows = new List<ModelsAFKO>();

                IRfcTable result = readTable.GetTable("DATA");

                for(int i = 0; i<result.RowCount; i++)
                {
                    var value = result.GetString(0).Split(";".ToCharArray());
                    result.CurrentIndex = i;
                    ModelsAFKO models = new ModelsAFKO
                    {
                        confirmQty = value[0].Trim(),
                        order = value[1].Trim()
                    };
                    rows.Add(models);
                }

                RfcSessionManager.EndContext(destination);
                RfcDestinationManager.UnregisterDestinationConfiguration(config);

                return Task.FromResult(rows);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult<List<ModelsAFKO>>(null);
            }
        }

    }
}
/*
    ithtc,
    Thaier2022
*/