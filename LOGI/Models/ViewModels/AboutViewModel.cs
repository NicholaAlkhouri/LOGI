using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class AboutViewModel
    {
        public List<TeamMember> TeamMembers { get; set; }

        public FinancialReport CurrentFinancialReport { get; set; }

        public List<FinancialReport> AllFinancialReports { get; set; }

        public string RegistrationPDF { get; set; }

        public string ByLawsPDF { get; set; }

        public string WhitePaper { get; set; }

        public string GiftAcceptance { get; set; }
    }
}