using LOGI.Mailers;
using LOGI.Models;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Controllers
{
    public class ContactusController : BaseController
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private ContactMessageService contactService;
        #endregion

        #region Constructor
        public ContactusController()
        {
            contactService = new ContactMessageService(DBContext);

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


        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactMessage contact_message)
        {
            if(ModelState.IsValid)
            {
                int id = contactService.AddContactMessage(contact_message);
                if (id > 0)
                {
                    // Send Email
                    UserMailer mailer = new UserMailer();

                    // Attempt to send the email
                    try
                    {
                        string conctUs_email = ConfigurationManager.AppSettings["ContactUsEmail"];
                        mailer.ContactUs(conctUs_email, contact_message).Send();
                    }
                    catch (Exception e)
                    {
                        LogService log_service = new LogService(DBContext);
                        string message = e.Message;
                        message += " " + e.StackTrace;

                        if(e.InnerException != null)
                        {
                            message += e.InnerException.Message + e.InnerException.StackTrace;
                            if (e.InnerException.InnerException != null)
                            {
                                message += e.InnerException.InnerException.Message + e.InnerException.InnerException.StackTrace;

                            }
                        }

                        log_service.AddLog(new Log() { Message = message });
                    }
                    TempData["success"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["success"] = false;
                    return View(contact_message);
                }
            }
            else
                return View(contact_message);
        }
    }
}