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
    public class AdminExpertController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private VacancyService vacancyService;
        private SectionVariableService variabeService;
        #endregion

        #region Constructor
        public AdminExpertController()
        {
            vacancyService = new VacancyService(DBContext);
            variabeService = new SectionVariableService(DBContext);

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

        public ActionResult List()
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.EXPERTS;
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.EXPERTS;
            Expert view_model = vacancyService.GetExpert(id);
            return View(view_model);
        }

        public ActionResult Delete(int id)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.EXPERTS;

            if (vacancyService.DeleteExpert(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Details", new { id = id });
        }

        public JsonResult _ExpertsList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = vacancyService.GetExperts(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        public ActionResult SelectionPolicy()
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.SELECTIONPOLICY;

            SectionVariable donate_pdf = variabeService.GetVariableByKey("SelectionPolicyPDF");

            return View(donate_pdf);
        }

        [HttpPost]
        public ActionResult SelectionPolicy(IEnumerable<HttpPostedFileBase> PDF)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.SELECTIONPOLICY;

            if (ModelState.IsValid)
            {
                SectionVariable donate_pdf = variabeService.GetVariableByKey("SelectionPolicyPDF");

                if (PDF != null && PDF.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    if (donate_pdf != null)
                        fs.DeleteFile(donate_pdf.Value);

                    string file_url = fs.SaveFiles(PDF);

                    if (variabeService.AddUpdateVariable("SelectionPolicyPDF", file_url) > 0)
                    {
                        TempData["SuccessMessage"] = "File Updated Successfully";
                        return RedirectToAction("SelectionPolicy");
                    }
                    else
                        TempData["ErrorMessage"] = "File Failed to Updated";
                }

            }
            return View();
        }
        #endregion
    }
}