using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class InfographicViewModel
    {
        public int ID { get; set; }

        public int CategoryOrder { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string MetaDescription { get; set; }

        public string ImageURL { get; set; }

        public string ThumbURL { get; set; }

        public string FriendlyURL { get; set; }
    }
}