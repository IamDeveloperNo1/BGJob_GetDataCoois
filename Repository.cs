using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTDATABASE
{
    internal class Repository
    {
        public static Task<List<ModelsAUFK>> GetInfo(string startOfMonth, string endOfMonth,string plant, string condition)
        {
            try
            {
                DestinationConfiguration config = new DestinationConfiguration();
                RfcDestinationManager.RegisterDestinationConfiguration(config);

                RfcDestination destination = RfcDestinationManager.GetDestination(DestinationConfiguration.appName);
                IRfcFunction readTable = destination.Repository.CreateFunction("BBP_RFC_READ_TABLE");

                readTable.SetValue("QUERY_TABLE", ModelsAUFK.satablename);
                readTable.SetValue("DELIMITER", ";");

                IRfcTable fieldTable = readTable.GetTable("FIELDS");

                string[] field_name = ModelsAUFK.safieldname.Split(",".ToCharArray());

                foreach (string field in field_name)
                {
                    fieldTable.Append();
                    fieldTable.SetValue("FIELDNAME",field);
                }
                IRfcTable optsTable = readTable.GetTable("OPTIONS");
                optsTable.Append();
                optsTable.SetValue("TEXT", $@"WERKS eq '{plant}' ");

                optsTable.Append();
                optsTable.SetValue("TEXT", $"AND GSTRP ge '{startOfMonth}' ");

                optsTable.Append();
                optsTable.SetValue("TEXT", $"AND GSTRP le '{endOfMonth}' ");

                if (condition != "1")
                {
                    optsTable.Append();
                    optsTable.SetValue("TEXT", $"AND PLNBEZ like '{condition}' ");
                }
                //optsTable.Append();
                //optsTable.SetValue("TEXT", "AND PLNBEZ CP 'B*' ");


                readTable.Invoke(destination);

                var rows = new List<ModelsAUFK>();

                IRfcTable result = readTable.GetTable("DATA");

                for(int i = 0; i<result.RowCount; i++)
                {
                    var value = result.GetString(0).Split(";".ToCharArray());
                    result.CurrentIndex = i;
                    ModelsAUFK models = new ModelsAUFK
                    {
                        OrderJob = value[0].Trim(),
                        Type = value[1].Trim(),
                        EnteredBy = value[2].Trim(),
                        CreateOn = value[3].Trim(),
                        ChangeBy = value[4].Trim(),
                        ChangeDate = value[5].Trim(),
                        plant = value[6].Trim(),
                        BasicFinishDate = value[7].Trim(),
                        BasicStartDate = value[8].Trim(),
                        ScheduledReleseDate = value[9].Trim(),
                        ScheduledFinish = value[10].Trim(),
                        ScheduledStart = value[11].Trim(),
                        ActualStartDate = value[12].Trim(),
                        ActualOrderFinishDate = value[13].Trim(),
                        ActualFinishDate = value[14].Trim(),
                        ActualReleaseDate = value[15].Trim(),
                        TotalOrderQuantity = value[16].Trim(),
                        Material = value[17].Trim(),
                        MaterialBuf = value[18].Trim(),
                        MRPController = value[19].Trim(),
                        BaseQuantity = value[20].Trim()
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
                return Task.FromResult<List<ModelsAUFK>>(null);
            }
        }

    }
}
/*
    ithtc,
    Thaier2022
*/