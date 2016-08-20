using LOGI.Models;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class SubscriberController : BaseController
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private SubscriberService subscriberService;
        #endregion

        #region Constructor
        public SubscriberController()
        {
            subscriberService = new SubscriberService(DBContext);

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

        #region actions

        public JsonResult Subscribe(string email)
        {
            if (ModelState.IsValid)
            {
                if (subscriberService.Subscribe(email))
                    return Json(new { success = true });
                else
                    return Json(new { success = false});
            }
            return Json(new { success = false});
        }
        #endregion
    }
}