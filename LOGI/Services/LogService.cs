using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class LogService
    {
        private ApplicationDbContext DbContext;

        public LogService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void  AddLog(Log log)
        {
            DbContext.Logs.Add(log);
            try{
                DbContext.SaveChanges();
            }catch(Exception ex)
            {

            }
        }

    }
}