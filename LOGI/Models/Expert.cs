using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Expert
    {
        public int ID { get; set; }

        public DateTime ApplyDate { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Expertise { get; set; }
        
        public string Education { get; set; }

        [Display(Name = "Political Affiliation")]
        public string PoliticalAffiliation { get; set; }

        [Display(Name = "Why do you want to join LOGI?")]
        public string WhyToJoin { get; set; }

        [Display(Name = "How do you envisage your contribution to LOGI?")]
        public string Message { get; set; }

        [Display(Name = "Resume")]
        public string ResumeURL { get; set; }
    }
}