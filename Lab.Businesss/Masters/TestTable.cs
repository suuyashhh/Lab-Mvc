using Lab.DALDapper.Implimantation.Masters;
using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Businesss.Masters
{
    public class TestTable
    {
        public static IMstTestTable _dalTestTabe;

        public Int64 TestCode { get; set; }
        public Int64 TrnNo { get; set; }
        public int SrNo { get; set; }
        public decimal Price { get; set; }
        public decimal LabPrice { get; set; }



    }
}
