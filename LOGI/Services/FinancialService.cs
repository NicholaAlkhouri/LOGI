using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LOGI.Models.ViewModels;

namespace LOGI.Services
{
    public class FinancialService
    {
         private ApplicationDbContext DbContext;

         public FinancialService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public FinancialReport GetCurrentFinanicalReport()
        {
            FinancialReport fr = DbContext.FinancialReports.OrderByDescending(r => r.Year).Include(a=> a.FinancialReportEntries).FirstOrDefault();

            return fr;
        }

        public List<FinancialReport> GetAllFinancialReports()
        {
            List<FinancialReport> frs = DbContext.FinancialReports.OrderByDescending(r => r.Year).ToList();

            return frs;
        }

        public List<FinancialReport> GetFinancialReports(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<FinancialReport> result;
            if (order_by == "Year")
            {
                if (order_dir == "desc")
                    result = DbContext.FinancialReports.OrderByDescending(a => a.Year);
                else
                    result = DbContext.FinancialReports.OrderBy(a => a.Year);
            }
            else if (order_by == "Funding")
            {
                if (order_dir == "desc")
                    result = DbContext.FinancialReports.OrderByDescending(a => a.Funding);
                else
                    result = DbContext.FinancialReports.OrderBy(a => a.Funding);
            }
            else if (order_by == "Expences")
            {
                if (order_dir == "desc")
                    result = DbContext.FinancialReports.OrderByDescending(a => a.Expences);
                else
                    result = DbContext.FinancialReports.OrderBy(a => a.Expences);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.FinancialReports.OrderByDescending(a => a.ID);
                else
                    result = DbContext.FinancialReports.OrderBy(a => a.ID);
            }

            if (search_key != "")
                result = result.Where(cat => cat.Year.Contains(search_key) || cat.Year.Contains(search_key));


            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            return result.ToList();
        }

        public FinancialReportViewModel GetFinancialReport(int id)
        {
            FinancialReport FR = DbContext.FinancialReports.Where(fr => fr.ID == id).Include(fr=> fr.FinancialReportEntries).FirstOrDefault();
            FinancialReportViewModel result = new FinancialReportViewModel()
            {
                Expences = FR.Expences,
                FileURL = FR.FileURL,
                ExpencesEntries = FR.FinancialReportEntries.Where(a=> a.Type == "expences").ToList(),
                FundingEntries = FR.FinancialReportEntries.Where(a=> a.Type == "funding").ToList(),
                Funding = FR.Funding,
                ID = FR.ID,
                Year = FR.Year
            };
            return result;
        }

        public int UpdateReport(FinancialReportViewModel report, IEnumerable<HttpPostedFileBase> Files = null)
        {
            FinancialReport FR = DbContext.FinancialReports.Where(f => f.ID == report.ID).Include(a=> a.FinancialReportEntries).FirstOrDefault();

            if (FR == null)
                return -1;

            FR.Expences = report.ExpencesEntries.Sum(a=> a.Value);
            FR.Funding = report.FundingEntries.Sum(a => a.Value);
            FR.Year = report.Year;

            
            foreach (FinancialReportEntry e in FR.FinancialReportEntries.ToList())
                DbContext.FinancialReportEntries.Remove(e);

            FR.FinancialReportEntries = new List<FinancialReportEntry>();

            foreach(FinancialReportEntry fe in report.ExpencesEntries.Where(a=> a.Value != 0))
                FR.FinancialReportEntries.Add(fe);

            foreach (FinancialReportEntry fe in report.FundingEntries.Where(a => a.Value != 0))
                FR.FinancialReportEntries.Add(fe);
            //Update Entries
            if (Files.Where(f => f != null).Count() > 0)
            {
                FilesService fs = new FilesService();
                string file_url = fs.SaveFiles(Files);
                FR.FileURL = file_url;
            }

         

            DbContext.SaveChanges();
            return FR.ID;
        }

        public int AddReport(FinancialReportViewModel report, IEnumerable<HttpPostedFileBase> Files = null)
        {
            FinancialReport FR = new FinancialReport();

            FR.Expences = report.ExpencesEntries.Sum(a => a.Value);
            FR.Funding = report.FundingEntries.Sum(a => a.Value);
            FR.Year = report.Year;

            FR.FinancialReportEntries = new List<FinancialReportEntry>();
          
            foreach (FinancialReportEntry fe in report.ExpencesEntries)
                FR.FinancialReportEntries.Add(fe);

            foreach (FinancialReportEntry fe in report.FundingEntries)
                FR.FinancialReportEntries.Add(fe);
            //Update Entries

            if (Files.Where(f => f != null).Count() > 0)
            {
                FilesService fs = new FilesService();
                string file_url = fs.SaveFiles(Files);
                FR.FileURL = file_url;
            }

            DbContext.FinancialReports.Add(FR);
            DbContext.SaveChanges();
            return FR.ID;
        }

        public bool DeleteReport(int id)
        {
            FinancialReport FR = DbContext.FinancialReports.Where(f => f.ID == id).Include(f=> f.FinancialReportEntries).FirstOrDefault();

            if (FR == null)
                return false;
            try
            {
                if(FR.FinancialReportEntries != null)
                { 
                    foreach (FinancialReportEntry fre in FR.FinancialReportEntries.ToList())
                        DbContext.FinancialReportEntries.Remove(fre);

                    FR.FinancialReportEntries = new List<FinancialReportEntry>();
                    DbContext.SaveChanges();
                }
               

                DbContext.FinancialReports.Remove(FR);

                DbContext.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}