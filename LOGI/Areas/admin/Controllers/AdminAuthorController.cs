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
    public class AdminAuthorController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private AuthorService authorService;
        #endregion

        #region Constructor
        public AdminAuthorController()
        {
            authorService = new AuthorService(DBContext);

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
            ViewBag.MainNav = Navigator.Main.AUTHOR;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Author author)
        {
            ViewBag.MainNav = Navigator.Main.AUTHOR;

            if (ModelState.IsValid)
            {
                int new_id = authorService.AddAuthor(author);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Author Added Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Author Failed To Add";
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.AUTHOR;
            Author view_model = authorService.GetAuthor(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(Author author)
        {
            ViewBag.MainNav = Navigator.Main.AUTHOR;

            if (ModelState.IsValid)
            {
                int new_id = authorService.UpdateAuthor(author);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Author Updated Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Author Failed To Update";
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (authorService.DeleteAuthor(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult List(int CategoryId = 0)
        {
            ViewBag.MainNav = Navigator.Main.AUTHOR;
            return View();
        }

        public JsonResult _AuthorsList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = authorService.GetAuthors(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }
        #endregion
    }
}