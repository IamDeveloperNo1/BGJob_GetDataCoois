using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace TESTDATABASE
{
    internal class ModelsAUFK
    {
        public static string satablename = "CAUFV";
        public static string safieldname = "AUFNR,AUART,ERNAM,ERDAT,AENAM,AEDAT,WERKS,GLTRP,GSTRP,FTRMS,GLTRS,GSTRS,GSTRI,GETRI,GLTRI,FTRMI,GAMNG,PLNBEZ,STLBEZ,DISPO,BMENGE";

        public string OrderJob { get; set; }
        public string Type { get; set; }
        public string EnteredBy { get; set; }
        public string CreateOn { get; set; }
        public string ChangeBy { get; set; }
        public string ChangeDate { get; set; }
        public string plant { get; set; }
        public string BasicFinishDate { get; set; }
        public string BasicStartDate { get; set; }
        public string ScheduledReleseDate { get; set; }
        public string ScheduledFinish { get; set; }
        public string ScheduledStart { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualOrderFinishDate { get; set; }
        public string ActualFinishDate { get; set; }
        public string ActualReleaseDate { get; set; }
        public string TotalOrderQuantity { get; set; }
        public string Material { get; set; }
        public string MaterialBuf { get; set; }
        public string MRPController { get; set; }
        public string BaseQuantity { get; set; }
    }
}
