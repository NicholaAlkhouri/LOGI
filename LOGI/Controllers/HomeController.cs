using LOGI.Helpers;
using LOGI.Models;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class HomeController : BaseController
    {
        private ApplicationDbContext _dbContext;

        public HomeController()
        {
        }
        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
           KeyIssueService keyissueService = new KeyIssueService(DBContext);
            ViewBag.FeaturedVideo = keyissueService.GetHomeFeaturedVideo(CultureHelper.GetCurrentCulture().Contains("ar") ? "ar" : "en");

            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            if(isarabic)
            {
                SectionVariable whitepaper = DBContext.SectionVariables.Where(v => v.Key == "WhitePaperPDFAr").FirstOrDefault();
                ViewBag.whitePaper = whitepaper == null ? "" : whitepaper.Value;
            }
            else
            {
                SectionVariable whitepaper = DBContext.SectionVariables.Where(v => v.Key == "WhitePaperPDF").FirstOrDefault();
                ViewBag.whitePaper = whitepaper == null ? "" : whitepaper.Value;
            }

            CampaignService campaignService = new CampaignService(DBContext);
            Campaign camp = campaignService.GetCampaign();
            if(camp != null && camp.IsOnline)
            {
                ViewBag.campaign = camp;
            }

            return View();
        }

        public ActionResult About()
        {
            AboutService about_service = new AboutService(DBContext);

            bool isarabic = CultureHelper.GetCurrentCulture().Contains("ar");
            if(isarabic)
                return View(about_service.GetAboutViewModel("ar"));
            else
                return View(about_service.GetAboutViewModel("en"));
        }

        [HttpPost]
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
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
            return RedirectToAction("Index");
        }

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

    }
}