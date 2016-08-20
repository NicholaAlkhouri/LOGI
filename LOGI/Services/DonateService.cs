using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class DonateService
    {
         private ApplicationDbContext DbContext;

         public DonateService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public DonateViewModel getDonateViewModel()
        {
            DonateViewModel view_model = new DonateViewModel();

            SectionVariable var = DbContext.SectionVariables.Where(v => v.Key == "DonatePDF").FirstOrDefault();

            if (var == null)
                return view_model;
            else
            {
                view_model.PdfURL = var.Value;
                return view_model;
            }
        }
    }
}