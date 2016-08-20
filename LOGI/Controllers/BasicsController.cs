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
    public class BasicsController : BaseController
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private InfographicService infographicService;
        #endregion

        #region Constructor
        public BasicsController()
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
        // GET: Basics
        public ActionResult Index(string id = "0", string page = "1")
        {
            ViewBag.page = page;
            //Redirecting for old URLs
            if(id == "1")
            {
                return RedirectToActionPermanent("Infographic", new { id = 1 });
            }
            if (id == "2")
            {
                return RedirectToActionPermanent("Infographic", new { id = 2 });
            }
            if (id == "5")
            {
                return RedirectToActionPermanent("Infographic", new { id = 2 });
            }
            if (id == "6")
            {
                return RedirectToActionPermanent("Infographic", new { id = 2 });
            }
            //End of redirection
            
            return View(Convert.ToInt32(id));
        }

        public ActionResult _Infographics(string language = "en", int order = 1, int selected = 0)
        {
            InfographicSetViewModel view_model = infographicService.GetInfographicsViewModel(language, order);
            ViewBag.SelectedInfographic = selected;
            return PartialView(view_model);
        }

        public ActionResult Infographic(string id = "")
        {
            string culture = CultureHelper.GetCurrentCulture();
            bool isArabic = false;
            if (culture.Contains("ar"))
                isArabic = true;
            InfographicViewModel view_model = infographicService.GetInfographicViewModel(id, isArabic?"ar":"en");
            int res; 
            if(Int32.TryParse(id, out res))
            {
                return RedirectToAction("Infographic", new { id = view_model.FriendlyURL });
            }

            return View(view_model);
        }
    }
}