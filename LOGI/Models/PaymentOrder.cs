using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class PaymentOrder
    {
        public int ID { get; set; }

        public DateTime OrderDate { get; set; }

        public string Amount { get; set; }

        public String Currency { get; set; }
    }
}