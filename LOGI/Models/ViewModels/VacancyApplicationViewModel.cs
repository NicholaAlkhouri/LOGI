using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class VacancyApplicationViewModel
    {
        public int ID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }

        public string ResumeURL { get; set; }

        public string ApplyDate { get; set; }
    }
}