using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Timeline
    {
        public int ID { get; set; }

        [Required]
        public string Type { get; set; }

        public string Period { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PeriodAr { get; set; }

        public string TitleAr { get; set; }

        public string DescriptionAr { get; set; }

        public string ImageURL { get; set; }

        public int order { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}