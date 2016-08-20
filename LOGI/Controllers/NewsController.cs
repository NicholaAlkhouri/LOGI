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
    public class NewsController : BaseController
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private KeyIssueService keyIssueService;
        #endregion

        #region Constructor
        public NewsController()
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

        // GET: News
        public ActionResult Index()
        {
            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            NewsViewModel res_model = new NewsViewModel();

            SearchViewModel view_model = keyIssueService.GetSearchResult("", null, 1, 20, null, null, null, null, null, isarabic ? "ar" : "en", null, null, true, true, true, true);
            view_model.keyword = "";
            view_model.topicId = null;
            view_model.authorId = null;
            view_model.sourceId = null;
            view_model.Page = 1;
            view_model.PageSize = 20;
            view_model.orderby = null;
            view_model.typeId = null;
            view_model.countryId = null;
            view_model.language = isarabic ? "ar" : "en";
            view_model.fromDate = null;
            view_model.toDate = null;
            view_model.isVideo = true;
            view_model.isText = true;
            view_model.isPDF = true;

            res_model.Search = view_model;
            res_model.LatestNews = keyIssueService.GetLatestNews(isarabic ? "ar" : "en");
            res_model.FeatureVideo = keyIssueService.GetFeatureNewsVideo(isarabic ? "ar" : "en");
            res_model.PressRelease = keyIssueService.GetPressRelease(isarabic ? "ar" : "en");
            res_model.Sources = keyIssueService.GetMediaSources();
            ViewBag.isSearch = false;
            return View(res_model);
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
            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");

            SearchViewModel view_model = keyIssueService.GetSearchResult(keyword, topicId, page, page_size, authorId, sourceId, orderby, typeId, countryId, isarabic ? "ar" : "en", fromDate, toDate, isText, isPDF, isVideo, true);
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

            NewsViewModel res_model = new NewsViewModel();
            res_model.Search = view_model;
            res_model.LatestNews = keyIssueService.GetLatestNews(isarabic ? "ar" : "en");
            res_model.FeatureVideo = keyIssueService.GetFeatureNewsVideo(isarabic ? "ar" : "en");
            res_model.PressRelease = keyIssueService.GetPressRelease(isarabic ? "ar" : "en");
            res_model.Sources = keyIssueService.GetMediaSources();
            ViewBag.isSearch = true;
            return View("Index", res_model);
        }

        // GET: all LOGI Press Releases
        public ActionResult PressReleases(int page = 1, int page_size = 20)
        {
            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            SearchViewModel view_model = keyIssueService.GetAllPressReleases(isarabic ? "ar" : "en", page, page_size);
            return View(view_model);
        }
    }   
}