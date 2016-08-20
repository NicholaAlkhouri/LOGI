using LOGI.Models;
using LOGI.Models.ViewModels;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class TimelineController : BaseController
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private TimelineService timelineService;
        #endregion

        #region Constructor
        public TimelineController()
        {
            timelineService = new TimelineService(DBContext);

        }
        #endregion

        #region Properties
        public ApplicationDbContext DBContext
        {
            get
            {
                return (ApplicationDbContext)ContextManager.GetDBContext("DBContext");
            }
            private set
            {
                _dbContext = value;
            }
        }
        #endregion
        
        public ActionResult Index(int? countryId = null, string category = "")
        {
            TimelinesViewModel view_model = timelineService.GetTimelinesViewModel(countryId, category);
            return View(view_model);
        }
    }
}