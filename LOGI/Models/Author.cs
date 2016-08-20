using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        public string  FullName { get; set; }

        [Display(Name = "Arabic Full Name")]
        public string FullNameAr { get; set; }

        public virtual ICollection<KeyIssue> KeyIssues { get; set; }
    }
}