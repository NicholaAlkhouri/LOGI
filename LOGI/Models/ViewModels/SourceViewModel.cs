using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class SourceViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; }

        //images sizes are 310x264 520x264 for slider  410x210 for search
        public string ImageURL { get; set; }

        public string LogoURL { get; set; }
    }
}