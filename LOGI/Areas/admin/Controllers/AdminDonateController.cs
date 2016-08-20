using LOGI.Areas.Admin.Models;
using LOGI.Models;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Areas.admin.Controllers
{
    public class AdminDonateController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private SectionVariableService variabeService;
        #endregion

        #region Constructor
        public AdminDonateController()
        {
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
        public ActionResult Index()
        {
            ViewBag.MainNav = Navigator.Main.DONATE;

            SectionVariable donate_pdf = variabeService.GetVariableByKey("DonatePDF");

            return View(donate_pdf);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> PDF)
        {
            ViewBag.MainNav = Navigator.Main.DONATE;

            if (ModelState.IsValid)
            {
                SectionVariable donate_pdf = variabeService.GetVariableByKey("DonatePDF");

                if (PDF != null && PDF.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    if (donate_pdf != null)
                        fs.DeleteFile(donate_pdf.Value);

                    string file_url = fs.SaveFiles(PDF);

                    if(variabeService.AddUpdateVariable("DonatePDF", file_url) > 0)
                    {
                        TempData["SuccessMessage"] = "File Updated Successfully";
                        return RedirectToAction("Index");
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