using LOGI.Areas.admin.Models.ViewModels;
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
    public class AdminInfographicController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private InfographicService infographicService;
        #endregion

        #region Constructor
        public AdminInfographicController()
        {
            infographicService = new InfographicService(DBContext);

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

        // GET: admin/AdminInfographic
        public ActionResult Categories()
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOCATEGORY;
            return View();
        }

        public ActionResult NewCategory()
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOCATEGORY;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewCategory(InfographicCategory category)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOCATEGORY;

            if (ModelState.IsValid)
            {

                int new_id = infographicService.AddCategory(category);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Category Added Successfully";
                    return RedirectToAction("EditCategory", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Category Failed To Add";

            }
            return View();
        }

        public ActionResult EditCategory(int id)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOCATEGORY;

            InfographicCategory view_model = infographicService.GetCategory(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(InfographicCategory category)
        {
            ViewBag.MainNav = Navigator.Main.SOURCE;

            if (ModelState.IsValid)
            {
                int new_id = infographicService.UpdateCategory(category);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Category Updated Successfully";
                    return RedirectToAction("EditCategory", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Category Failed To Update";
                
            }
            return View();
        }

        public ActionResult DeleteCategory(int id)
        {
            if (infographicService.DeleteCategory(id))
            {
                TempData["ErrorMessage"] = "Category Deleted Successfuly";
                return RedirectToAction("Categories");
            }
            else
            {
                TempData["ErrorMessage"] = "Category Failed To Delete";
                return RedirectToAction("EditCategory", new { id = id });
            }
        }


        public ActionResult Infographics(int InfographicCategoryID = 0)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOGRAPHIC;

            AdminInfographicCatList view_model = new AdminInfographicCatList() { InfographicCategoryID = InfographicCategoryID };
            FillCategories(InfographicCategoryID);

            return View(view_model);
        }

        public ActionResult NewInfographic()
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOGRAPHIC;

            FillCategories(null);

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewInfographic(Infographic infographic, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Thumbs, IEnumerable<HttpPostedFileBase> ImagesAr, IEnumerable<HttpPostedFileBase> ThumbsAr)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOGRAPHIC;

            FillCategories(infographic.InfographicCategoryID);

            if (ModelState.IsValid)
            {
                string cleaned_url  = URLValidator.CleanURL(infographic.FriendlyURL);
                string cleaned_url_ar  = URLValidator.CleanURL(infographic.FriendlyURLAr);
                
                if(infographicService.IsURLExist(cleaned_url, -1))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Friendly URL Already Exist";
                }
                else if(infographicService.IsURLExist(cleaned_url_ar, -1))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Arabic Friendly URL Already Exist";
                }
                else if (!URLValidator.IsValidURLPart(infographic.FriendlyURL))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Friendly URL Not Valid";
                }
                else if (!URLValidator.IsValidURLPart(infographic.FriendlyURLAr))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Arabic Friendly URL Not Valid";
                }
                else if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Image File";
                }
                else if (Thumbs != null && Thumbs.Count() > 0 && !ImageService.IsValid(Thumbs))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Thumb File";
                }
                else if (ImagesAr != null && ImagesAr.Count() > 0 && !ImageService.IsValid(ImagesAr))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Arabic Image File";
                }
                else if (ThumbsAr != null && ThumbsAr.Count() > 0 && !ImageService.IsValid(ThumbsAr))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Arabic Thumbs File";
                }
                else
                {
                    int new_id = infographicService.AddInfographic(infographic, Images, Thumbs, ImagesAr, ThumbsAr);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Infographic Added Successfully";
                        return RedirectToAction("EditInfographic", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Infographic Failed To Add";
                }

            }
            return View();
        }

        public ActionResult EditInfographic(int id)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOGRAPHIC;

            Infographic view_model = infographicService.GetInfographic(id);

            FillCategories(view_model.InfographicCategoryID);

            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditInfographic(Infographic infographic, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Thumbs, IEnumerable<HttpPostedFileBase> ImagesAr, IEnumerable<HttpPostedFileBase> ThumbsAr)
        {
            ViewBag.MainNav = Navigator.Main.INPHOGRAPHIC;
            ViewBag.SubNav = Navigator.Sub.INFOGRAPHIC;

            FillCategories(infographic.InfographicCategoryID);

            if (ModelState.IsValid)
            {
                 string cleaned_url  = URLValidator.CleanURL(infographic.FriendlyURL);
                string cleaned_url_ar  = URLValidator.CleanURL(infographic.FriendlyURLAr);
                
                if(infographicService.IsURLExist(cleaned_url, infographic.ID))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Friendly URL Already Exist";
                }
                else if (infographicService.IsURLExist(cleaned_url_ar, infographic.ID))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Arabic Friendly URL Already Exist";
                }
                else if (!URLValidator.IsValidURLPart(infographic.FriendlyURL))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Friendly URL Not Valid";
                }
                else if (!URLValidator.IsValidURLPart(infographic.FriendlyURLAr))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Arabic Friendly URL Not Valid";
                }
                else if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Image File";
                }
                else if (Thumbs != null && Thumbs.Count() > 0 && !ImageService.IsValid(Thumbs))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Thumb File";
                }
                else if (ImagesAr != null && ImagesAr.Count() > 0 && !ImageService.IsValid(ImagesAr))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Arabic Image File";
                }
                else if (ThumbsAr != null && ThumbsAr.Count() > 0 && !ImageService.IsValid(ThumbsAr))
                {
                    TempData["ErrorMessage"] = "Infographic Failed To Add, Invalid Arabic Thumbs File";
                }
                else
                {
                    infographic.FriendlyURL = cleaned_url;
                    int new_id = infographicService.UpdateInfographic(infographic, Images, Thumbs, ImagesAr, ThumbsAr);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "Infographic Updated Successfully";
                        return RedirectToAction("EditInfographic", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "Infographic Failed To Update";
                }
            }
            return View(infographic);
        }

        public JsonResult _CategoryList(int draw, int start = 0, int length = 2)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = infographicService.GetCategories(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        public JsonResult _InfographicList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = infographicService.GetInfographics(out total_count, start, length, search, order_by, order_dir, CategoryId);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }


        private void FillCategories(int? selected_value)
        {
            SelectList result = infographicService.GetSelectListCategories(selected_value);
            ViewData["InfographicCategoryID_Data"] = result;
        }

        [HttpPost]
        public ActionResult DeleteInfographicImage(int KeyissueId, int Type)
        {
            if (infographicService.DeleteInfographicImage(KeyissueId, Type))
                TempData["SuccessMessage"] = "Image deleted Successfully";
            else
                TempData["ErrorMessage"] = "Image failed to delete";

            return RedirectToAction("EditInfographic", new { id = KeyissueId });
        }

        public ActionResult DeleteInfographic(int id)
        {
            if (infographicService.DeleteInfographic(id))
            {
                TempData["SuccessMessage"] = "Infographics Deleted Successfuly";
                return RedirectToAction("Infographics");
            }
            else
            {
                TempData["ErrorMessage"] = "Category Failed To Delete";
                return RedirectToAction("EditInfographic", new { id = id });
            }
        }
    }
}