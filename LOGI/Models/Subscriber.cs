using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Subscriber
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public DateTime SubscriptionDate { get; set; }
    }
}