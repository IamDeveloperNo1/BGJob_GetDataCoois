//using SAP.Middleware.Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace TESTDATABASE
//{
//    internal class Demo
//    {
//        public static bool GetCostEstimateNumber(string mat_list, out List<ModelProdCostEstimateNo> rows)
//        {
//            try
//            {
//                string[] field_names = fieldEstimate.Split(",".ToCharArray());
//                DestinationConfiguration config = new DestinationConfiguration();
//                RfcDestinationManager.RegisterDestinationConfiguration(config);
//                RfcDestination destination = RfcDestinationManager.GetDestination(dest);
//                IRfcFunction readTable = destination.Repository.CreateFunction("RFC_READ_TABLE");
//                readTable.SetValue("QUERY_TABLE", tableEstimate);
//                readTable.SetValue("DELIMITER", ";");
//                IRfcTable fieldsTable = readTable.GetTable("FIELDS");
//                foreach (string field in field_names)
//                {
//                    fieldsTable.Append();
//                    fieldsTable.SetValue("FIELDNAME", field);
//                }
//                IRfcTable optsTable = readTable.GetTable("OPTIONS");
//                // สร้าง List ที่เก็บข้อมูล
//                List<string> matList = new List<string>();
//                string[] mat = mat_list.Split(",".ToCharArray());
//                bool isFirst = true;
//                // เพิ่มข้อมูลลงใน List
//                foreach (string field in mat)
//                {
//                    if (mat.Count() == 1)
//                    {
//                        matList.Add($"MATNR eq '{field}' ");
//                    }
//                    else if (mat.Count() > 1)
//                    {
//                        if (isFirst) // ใส่ "(" ตัวแรก
//                        {
//                            matList.Add($"(MATNR eq '{field}' ");
//                            isFirst = false;
//                        }
//                        else if (field == mat.Last()) // ใส่ ")" ตัวสุดท้าย note: ไม่งั้นข้อมูลไม่ตรง ลอง compare กับ SAP
//                        {
//                            matList.Add($"OR MATNR eq '{field}')");
//                        }
//                        else
//                        {
//                            matList.Add($"OR MATNR eq '{field}' ");
//                        }
//                    }
//                }
//                // สร้างตารางข้อมูลใน IRfcTable
//                foreach (string option in matList)
//                {
//                    optsTable.Append();
//                    optsTable.SetValue("TEXT", option);
//                }
//                optsTable.Append();
//                optsTable.SetValue("TEXT", $"AND BKLAS eq '7920'");
//                optsTable.Append();
//                optsTable.SetValue("TEXT", $"AND (BWKEY eq '9771' OR BWKEY eq '9772' ");
//                optsTable.Append();
//                optsTable.SetValue("TEXT", $"OR BWKEY eq '9773' OR BWKEY eq '9774')");
//                readTable.Invoke(destination);
//                rows = new List<ModelProdCostEstimateNo>();
//                IRfcTable result = readTable.GetTable("DATA");
//                for (int i = 0; i < result.RowCount; i++)
//                {
//                    var value = result.GetString(0).Split(";".ToCharArray());
//                    result.CurrentIndex = i;
//                    ModelProdCostEstimateNo model = new ModelProdCostEstimateNo()
//                    {
//                        ProdCostEstimateNo = value[0].Trim(),
//                        Cl = value[1].Trim(),
//                        Material = value[2].Trim(),
//                        Plant = value[3].Trim(),
//                        CostEstimateNo = value[4].Trim(),
//                        ValType = value[5].Trim(),
//                        Vty = value[6].Trim(),
//                        TotalStock = value[7].Trim(),
//                        TotalValue = value[8].Trim(),
//                        PriceControl = value[9].Trim(),
//                        MovevingPrice = value[10].Trim(),
//                        StandardPrice = value[11].Trim(),
//                        PriceUnit = value[12].Trim(),
//                        ValuationClass = value[13].Trim(),
//                        Year = value[14].Trim(),
//                        Period = value[15].Trim(),
//                    };
//                    rows.Add(model);
//                }
//                RfcSessionManager.EndContext(destination);
//                RfcDestinationManager.UnregisterDestinationConfiguration(config);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Program.logger.LogError(ex.Message);
//                rows = null;
//                return false;
//            }
//        }
//    }
//}


// 00 Model Code
//using System;
//using System.Collections.Generic;

//List<ModelDetailProd> matProd;
//ProductionLossRepository.GetDetailProd(year, month, out matProd);
//Program.logger.LogInfo("ProductionAmount : 00 - Model Code : " + matProd.Count);

//string matProd_list = string.Join(",", matProd.Select(item => item.MatCode).Distinct());
