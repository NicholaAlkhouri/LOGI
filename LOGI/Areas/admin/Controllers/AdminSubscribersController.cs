using LOGI.Areas.Admin.Models;
using LOGI.Models;
using LOGI.Models.ViewModels;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Areas.admin.Controllers
{
    public class AdminSubscribersController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private SubscriberService subsciberService;
        #endregion

        #region Constructor
        public AdminSubscribersController()
        {
            subsciberService = new SubscriberService(DBContext);

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
        #region Actions
        public ActionResult List(int CategoryId = 0)
        {
            ViewBag.MainNav = Navigator.Main.SUBSCRIBE;
            return View();
        }

        public JsonResult _SubscribersList(int draw, int start = 0, int length = 2)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = subsciberService.GetSubscribers(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        #endregion
    }
}