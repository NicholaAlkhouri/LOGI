using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class AboutService
    {
        private ApplicationDbContext DbContext;

        public AboutService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public AboutViewModel GetAboutViewModel(string language = "en" )
        {
            AboutViewModel view_model = new AboutViewModel();
            FinancialService FS = new FinancialService(DbContext);

            view_model.TeamMembers = DbContext.TeamMembers.Where(t => t.IsOnline == true).ToList();
            
            view_model.CurrentFinancialReport = FS.GetCurrentFinanicalReport();

            view_model.AllFinancialReports = FS.GetAllFinancialReports();

            SectionVariable Registration =  DbContext.SectionVariables.Where(v => v.Key == "RegistrationPDF").FirstOrDefault();
            view_model.RegistrationPDF = Registration == null ? "": Registration.Value;

            SectionVariable bylaws = DbContext.SectionVariables.Where(v => v.Key == "ByLawsPDF").FirstOrDefault();
            view_model.ByLawsPDF = bylaws == null ? "":  bylaws.Value;

            if (language == "ar")
            {
                SectionVariable whitepaper = DbContext.SectionVariables.Where(v => v.Key == "WhitePaperPDFAr").FirstOrDefault();
                view_model.WhitePaper = whitepaper == null ? "" : whitepaper.Value;
            }
            else
            {
                SectionVariable whitepaper = DbContext.SectionVariables.Where(v => v.Key == "WhitePaperPDF").FirstOrDefault();
                view_model.WhitePaper = whitepaper == null ? "" : whitepaper.Value;
            }
            

            SectionVariable giftacceptance = DbContext.SectionVariables.Where(v => v.Key == "GiftAcceptancePolicy").FirstOrDefault();
            view_model.GiftAcceptance = giftacceptance == null ? "" : giftacceptance.Value;

            return view_model;
        }
    }
}