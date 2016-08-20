using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class VacancyViewModel
    {
        public int ID { get; set; }

        public string JobNumber { get; set; }

        public string DeadLine { get; set; }

        public string Title { get; set; }

        public string TitleAr { get; set; }

        public string RoleSummary { get; set; }

        public string RoleSummaryAr { get; set; }
    }
}