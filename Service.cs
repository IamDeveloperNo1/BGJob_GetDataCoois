using Dapper;
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTDATABASE
{
    internal static class Service
    {
        //public static async Task record(string startOfMonth,string endOfMonth,  string plant, string condition)
        public static async Task record(string startOfMonth,string endOfMonth,string plant, string condition)
        {
            try
            {
                var resulte = await Repository.GetInfo(startOfMonth, endOfMonth,plant,condition);
                string order_list = string.Join(",", resulte.Select(x => x.OrderJob).Distinct());
                var resulteCheckConfirmQtyOffline = await RepositoryAFKO.GetInfo(order_list);



                Service.Log(Service.logFilePath, $@"Data In SAP {resulte.Count}");
                Service.Log(Service.logFilePath, resulteCheckConfirmQtyOffline.Count.ToString());


                Service.Log(Service.logFilePath, $@"Start Join Linq");
                var data_insert =
                    from t1 in resulte
                    join t2 in resulteCheckConfirmQtyOffline
                        on t1.OrderJob equals t2.order
                        into gj
                    from sub in gj.DefaultIfEmpty()
                    select new
                    {
                        t1,
                        mes_to_sap = sub.confirmQty
                    };
                Service.Log(Service.logFilePath, $@"Connnect Postgresql");
                using (IDbConnection dbConnection = new NpgsqlConnection(DestinationConfiguration.npgConnString))
                {
                    dbConnection.Open();
                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        //string sqlDelete = "delete from public.sap_caufv";
                        //dbConnection.Execute(sqlDelete);

                        var list = SplitList(data_insert.ToList(), 1000).ToList();
                        Service.Log(Service.logFilePath, $@"Data Count Insert is {list.Count}");
                        int success = 0;
                        Service.Log(Service.logFilePath, $@"Start Insert Process {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        list.ForEach(idx =>
                        {
                            string header = @"
							insert
									into
									public.sap_caufv
								(
									orderjob,
									""types"",
									enteredby,
									createon,
									changeby,
									changedate,
									plant,
									basicfinishdate,
									basicstartdate,
									scheduledrelesedate,
									scheduledfinish,
									scheduledstart,
									actualstartdate,
									actualorderfinishdate,
									actualfinishdate,
									actualreleasedate,
									totalorderquantity,
									material,
									materialbuf,
									mrpcontroller,
									basequantity,
									create_date,
									mes_to_sap)
								values
						";
                            string values = "";
                            int i = 0;
                            foreach (var value in idx)
                            {
                                values += $@"(
									'{value.t1.OrderJob}',
									'{value.t1.Type}',
									'{value.t1.EnteredBy}',
									'{value.t1.CreateOn}',
									'{value.t1.ChangeBy}',
									'{value.t1.ChangeDate}',
									'{value.t1.plant}',
									'{value.t1.BasicFinishDate}',
									'{value.t1.BasicStartDate}',
									'{value.t1.ScheduledReleseDate}',
									'{value.t1.ScheduledFinish}',
									'{value.t1.ScheduledStart}',
									'{value.t1.ActualStartDate}',
									'{value.t1.ActualOrderFinishDate}',
									'{value.t1.ActualFinishDate}',
									'{value.t1.ActualReleaseDate}',
									'{value.t1.TotalOrderQuantity}',
									'{value.t1.Material}',
									'{value.t1.MaterialBuf}',
									'{value.t1.MRPController}',
									'{value.t1.BaseQuantity}',
									CURRENT_TIMESTAMP,
									'{value.mes_to_sap}'
							)";

                                if ((i + 1) == idx.Count)
                                {
                                    values += ";";
                                }
                                else
                                {
                                    values += ",";
                                }
                                i++;
                            }
                            string statement = header + values;
                            int execute = dbConnection.Execute(statement);
                            if (execute == idx.Count)
                            {
                                success += idx.Count;
                                Service.Log(Service.logFilePath, $@"End Insert Process {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            }
                        });
                        transaction.Commit();
                        Service.Log(Service.logFilePath, "Insert Finish Process");
                    }
                }
            }catch(Exception ex)
            {
                Service.Log(Service.logFilePath,$@"Error On Service of While Loop Reading Data In Linq Error is {ex.Message}");
            }

        }
        public static IEnumerable<List<T>> SplitList<T>(List<T> list, int nSize = 30)
        {
            for (int i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
        public static string TimeStamp() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public static string TimeStampFilePathe() => DateTime.Now.ToString("yyyyMMddHHmmss");

        // เมธอดสำหรับบันทึกข้อความลงในไฟล์ log
        // เปลี่ยน Log() method
        public static void Log(string filePath, string message)
        {
            string logMessage = $"[{TimeStamp()}] {message}";
            try
            {
                File.AppendAllText(filePath, logMessage + Environment.NewLine);
                // ส่งข้อความไปที่ Standard Error แทน
                Console.Error.WriteLine(logMessage);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        //public static string logFilePath = Path.Combine("E:\\_ServiceHandsome\\CooisSapGetToMes", "Logs", $"LogCoois_{TimeStampFilePathe()}.log");
        public static string logFilePath = Path.Combine("D:\\MyTaining\\Background_Job\\SapCoois\\TESTDATABASE\\bin", "Logs", $"LogCoois_{TimeStampFilePathe()}.log");
    }
}
