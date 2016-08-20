using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class FinancialReport
    {
        public int ID { get; set; }

        public string Year { get; set; }

        public string FileURL { get; set; }

        public double Funding { get; set; }

        public double Expences { get; set; }

        public virtual ICollection<FinancialReportEntry> FinancialReportEntries { get; set; }
    }
}