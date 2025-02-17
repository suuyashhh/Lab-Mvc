using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Objects
{
    public class DTOCasePaper
    {
        public Int64 TRN_NO { get; set; }
        public string PATIENT_NAME{ get; set; }
        public string GENDER { get; set; }
        public string CON_NUMBER { get; set; }
        public string ADDRESS { get; set; }
        public string DOCTOR_REF { get; set; }
        public string DATE { get; set; }
    }
}
