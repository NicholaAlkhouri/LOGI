using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Type
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; }

        public virtual ICollection<KeyIssue> KeyIssues { get; set; }
    }
}