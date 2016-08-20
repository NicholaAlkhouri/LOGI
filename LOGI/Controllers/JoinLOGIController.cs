using LOGI.Mailers;
using LOGI.Models;
using LOGI.Models.ViewModels;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class JoinLOGIController : BaseController
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private VacancyService vacancyService;
        #endregion

        #region Constructor
        public JoinLOGIController()
        {
            vacancyService = new VacancyService(DBContext);

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

        // GET: JoinLOGI
        public ActionResult Index()
        {
            JoinLOGIViewModel view_model = vacancyService.GetJoinLOGIViewModel();
            return View(view_model);
        }

        public ActionResult Apply(int id)
        {
            Vacancy vacancy = vacancyService.GetVacancy(id);

            if(vacancy == null)
                return RedirectToAction("Index");

            if (vacancy.DeadLine.CompareTo(DateTime.Now) < 0)
                return RedirectToAction("Index");

            ViewBag.Vacancy = vacancy;

            return View();
        }

        public ActionResult Preview(int id)
        {
            Vacancy vacancy = vacancyService.GetVacancy(id);

            if (vacancy == null)
                return RedirectToAction("Index");

            ViewBag.Vacancy = vacancy;

            return View("Apply",null);
        }

        [HttpPost]
        public ActionResult Apply(VacancyApplication vacancyApplication, int id, IEnumerable<HttpPostedFileBase> resume)
        {
            Vacancy vacancy = vacancyService.GetVacancy(id);
            
            if (vacancy == null)
                return RedirectToAction("Index");

            if (vacancy.DeadLine.CompareTo(DateTime.Now) < 0)
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                
                int new_id = vacancyService.AppplyToVacancy(id,vacancyApplication,resume);

                if (new_id > 0)
                {
                    VacancyApplication vacApp = vacancyService.GetVacancyApplicationById(new_id);

                    // Send Email
                    UserMailer mailer = new UserMailer();

                    // Attempt to send the email
                    try
                    {
                        string join_email = ConfigurationManager.AppSettings["JoinLogiEmail"];
                        mailer.JoinLogi(join_email, vacancy, vacApp).Send();
                    }
                    catch (Exception e)
                    {

                    }

                    TempData["Success"] = true;
                    return Redirect(Url.RouteUrl(new { controller = "JoinLOGI", action = "Apply", id = id }) + "#thankyou");
                    //return RedirectToAction("Apply", new {id = id });
                }
                else
                    TempData["Success"] = false;
                
            }

            ViewBag.Vacancy = vacancy;

            return View(vacancyApplication);
        }

        public ActionResult Expert()
        {
            ViewBag.SelectionPolicy = DBContext.SectionVariables.Where(v => v.Key == "SelectionPolicyPDF").FirstOrDefault();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Expert(Expert expert, IEnumerable<HttpPostedFileBase> resume)
        {
            if (ModelState.IsValid)
            {
                int new_id = vacancyService.AppplyAsExpert(expert, resume);

                if (new_id > 0)
                {

                    // Send Email
                    UserMailer mailer = new UserMailer();

                    // Attempt to send the email
                    try
                    {
                        string join_email = ConfigurationManager.AppSettings["JoinLogiEmail"];
                        mailer.JoinLogiAsExpert(join_email, expert).Send();
                    }
                    catch (Exception e)
                    {

                    }

                    TempData["Success"] = true;
                    return Redirect(Url.RouteUrl(new { controller = "JoinLOGI", action = "Expert" }) + "#thankyou");
                }
                else
                    TempData["Success"] = false;
            }

            return View(expert);
        }

    }
}