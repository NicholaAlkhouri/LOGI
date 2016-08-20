using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Services
{
    public class CountryService
    {
        private ApplicationDbContext DbContext;

        public CountryService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public MultiSelectList GetSelectListCountries(List<int> selected_value)
         {

             return new MultiSelectList((from s in DbContext.Countries.OrderBy(c => c.Name).ToList()
                                    select new
                                    {
                                        ID_Dipendente = s.ID,
                                        CName = s.Name + " (" + s.NameAr + ")"
                                    }),
                                 "ID_Dipendente",
                                 "CName",
                                 selected_value);
         }

        public SelectList GetSelectListFrontCountries(int? selected_value,string language ="en")
        {
            if(language == "en")
                return new SelectList((from s in DbContext.Countries.ToList()
                                            select new
                                            {
                                                ID_Dipendente = s.ID,
                                                CName = s.Name
                                            }),
                                    "ID_Dipendente",
                                    "CName",
                                    selected_value);
            else
                return new SelectList((from s in DbContext.Countries.ToList()
                                            select new
                                            {
                                                ID_Dipendente = s.ID,
                                                CName = s.NameAr
                                            }),
                                    "ID_Dipendente",
                                    "CName",
                                    selected_value);
        }
    }
}