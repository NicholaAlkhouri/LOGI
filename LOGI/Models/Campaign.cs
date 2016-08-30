using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Campaign
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string MobileImageUrl { get; set; }

        [Url]
        public string URL { get; set; }

        public int Location { get; set; }

        public bool IsOnline { get; set; }
    }
}