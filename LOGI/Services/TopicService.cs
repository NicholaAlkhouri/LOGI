using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Services
{
    public class TopicService
    {
        private ApplicationDbContext DbContext;

        public TopicService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<Topic> GetTopics(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<Topic> result;
            if (order_by == "Name")
            {
                if (order_dir == "desc")
                    result = DbContext.Topics.OrderByDescending(a => a.Name);
                else
                    result = DbContext.Topics.OrderBy(a => a.Name);
            }
            if (order_by == "NameAr")
            {
                if (order_dir == "desc")
                    result = DbContext.Topics.OrderByDescending(a => a.NameAr);
                else
                    result = DbContext.Topics.OrderBy(a => a.NameAr);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Topics.OrderByDescending(a => a.ID);
                else
                    result = DbContext.Topics.OrderBy(a => a.ID);
            }

            if (search_key != "")
                result = result.Where(a => a.Name.Contains(search_key) || a.NameAr.Contains(search_key));


            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            return result.ToList();
        }

        public int AddTopic(Topic topic)
        {
            DbContext.Topics.Add(topic);
            try
            {
                DbContext.SaveChanges();
                return topic.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public Topic GetTopic(int id)
        {
            Topic A = DbContext.Topics.Where(a => a.ID == id).FirstOrDefault();

            return A;
        }

        public int UpdateTopic(Topic topic)
        {
            Topic A = DbContext.Topics.Where(a => a.ID == topic.ID).FirstOrDefault();

            if (A == null)
                return -1;

            A.Name = topic.Name;
            A.NameAr = topic.NameAr;

            try
            {
                DbContext.SaveChanges();
                return A.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public bool DeleteTopic(int id)
        {
            Topic a = DbContext.Topics.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;

            DbContext.Topics.Remove(a);

            try
            {
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public SelectList GetSelectListTopics(int? selected_value)
        {

            return new SelectList((from s in DbContext.Topics.OrderBy(t=> t.Name).ToList()
                                   select new
                                   {
                                       ID_Dipendente = s.ID,
                                       TopicName = s.Name + " (" + s.NameAr + ")"
                                   }),
                                "ID_Dipendente",
                                "TopicName",
                                selected_value);
        }

        public SelectList GetSelectListFrontTopics(int? selected_value,string language = "en")
        {

            if(language == "en")
                return new SelectList((from s in DbContext.Topics.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           TopicName = s.Name
                                       }).OrderBy(t => t.TopicName),
                                    "ID_Dipendente",
                                    "TopicName",
                                    selected_value);
            else 
                return new SelectList((from s in DbContext.Topics.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           TopicName = s.NameAr
                                       }).OrderBy(t => t.TopicName),
                                    "ID_Dipendente",
                                    "TopicName",
                                    selected_value);
        }
    }
}