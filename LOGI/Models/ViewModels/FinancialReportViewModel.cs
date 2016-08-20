using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class FinancialReportViewModel
    {
        public int ID { get; set; }

        public string Year { get; set; }

        public string FileURL { get; set; }

        public double Funding { get; set; }

        public double Expences { get; set; }

        public List<FinancialReportEntry> ExpencesEntries { get; set; }

        public List<FinancialReportEntry> FundingEntries { get; set; }
    }
}