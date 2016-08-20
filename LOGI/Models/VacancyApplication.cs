using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class VacancyApplication
    {
        public int ID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Country { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

        public string ResumeURL { get; set; }

        public DateTime ApplyDate { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}