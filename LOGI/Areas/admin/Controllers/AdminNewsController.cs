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
    public class AdminNewsController : Controller
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private KeyIssueService keyIssueService;
        private SectionVariableService variabeService;
        #endregion

        #region Constructor
        public AdminNewsController()
        {
            keyIssueService = new KeyIssueService(DBContext);
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
        public ActionResult New(int? originalId = null, string language = "en")
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;

            KeyIssue KI = new KeyIssue();
            KI.PublishDate = DateTime.Now;
            KI.IsInNews = true;
            if (originalId != null)
            {
                KeyIssue ol_keyissue = keyIssueService.GetKeyIssue(originalId.Value);
                KI.OriginalId = originalId;
                KI.Language = language;
                KI.Tags = ol_keyissue.Tags;
                FillTopics(ol_keyissue.TopicID);
                FillAuthors(ol_keyissue.AuthorID);
                FillSources(ol_keyissue.SourceID);
                FillTypes(ol_keyissue.TypeID);
                FillCountries(ol_keyissue.Countries == null ? null : ol_keyissue.Countries.Select(k => k.ID).ToList());
                ViewBag.OriginalVersion = originalId;
            }
            else
            {
                FillTopics(null);
                FillAuthors(null);
                FillSources(null);
                FillTypes(null);
                FillCountries(null);
            }

           
            return View(KI);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(KeyIssue keyissue, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Files, List<string> ArabicTags, List<string> EnglishTags, List<int> KCountries)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;

            FillTopics(keyissue.TopicID);
            FillAuthors(keyissue.AuthorID);
            FillSources(keyissue.SourceID);
            FillTypes(keyissue.TypeID);
            FillCountries(KCountries);

            if (ModelState.IsValid)
            {
                string cleaned_url = URLValidator.CleanURL(keyissue.FriendlyURL);
                
                if (keyIssueService.IsURLExist(cleaned_url, -1))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Key Issue Failed To Edit, Friendly URL Already Exist";
                }
                else if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                 {
                     TempData["ErrorMessage"] = "News Failed To Add, Invalid Image File";
                 }
                else if (!URLValidator.IsValidURLPart(keyissue.FriendlyURL))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "News Failed To Add, Friendly URL Not Valid";
                }
                 else
                {
                    keyissue.FriendlyURL = cleaned_url;
                    int new_id = keyIssueService.AddKeyIssue(keyissue, ArabicTags, EnglishTags, Images, Files, KCountries);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "News Added Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "News Failed To Add";
                }
                
            }
            return View(keyissue);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;

            KeyIssue view_model = keyIssueService.GetKeyIssue(id);

            if (view_model.OriginalId != null)
                ViewBag.OriginalVersion = keyIssueService.GetSimpleKeyIssue(view_model.OriginalId.Value);

            ViewBag.TranslatedVersion = keyIssueService.GetTranslatedKeyIssue(view_model.ID);

            FillTopics(view_model.TopicID);
            FillAuthors(view_model.AuthorID);
            FillSources(view_model.SourceID);
            FillTypes(view_model.TypeID);
            FillCountries(view_model.Countries == null ? null : view_model.Countries.Select(k=>k.ID).ToList());

            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(KeyIssue keyissue, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> Files, List<string> ArabicTags, List<string> EnglishTags, List<int> KCountries)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;

            FillTopics(keyissue.TopicID);
            FillAuthors(keyissue.AuthorID);
            FillSources(keyissue.SourceID);
            FillTypes(keyissue.TypeID);
            FillCountries(KCountries);

            if (ModelState.IsValid)
            {
                string cleaned_url = URLValidator.CleanURL(keyissue.FriendlyURL);
                if (keyIssueService.IsURLExist(cleaned_url, keyissue.ID))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "Key Issue Failed To Edit, Friendly URL Already Exist";
                }
                else if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                {
                    TempData["ErrorMessage"] = "News Failed To Update, Invalid Image File";
                }
                else if (!URLValidator.IsValidURLPart(keyissue.FriendlyURL))
                {
                    ModelState.AddModelError("FriendlyURL", new Exception());
                    TempData["ErrorMessage"] = "News Failed To Update, Friendly URL Not Valid";
                }
                else
                {
                    keyissue.FriendlyURL = cleaned_url;
                    int new_id = keyIssueService.UpdateKeyIssue(keyissue, ArabicTags, EnglishTags, Images, Files, KCountries);

                    if (new_id > 0)
                    {
                        TempData["SuccessMessage"] = "News Updated Successfully";
                        return RedirectToAction("Edit", new { id = new_id });
                    }
                    else
                        TempData["ErrorMessage"] = "News Failed To Update";
                }
            }
            return View(keyissue);
        }

        public ActionResult Delete(int id)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;
            if (keyIssueService.DeleteKeyIssue(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public ActionResult DeleteImage(int KeyissueId)
        {
            if (keyIssueService.DeleteKeyissueImage(KeyissueId))
                TempData["SuccessMessage"] = "Image deleted Successfully";
            else
                TempData["ErrorMessage"] = "Image failed to delete";

            return RedirectToAction("Edit", new { id = KeyissueId });
        }

        public ActionResult List(int TopicID = 0)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSLIST;

            AdminTopicsListViewModel view_model = new AdminTopicsListViewModel() { TopicID = TopicID };
            FillTopics(TopicID);
            return View(view_model);
        }

        public JsonResult _KeyIssuesList(int draw, int start = 0, int length = 2, int TopicId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = keyIssueService.GetKeyIssues(out total_count, TopicId, start, length, search, order_by, order_dir,true);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

        public ActionResult PressRelease()
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSRELEASE;

            SectionVariable pdf = variabeService.GetVariableByKey("PressReleasePDF");

            return View(pdf);
        }

        [HttpPost]
        public ActionResult PressRelease(IEnumerable<HttpPostedFileBase> PDF)
        {
            ViewBag.MainNav = Navigator.Main.NEWS;
            ViewBag.SubNav = Navigator.Sub.NEWSRELEASE;

            if (ModelState.IsValid)
            {
                SectionVariable donate_pdf = variabeService.GetVariableByKey("PressReleasePDF");

                if (PDF != null && PDF.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    if (donate_pdf != null)
                        fs.DeleteFile(donate_pdf.Value);

                    string file_url = fs.SaveFiles(PDF);

                    if (variabeService.AddUpdateVariable("PressReleasePDF", file_url) > 0)
                    {
                        TempData["SuccessMessage"] = "File Updated Successfully";
                        return RedirectToAction("PressRelease");
                    }
                    else
                        TempData["ErrorMessage"] = "File Failed to Updated";
                }

            }
            return View();
        }
        #endregion

        #region Helpers
        private void FillTopics(int? selected_value)
        {
            TopicService TS = new TopicService(DBContext);
            SelectList result = TS.GetSelectListTopics(selected_value);
            ViewData["TopicID_Data"] = result;
        }
        private void FillSources(int? selected_value)
        {
            SourceService TS = new SourceService(DBContext);
            SelectList result = TS.GetSelectListSources(selected_value);
            ViewData["SourceID_Data"] = result;
        }
        private void FillAuthors(int? selected_value)
        {
            AuthorService TS = new AuthorService(DBContext);
            SelectList result = TS.GetSelectListAuthors(selected_value);
            ViewData["AuthorID_Data"] = result;
        }
        private void FillTypes(int? selected_value)
        {
            TypeService TS = new TypeService(DBContext);
            SelectList result = TS.GetSelectListTypes(selected_value);
            ViewData["TypeID_Data"] = result;
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