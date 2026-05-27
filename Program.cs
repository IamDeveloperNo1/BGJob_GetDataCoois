using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global = System.Globalization;
namespace TESTDATABASE
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //if (args.Length < 4)
                //{
                //    Service.Log(Service.logFilePath, $@"Missing arguments. Length of Args is {args.Length}");
                //    return;
                //}
                string dataString = String.Join(" ", args);

                string[] arr = dataString.Split(',');

                Service.Log(Service.logFilePath, "This is the End "+arr[0].ToString());
                //string plant = "9771";
                //string condition = "B%";
                //string startOfMonth = "20260521";
                //string endOfMonth = "20260521";
                string plant = arr[0];
                string condition = arr[1];
                string startOfMonth = arr[2];
                string endOfMonth = arr[3];

                Service.Log(Service.logFilePath, plant);
                Service.Log(Service.logFilePath, condition);
                Service.Log(Service.logFilePath, startOfMonth);
                Service.Log(Service.logFilePath, endOfMonth);
                var now = DateTime.Now;

                //string startOfMonth = new DateTime(now.Year, now.Month, 1).ToString("yyyyMMdd", new Global.CultureInfo("en-US"));
                //string endOfMonth = new DateTime(now.Year,now.Month,DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyyMMdd", new Global.CultureInfo("en-US"));

                await Service.record(startOfMonth, endOfMonth, plant, condition);
            }
            catch(Exception ex)
            {
                Service.Log(Service.logFilePath, $@"Error Program {ex.Message}");

            }
            
            
        }
    }
}
