using LOGI.Models;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Areas.admin.Controllers
{
    public class AdminCampaignController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private CampaignService campaignService;
        #endregion

        #region Constructor
        public AdminCampaignController()
        {
            campaignService = new CampaignService(DBContext);

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

        // GET: admin/AdminCampaign
        public ActionResult Index()
        {
            Campaign view_model = campaignService.GetCampaign();
            return View(view_model);
        }

       [HttpPost]
        public ActionResult Index(Campaign campaign, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> MobileImages)
        {
            if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
            {
                TempData["ErrorMessage"] = "Campaign Failed To Update, Invalid Image File";
            }
            else if (MobileImages != null && MobileImages.Count() > 0 && !ImageService.IsValid(MobileImages))
            {
                TempData["ErrorMessage"] = "Campaign Failed To Update, Invalid Mobile Image File";
            }
            else
            {
                Campaign result = campaignService.UpdateCampaign(campaign, Images, MobileImages);
                TempData["SuccessMessage"] = "Campaign Updated Successfully";
                return View(result);
            }
            
            return View(campaign);
        }
    }
}