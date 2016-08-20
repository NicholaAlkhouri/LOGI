using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class FinancialReportEntry
    {
        public int ID { get; set; }

        public string Type { get; set; }

        public double Value { get; set; }

        public string Title { get; set; }

        public string TitleAr { get; set; }

        public string Color { get; set; }

        public virtual FinancialReport FinancialReport { get; set; }

    }
}