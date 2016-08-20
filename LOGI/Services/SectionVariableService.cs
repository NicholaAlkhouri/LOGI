using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class SectionVariableService
    {
        private ApplicationDbContext DbContext;

        public SectionVariableService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public int AddUpdateVariable(string key,string  value)
        {
            SectionVariable var = DbContext.SectionVariables.Where(k => k.Key == key).FirstOrDefault();

            if (var == null)
            {
                var = new SectionVariable() { Key = key, Value = value };
                DbContext.SectionVariables.Add(var);
            }else
            {
                var.Value = value;
            }

            try
            {
                DbContext.SaveChanges();
                return var.ID;
            }catch(Exception ex)
            {
                return -1;
            }
        }

        public SectionVariable GetVariableByKey(string key)
        {
            return DbContext.SectionVariables.Where(k => k.Key == key).FirstOrDefault();
        }
    }
}