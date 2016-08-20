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
    public class DonateController : BaseController
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private DonateService donateService;
        private PaymentOrderService paymentService;
        #endregion

        #region Constructor
        public DonateController()
        {
            donateService = new DonateService(DBContext);
            paymentService = new PaymentOrderService(DBContext);
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
            DonateViewModel view_model = donateService.getDonateViewModel();
            return View(view_model);
        }

        [HttpPost]
        public ActionResult Index(string reason_code = "")
        {
            DonateViewModel view_model = donateService.getDonateViewModel();
            return View(view_model);
        }

        [HttpPost]
        public JsonResult Donate(string access_key, string profile_id, string transaction_uuid, string signed_field_names, string unsigned_field_names, string signed_date_time, string locale, string transaction_type, string amount, string currency)
        {
            TempData["RedirectToPayment"] = true;
            PaymentOrder order = null;
            try
            {
                order = new PaymentOrder()
                {
                    Amount = amount,
                    Currency = currency,
                    OrderDate = DateTime.Now
                };
                paymentService.AddPaymentOrder(order);
            }catch(Exception ex)
            {

            }

            Dictionary<string, string> payment_info = new Dictionary<string, string>();
            payment_info.Add("access_key", access_key);
            payment_info.Add("profile_id", profile_id);
            payment_info.Add("transaction_uuid", transaction_uuid);
            payment_info.Add("signed_field_names", signed_field_names);
            payment_info.Add("unsigned_field_names", unsigned_field_names);
            payment_info.Add("signed_date_time", signed_date_time);
            payment_info.Add("locale", locale);
            payment_info.Add("transaction_type", transaction_type);
            if (order != null && order.ID > 0)
                 payment_info.Add("reference_number", order.ID.ToString());
            else
                payment_info.Add("reference_number", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            payment_info.Add("amount", amount);
            payment_info.Add("currency", currency);

            JsonResult result = Json(new {
                success= true,
                access_key = access_key,
                profile_id = profile_id,
                transaction_uuid = transaction_uuid,
                signed_field_names = signed_field_names,
                unsigned_field_names = unsigned_field_names,
                signed_date_time = signed_date_time,
                locale = locale,
                transaction_type = transaction_type,
                reference_number = (order != null && order.ID > 0) ? order.ID.ToString() : DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString(),
                amount = amount,
                currency = currency,
                signature = LOGI.Services.PaymentOrderService.sign(payment_info)
            });

            return result;
        }
        #endregion
    }
}