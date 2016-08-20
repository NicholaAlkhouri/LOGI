using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Vacancy
    {
        public int ID { get; set; }

        public string JobNumber { get; set; }

        public DateTime DeadLine { get; set; }

        [Display(Name = "Vacancyt Title")]
        public string Title { get; set; }

        [Display(Name = "Role Summary")]
        public string RoleSummary { get; set; }

        [Display(Name = "Essential Responsibilities")]
        public string EssentialResponsibilities { get; set; }

        [Display(Name = "Project Deadline")]
        public string ProjectDeadline { get; set; }

        [Display(Name = "Desired Characteristics")]
        public string DesiredCharacteristics { get; set; }

        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; }

        public string Salary { get; set; }

        public string Outcome { get; set; }

        [Display(Name="Arabic Title")]
        public string TitleAr { get; set; }

        [Display(Name = "Role Summary AR")]
        public string RoleSummaryAr { get; set; }

        [Display(Name = "Essential Responsibilities AR")]
        public string EssentialResponsibilitiesAr { get; set; }

        [Display(Name = "Project Deadline AR")]
        public string ProjectDeadlineAr { get; set; }

        [Display(Name = "Desired Characteristics AR")]
        public string DesiredCharacteristicsAr { get; set; }

        [Display(Name = "Qualifications AR")]
        public string QualificationsAr { get; set; }

        [Display(Name = "Salary AR")]
        public string SalaryAr { get; set; }

        [Display(Name = "Outcome AR")]
        public string OutcomeAr { get; set; }

        public virtual ICollection<VacancyApplication> VacancyApplications { get; set; }
    }
}