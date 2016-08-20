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
    public class AdminVacancyController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private VacancyService vacancyService;
        #endregion

        #region Constructor
        public AdminVacancyController()
        {
            vacancyService = new VacancyService(DBContext);

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
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Vacancy vacancy)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;

            if (ModelState.IsValid)
            {

                int new_id = vacancyService.AddVacancy(vacancy);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Vacancy Added Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Vacancy Failed To Add";
               
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;
            Vacancy view_model = vacancyService.GetVacancy(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(Vacancy vacancy)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;

            if (ModelState.IsValid)
            {
                int new_id = vacancyService.UpdateVacancy(vacancy);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Vacancy Updated Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Vacancy Failed To Update";
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;

            if (vacancyService.DeleteVacancy(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult List()
        {
            ViewBag.MainNav = Navigator.Main.JOINLOGI;
            ViewBag.SubNav = Navigator.Sub.VACANCIES;
            return View();
        }

        public JsonResult _VacanciesList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = vacancyService.GetVacancies(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        public JsonResult _ApplicationList(int draw, int start = 0, int length = 2, int VacancyId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = vacancyService.GetVacancyApplication(out total_count, start, length, search, order_by, order_dir, VacancyId);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }
        #endregion
    }
}