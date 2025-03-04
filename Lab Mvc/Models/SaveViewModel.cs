using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_Mvc.Models
{
    public class SaveViewModel
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public string DocNo { get; set; }
        public string TrnNo { get; set; }
    }
}