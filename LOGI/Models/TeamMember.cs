using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class TeamMember
    {
        public int ID { get; set; }

        [Required]
        public string Type { get; set; }

        public string ImageURL { get; set; }

        public bool IsOnline { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string  FullName { get; set; }

        [Display(Name = "Expertise")]
        public string Expertise { get; set; }

        [Display(Name = "Current Role")]
        public string CurrentRole { get; set; }

        [Display(Name = "Career")]
        public string Career { get; set; }

        [Display(Name = "Education")]
        public string Education { get; set; }

        [Display(Name = "Arabic Full Name")]
        public string FullNameAr { get; set; }

        [Display(Name = "Arabic Expertise")]
        public string ExpertiseAr { get; set; }

        [Display(Name = "Arabic Current Role")]
        public string CurrentRoleAr { get; set; }

        [Display(Name = "Arabic Career")]
        public string CareerAr { get; set; }

        [Display(Name = "Arabic Education")]
        public string EducationAr { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Twitter")]
        [Url]
        public string Twitter { get; set; }

        [Display(Name = "Linkedin")]
        [Url]
        public string Linkedin { get; set; }
    }
}