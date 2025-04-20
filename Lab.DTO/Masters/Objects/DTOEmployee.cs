using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Objects
{
    public class DTOEmployee
    {
        public Int64 EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_CONTACT { get; set; }
        public string EMP_PASSWORD { get; set; }
        public string DELETE_REASON { get; set; }
        public string COM_ID { get; set; }
    }
}
