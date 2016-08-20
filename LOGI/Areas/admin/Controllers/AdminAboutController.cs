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
    [Authorize(Roles = "Admin")]
    public class AdminAboutController : Controller
    {
        #region Privates
        private ApplicationDbContext _dbContext;
        private SectionVariableService variabeService;
        #endregion

        #region Constructor
        public AdminAboutController()
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
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.OFFICIALDOC;

            SectionVariable R_pdf = variabeService.GetVariableByKey("RegistrationPDF");
            SectionVariable bylaw_pdf = variabeService.GetVariableByKey("ByLawsPDF");
            SectionVariable whitepaper = variabeService.GetVariableByKey("WhitePaperPDF");
            SectionVariable whitepaperAr = variabeService.GetVariableByKey("WhitePaperPDFAr");
            SectionVariable gift_acceptance = variabeService.GetVariableByKey("GiftAcceptancePolicy");

            OfficialDocsViewModel view_model = new OfficialDocsViewModel() { 
                ByLaws = bylaw_pdf, 
                Registration = R_pdf,
                Whitepaper = whitepaper, 
                WhitepaperAr = whitepaperAr,
                GiftAcceptance = gift_acceptance };

            return View(view_model);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> registration, IEnumerable<HttpPostedFileBase> giftAcceptance, IEnumerable<HttpPostedFileBase> bylaws, IEnumerable<HttpPostedFileBase> whitepaper, IEnumerable<HttpPostedFileBase> whitepaperAr)
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.OFFICIALDOC;

            if (ModelState.IsValid)
            {
                try
                { 
                    SectionVariable R_pdf = variabeService.GetVariableByKey("RegistrationPDF");

                    if (registration != null && registration.Where(f => f != null).Count() > 0)
                    {
                        FilesService fs = new FilesService();

                        if (R_pdf != null)
                            fs.DeleteFile(R_pdf.Value);

                        string file_url = fs.SaveFiles(registration);

                        variabeService.AddUpdateVariable("RegistrationPDF", file_url);
                    }

                    SectionVariable B_pdf = variabeService.GetVariableByKey("ByLawsPDF");

                    if (bylaws != null && bylaws.Where(f => f != null).Count() > 0)
                    {
                        FilesService fs = new FilesService();

                        if (B_pdf != null)
                            fs.DeleteFile(B_pdf.Value);

                        string file_url = fs.SaveFiles(bylaws);

                        variabeService.AddUpdateVariable("ByLawsPDF", file_url);
                    }


                    SectionVariable W_pdf = variabeService.GetVariableByKey("WhitePaperPDF");

                    if (whitepaper != null && whitepaper.Where(f => f != null).Count() > 0)
                    {
                        FilesService fs = new FilesService();

                        if (W_pdf != null)
                            fs.DeleteFile(W_pdf.Value);

                        string file_url = fs.SaveFiles(whitepaper);

                        variabeService.AddUpdateVariable("WhitePaperPDF", file_url);
                    }

                    SectionVariable W_pdf_ar = variabeService.GetVariableByKey("WhitePaperPDFAr");

                    if (whitepaperAr != null && whitepaperAr.Where(f => f != null).Count() > 0)
                    {
                        FilesService fs = new FilesService();

                        if (W_pdf_ar != null)
                            fs.DeleteFile(W_pdf_ar.Value);

                        string file_url = fs.SaveFiles(whitepaperAr);

                        variabeService.AddUpdateVariable("WhitePaperPDFAr", file_url);
                    }

                    SectionVariable A_pdf = variabeService.GetVariableByKey("GiftAcceptancePolicy");

                    if (giftAcceptance != null && giftAcceptance.Where(f => f != null).Count() > 0)
                    {
                        FilesService fs = new FilesService();

                        if (A_pdf != null)
                            fs.DeleteFile(A_pdf.Value);

                        string file_url = fs.SaveFiles(giftAcceptance);

                        variabeService.AddUpdateVariable("GiftAcceptancePolicy", file_url);
                    }

                    TempData["SuccessMessage"] = "Files Updated Successfully";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    TempData["ErrorMessage"] = "Files Failed to Updated";
                }
            }
            return View();
        }
        #endregion
    }
}