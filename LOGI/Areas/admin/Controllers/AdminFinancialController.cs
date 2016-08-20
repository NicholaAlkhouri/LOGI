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
    [Authorize(Roles = "Admin")]
    public class AdminFinancialController : Controller
    {
        #region Privates 
            private ApplicationDbContext _dbContext;
            private FinancialService financialService;
        #endregion

        #region Constructor
            public AdminFinancialController()
            {
                financialService = new FinancialService(DBContext);

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
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.FINANCIAL;
            return View();
        }

        public JsonResult _ReportsList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            DBContext.Configuration.LazyLoadingEnabled = false;
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = financialService.GetFinancialReports(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        public ActionResult New()
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.FINANCIAL;

            FinancialReportViewModel view_model = new FinancialReportViewModel();
            view_model.ExpencesEntries = new List<FinancialReportEntry>();
            view_model.FundingEntries = new List<FinancialReportEntry>();
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(FinancialReportViewModel report, IEnumerable<HttpPostedFileBase> Files)
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.FINANCIAL;

            if (ModelState.IsValid)
            {
                int new_id = financialService.AddReport(report, Files);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Report Added Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Report Failed To Add";
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.FINANCIAL;

            FinancialReportViewModel view_model = financialService.GetFinancialReport(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FinancialReportViewModel report, IEnumerable<HttpPostedFileBase> Files)
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.FINANCIAL;

            if (ModelState.IsValid)
            {
                int new_id = financialService.UpdateReport(report, Files);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Report Updated Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Report Failed To Update";
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (financialService.DeleteReport(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }
        #endregion
    }
}