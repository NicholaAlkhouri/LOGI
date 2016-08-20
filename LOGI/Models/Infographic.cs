using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Infographic
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public string MetaDescription { get; set; }

        public string MetaDescriptionAr { get; set; }

        public int Order { get; set; }

        public string ImageURL { get; set; }

        public string ImageURLAr { get; set; }

        public string ThumbURL { get; set; }

        public string ThumbURLAr { get; set; }

        public string FriendlyURL { get; set; }

        public string FriendlyURLAr { get; set; }

        [UIHint("ForeignKey")]
        [Display(Name = "Category")]
        public int InfographicCategoryID { get; set; }

        public virtual InfographicCategory InfographicCategory { get; set; }
    }
}