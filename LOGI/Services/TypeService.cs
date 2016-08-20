using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Services
{
    public class TypeService
    {
         private ApplicationDbContext DbContext;

         public TypeService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

         public SelectList GetSelectListTypes(int? selected_value)
         {

             return new SelectList((from s in DbContext.Types.OrderBy(t => t.Name).ToList()
                                    select new
                                    {
                                        ID_Dipendente = s.ID,
                                        TypeName = s.Name + " (" + s.NameAr + ")"
                                    }),
                                 "ID_Dipendente",
                                 "TypeName",
                                 selected_value);
         }

         public SelectList GetSelectListFrontTypes(int? selected_value,string language ="en")
         {
             if(language == "en")
                 return new SelectList((from s in DbContext.Types.ToList()
                                        select new
                                        {
                                            ID_Dipendente = s.ID,
                                            TypeName = s.Name
                                        }),
                                     "ID_Dipendente",
                                     "TypeName",
                                     selected_value);
             else
                 return new SelectList((from s in DbContext.Types.ToList()
                                        select new
                                        {
                                            ID_Dipendente = s.ID,
                                            TypeName = s.NameAr
                                        }),
                                     "ID_Dipendente",
                                     "TypeName",
                                     selected_value);
         }
    }
}