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
    public class AdminSourceController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private SourceService sourceService;
        #endregion

        #region Constructor
        public AdminSourceController()
        {
            sourceService = new SourceService(DBContext);

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
            ViewBag.MainNav = Navigator.Main.SOURCE;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Source source, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Logo)
        {
            ViewBag.MainNav = Navigator.Main.SOURCE;

            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                 {
                     TempData["ErrorMessage"] = "Source Failed To Add, Invalid Image File";
                 }
                else if (Logo != null && Logo.Count() > 0 && !ImageService.IsValid(Logo))
                {
                    TempData["ErrorMessage"] = "Source Failed To Add, Invalid Logo File";
                }
                 else
                {
                    int new_id = sourceService.AddSource(source, Images, Logo);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Source Added Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Source Failed To Add";
                }
                
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.SOURCE;
            Source view_model = sourceService.GetSource(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(Source source, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Logo)
        {
            ViewBag.MainNav = Navigator.Main.SOURCE;

            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                {
                    TempData["ErrorMessage"] = "Source Failed To Update, Invalid Image File";
                }
                else if (Logo != null && Logo.Count() > 0 && !ImageService.IsValid(Logo))
                {
                    TempData["ErrorMessage"] = "Source Failed To Add, Invalid Logo File";
                }
                else
                {
                    int new_id = sourceService.UpdateSource(source, Images, Logo);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Source Updated Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Source Failed To Update";
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (sourceService.DeleteSource(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult List()
        {
            ViewBag.MainNav = Navigator.Main.SOURCE;
            return View();
        }

        public JsonResult _SourcesList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = sourceService.GetSources(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteImage(int SourceId)
        {
            if (sourceService.DeleteSourceImage(SourceId))
                TempData["SuccessMessage"] = "Image deleted Successfully";      
            else
                TempData["ErrorMessage"] = "Image failed to delete";

            return RedirectToAction("Edit", new { id = SourceId });
        }

        [HttpPost]
        public ActionResult DeleteLogo(int SourceId)
        {
            if (sourceService.DeleteSourceLogo(SourceId))
                TempData["SuccessMessage"] = "Image deleted Successfully";
            else
                TempData["ErrorMessage"] = "Image failed to delete";

            return RedirectToAction("Edit", new { id = SourceId });
        }
        #endregion
    }
}