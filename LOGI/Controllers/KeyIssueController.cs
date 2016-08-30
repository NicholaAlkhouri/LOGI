using LOGI.Helpers;
using LOGI.Models;
using LOGI.Models.ViewModels;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class KeyIssuesController : BaseController
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private KeyIssueService keyIssueService;
        #endregion

        #region Constructor
        public KeyIssuesController()
        {
            keyIssueService = new KeyIssueService(DBContext);

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

        // GET: KeyIssue
        public ActionResult Index(string id = "")
        {
            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            KeyIssuesViewModel view_model = keyIssueService.GetKeyIssuesViewModel(isarabic?"ar":"en");

            return View("Index", view_model);
        }

        public ActionResult KeyIssue(string id)
        {
            //get current culture
            string culture = CultureHelper.GetCurrentCulture();

            KeyIssueViewModel view_model = keyIssueService.GetKeyIssueByURL(id);

            if (!view_model.KeyIssue.IsOnline)
                return RedirectToAction("Index");

            if(view_model.KeyIssue.Language == "ar" && !culture.Contains("ar"))
            {
                culture = CultureHelper.GetImplementedCulture("ar-LB");
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_culture"];
                if (cookie != null)
                    cookie.Value = culture;   // update cookie value
                else
                {
                    cookie = new HttpCookie("_culture");
                    cookie.Value = culture;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Add(cookie);
                return RedirectToAction("Keyissue", new { id = id });
            }
            else if (view_model.KeyIssue.Language == "en" && !culture.Contains("en"))
            {
                culture = CultureHelper.GetImplementedCulture("en-US");
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_culture"];
                if (cookie != null)
                    cookie.Value = culture;   // update cookie value
                else
                {
                    cookie = new HttpCookie("_culture");
                    cookie.Value = culture;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Add(cookie);
                return RedirectToAction("Keyissue", new {id = id });
            }

            if (view_model == null)
                return RedirectToAction("Index");
            else if(view_model.KeyIssue != null)
                keyIssueService.KeyIssueViewed(view_model.KeyIssue.ID,"en");

            return View(view_model);
        }

        public ActionResult Preview(string id)
        {
            //get current culture
            string culture = CultureHelper.GetCurrentCulture();

            KeyIssueViewModel view_model = keyIssueService.GetKeyIssueByURL(id);

            if (view_model.KeyIssue.Language == "ar" && !culture.Contains("ar"))
            {
                culture = CultureHelper.GetImplementedCulture("ar-LB");
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_culture"];
                if (cookie != null)
                    cookie.Value = culture;   // update cookie value
                else
                {
                    cookie = new HttpCookie("_culture");
                    cookie.Value = culture;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Add(cookie);
                return RedirectToAction("Preview", new { id = id });
            }
            else if (view_model.KeyIssue.Language == "en" && !culture.Contains("en"))
            {
                culture = CultureHelper.GetImplementedCulture("en-US");
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_culture"];
                if (cookie != null)
                    cookie.Value = culture;   // update cookie value
                else
                {
                    cookie = new HttpCookie("_culture");
                    cookie.Value = culture;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Add(cookie);
                return RedirectToAction("Preview", new { id = id });
            }

            if (view_model == null)
                return RedirectToAction("Index");
            else if (view_model.KeyIssue != null)
                keyIssueService.KeyIssueViewed(view_model.KeyIssue.ID, "en");

            return View("Keyissue",view_model);
        }

        public ActionResult AdvancedSearch(string keyword = null, int? topicId = null, int? authorId = null, int? sourceId = null, int? typeId = null, int? countryId = null, string language = null, DateTime? fromDate = null, DateTime? toDate = null, bool isVideo = true, bool isText = true, bool isPDF = true)
        {
            SearchViewModel view_model = new SearchViewModel();
            view_model.keyword = keyword;
            view_model.topicId = topicId;
            view_model.authorId = authorId;
            view_model.sourceId = sourceId;
            view_model.typeId = typeId;
            view_model.countryId = countryId;
            view_model.language = language;
            view_model.toDate = toDate;
            view_model.fromDate = fromDate;
            view_model.isVideo = isVideo;
            view_model.isText = isText;
            view_model.isPDF = isPDF;

            var lan = "en";
            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            if (isarabic)
                lan = "ar";

            FillSources(sourceId, lan);
            FillTopics(topicId, lan);

            FillCountries(countryId, lan);
            FillAuthors(authorId, lan);
            FillTypes(typeId, lan);

            return View(view_model);
        }

        public ActionResult Search(string keyword = null, int? topicId = null, int page = 1, int page_size = 20, int? authorId = null, int? sourceId = null, 
            string orderby = null,
            int? typeId = null, 
            int? countryId = null, 
            string language = null, 
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            bool isVideo = true, bool isText = true, bool isPDF = true)
        {

            SearchViewModel view_model = keyIssueService.GetSearchResult(keyword, topicId, page, page_size, authorId, sourceId, orderby, typeId, countryId, language, fromDate, toDate, isText, isPDF, isVideo);
            view_model.keyword = keyword;
            view_model.topicId = topicId;
            view_model.authorId = authorId;
            view_model.sourceId = sourceId;
            view_model.Page = page;
            view_model.PageSize = page_size;
            view_model.orderby = orderby;
            view_model.typeId = typeId;
            view_model.countryId = countryId;
            view_model.language = language;
            view_model.fromDate = fromDate;
            view_model.toDate = toDate;
            view_model.isVideo = isVideo;
            view_model.isText = isText;
            view_model.isPDF = isPDF;

            
            return View(view_model);
        }

        public ActionResult _KeyIssueSlider(string language = "en")
        {
            List<KeyIssueLink> view_model = keyIssueService.GetSliderKeyIssues(language);
            //reorder the result so the latest are displayed in the left part of the slider
            if(view_model.Count == 15)
            {
                List<KeyIssueLink> Final_list = new List<KeyIssueLink>();
                Final_list.Add(view_model[0]);
                Final_list.Add(view_model[2]);
                Final_list.Add(view_model[4]);
                Final_list.Add(view_model[6]);
                Final_list.Add(view_model[8]);
                Final_list.Add(view_model[10]);
                Final_list.Add(view_model[12]);
                Final_list.Add(view_model[1]);
                Final_list.Add(view_model[3]);
                Final_list.Add(view_model[5]);
                Final_list.Add(view_model[7]);
                Final_list.Add(view_model[9]);
                Final_list.Add(view_model[11]);
                Final_list.Add(view_model[13]);
                Final_list.Add(view_model[14]);

                view_model = Final_list;
            }
            

            return PartialView("_KeyIssuesSlider", view_model);
        }

        private void FillTopics(int? selected_value,string language)
        {
            TopicService TS = new TopicService(DBContext);
            SelectList result = TS.GetSelectListFrontTopics(selected_value, language);
            ViewData["TopicID_Data"] = result;
        }
        private void FillSources(int? selected_value,string language)
        {
            SourceService TS = new SourceService(DBContext);
            SelectList result = TS.GetSelectListFrontSources(selected_value, language);
            ViewData["SourceID_Data"] = result;
        }
        private void FillAuthors(int? selected_value,string language)
        {
            AuthorService TS = new AuthorService(DBContext);
            SelectList result = TS.GetSelectListFrontAuthors(selected_value, language);
            ViewData["AuthorID_Data"] = result;
        }
        private void FillTypes(int? selected_value, string language)
        {
            TypeService TS = new TypeService(DBContext);
            SelectList result = TS.GetSelectListFrontTypes(selected_value, language);
            ViewData["TypeID_Data"] = result;
        }
        private void FillCountries(int? selected_value, string language)
        {
            CountryService TS = new CountryService(DBContext);
            SelectList result = TS.GetSelectListFrontCountries(selected_value, language);
            ViewData["CountryID_Data"] = result;
        }
    }
}