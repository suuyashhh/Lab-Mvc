﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Objects
{
    public class DTOTest
    {
        public Int64 TRN_NO { get; set; }
        public Int64  TEST_CODE { get; set; }
        public string  TEST_NAME { get; set; }
        public decimal  PRICE{ get; set; }
        public decimal  LAB_PRICE{ get; set; }
        public int SR_NO { get; set; }
        public int STATUS_CODE{ get; set; }
        public string SHORT_TRN_NO { get; set; }
        public string DELETE_REASON { get; set; }
        public string COM_ID { get; set; }
        public string CRT_BY { get; set; }
    }
}
