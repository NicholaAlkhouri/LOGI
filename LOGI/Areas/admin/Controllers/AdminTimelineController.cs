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
    public class AdminTimelineController : Controller
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private TimelineService timelineService;
        #endregion

        #region Constructor
        public AdminTimelineController()
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
        #region Actions
        public ActionResult New()
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;
            FillCountries(null);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Timeline timeline, IEnumerable<HttpPostedFileBase> Images, List<int> TCountries)
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;
            FillCountries(TCountries);

            if (ModelState.IsValid)
            {
                 if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                 {
                     TempData["ErrorMessage"] = "Item Failed To Add, Invalid Image File";
                 }
                 else
                 {
                     int new_id = timelineService.AddTimeline(timeline, Images, TCountries);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Timeline Item Added Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Timeline Item Failed To Add";
               }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;
            Timeline view_model = timelineService.GetTimeline(id);
            FillCountries(view_model.Countries == null ? null : view_model.Countries.Select(k => k.ID).ToList());
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(Timeline timeline, IEnumerable<HttpPostedFileBase> Images, List<int> TCountries)
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;
            FillCountries(TCountries);

            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                {
                    TempData["ErrorMessage"] = "Key Issue Failed To Update, Invalid Image File";
                }
                else{
                    int new_id = timelineService.UpdateTimeline(timeline, Images, TCountries);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Timeline Item Updated Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Timeline Item Failed To Update";
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;

            if (timelineService.DeleteTimeline(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult List()
        {
            ViewBag.MainNav = Navigator.Main.TIMELINE;
            return View();
        }

        public JsonResult _TimelinesList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = timelineService.GetTimelines(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }
        private void FillCountries(List<int> selected_value)
        {
            CountryService TS = new CountryService(DBContext);
            MultiSelectList result = TS.GetSelectListCountries(selected_value);
            ViewData["CountryID_Data"] = result;
        }
        #endregion
    }
}