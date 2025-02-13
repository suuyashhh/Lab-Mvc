using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Objects
{
    public class DTOCasePaper
    {
        public Int64 PATIENT_ID { get; set; }
        public string PATIENT_NAME{ get; set; }
        public string GENDER { get; set; }
        public string CONTACT { get; set; }
        public string ADDRESS { get; set; }
        public string DOCTOR { get; set; }
        public string DATE { get; set; }
    }
}
