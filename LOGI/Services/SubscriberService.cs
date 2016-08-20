using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class SubscriberService
    {
        private ApplicationDbContext DbContext;

        public SubscriberService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool Subscribe(string email)
        {
            Subscriber old_entity = DbContext.Subscribers.Where(s => s.Email == email.Trim()).FirstOrDefault();

            if (old_entity != null)
                return true;

            DbContext.Subscribers.Add(new Subscriber() { Email = email, SubscriptionDate = DateTime.Now });

            try
            {
                DbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<SubscribeViewModel> GetSubscribers(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<Subscriber> result;
            if (order_by == "Email")
            {
                if (order_dir == "desc")
                    result = DbContext.Subscribers.OrderByDescending(a => a.Email);
                else
                    result = DbContext.Subscribers.OrderBy(a => a.Email);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Subscribers.OrderByDescending(a => a.SubscriptionDate);
                else
                    result = DbContext.Subscribers.OrderBy(a => a.SubscriptionDate);
            }

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            List<Subscriber> res = result.ToList();

            List<SubscribeViewModel> final_res = new List<SubscribeViewModel>();

            foreach (Subscriber c in res)
            {
                final_res.Add(new SubscribeViewModel()
                {
                    SubscriptionDate = c.SubscriptionDate.ToString("dd/MMMM/yyyy"),
                    Email = c.Email,

                });
            }

            return final_res;
        }
    }
}