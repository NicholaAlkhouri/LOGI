using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LOGI.Services
{
    public class TimelineService
    {
        private ApplicationDbContext DbContext;
        private int TimelineImageWidth = 520;
        private int TimelineImageHeight = 264;

        public TimelineService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<Timeline> GetTimelines(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "order", string order_dir = "desc")
        {
            IQueryable<Timeline> result;
            if (order_by == "Title")
            {
                if (order_dir == "desc")
                    result = DbContext.Timelines.OrderByDescending(a => a.Title);
                else
                    result = DbContext.Timelines.OrderBy(a => a.Title);
            }
            else if (order_by == "Period")
            {
                if (order_dir == "desc")
                    result = DbContext.Timelines.OrderByDescending(a => a.Period);
                else
                    result = DbContext.Timelines.OrderBy(a => a.Period);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Timelines.OrderByDescending(a => a.order);
                else
                    result = DbContext.Timelines.OrderBy(a => a.order);
            }

            if (search_key != "")
                result = result.Where(a => a.Title.Contains(search_key));

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);


            return result.ToList();
        }


        public int AddTimeline(Timeline timeline, IEnumerable<HttpPostedFileBase> Images = null, List<int> Countries = null)
        {
            DbContext.Timelines.Add(timeline);
            try
            {
                if (Countries != null)
                {
                    timeline.Countries = new List<Country>();
                    foreach (int c in Countries)
                    {
                        Country country = DbContext.Countries.Where(cc => cc.ID == c).FirstOrDefault();
                        if (country != null)
                            timeline.Countries.Add(country);
                    }
                }

                DbContext.SaveChanges();
                if (timeline.ID > 0 && Images != null && Images.Count() > 0 && Images.Where(i => i != null).Count() > 0)
                {
                    AddTimelineImages(timeline.ID, Images);
                }
                return timeline.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool AddTimelineImages(int ID, IEnumerable<HttpPostedFileBase> Images)
        {
            Timeline timeline = DbContext.Timelines.Where(m => m.ID == ID).FirstOrDefault();

            if (timeline == null)
                return false;

            if (Images != null)
                foreach (HttpPostedFileBase image in Images)
                {
                    if (image != null)
                    {
                        try
                        {
                            //Original
                            string file_name = "";

                            file_name = (ID + "-timeline-" + image.FileName).Replace(" ", "-");

                            System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, TimelineImageWidth, TimelineImageHeight);

                            //Update the DB value
                            timeline.ImageURL = ImageService.GetImagesDirectory() + file_name;
                            DbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                }
            return true;
        }


        public Timeline GetTimeline(int id)
        {
            Timeline T = DbContext.Timelines.Where(a => a.ID == id).Include(a => a.Countries).FirstOrDefault();

            return T;
        }

        public int UpdateTimeline(Timeline timeline, IEnumerable<HttpPostedFileBase> Images = null, List<int> Countries = null)
        {
            Timeline T = DbContext.Timelines.Where(a => a.ID == timeline.ID).Include(a => a.Countries).FirstOrDefault();

            if (T == null)
                return -1;

            T.Description = timeline.Description;
            T.order = timeline.order;
            T.Period = timeline.Period;
            T.Title = timeline.Title;
            T.Type = timeline.Type;

            T.DescriptionAr = timeline.DescriptionAr;
            T.PeriodAr = timeline.PeriodAr;
            T.TitleAr = timeline.TitleAr;

            try
            {
                DbContext.SaveChanges();

                foreach (Country country in T.Countries.ToList())
                    T.Countries.Remove(country);

                if (Countries != null)
                {
                    T.Countries = new List<Country>();
                    foreach (int c in Countries)
                    {
                        Country country = DbContext.Countries.Where(cc => cc.ID == c).FirstOrDefault();
                        if (country != null)
                            T.Countries.Add(country);
                    }
                }
                DbContext.SaveChanges();

                if (T.ID > 0 && Images != null && Images.Where(i => i != null).Count() > 0)
                {
                    if (T.ImageURL != null && T.ImageURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + T.ImageURL));
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + T.ImageURL), TimelineImageWidth.ToString(), TimelineImageHeight.ToString());
                    }
                    AddTimelineImages(T.ID, Images);
                }
                return T.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public bool DeleteTimeline(int id)
        {
            Timeline T = DbContext.Timelines.Where(t => t.ID == id).FirstOrDefault();

            if (T == null)
                return false;

            DbContext.Timelines.Remove(T);

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

        public TimelinesViewModel GetTimelinesViewModel(int? countryId = null, string category = "")
        {
            TimelinesViewModel view_model = new TimelinesViewModel() {category = category ,countryId = countryId };

            var res = DbContext.Timelines.AsQueryable();

            if(countryId != null)
            {
                res = res.Where(t => t.Countries.Where(c => c.ID == countryId).Count() > 0);
            }

            if(category != "")
            {
                res = res.Where(t => t.Type == category);
            }

            res = res.OrderBy(t => t.order);

            view_model.Timelines = res.Select(t => new TimelineViewModel() { 
                    Description = t.Description,
                    DescriptionAr = t.DescriptionAr,
                    ID = t.ID,
                    ImageURL = t.ImageURL,
                    order = t.order,
                    Period = t.Period,
                    PeriodAr = t.PeriodAr,
                    Title = t.Title,
                    TitleAr = t.TitleAr,
                    Type = t.Type
            }).ToList();

            foreach (TimelineViewModel s in view_model.Timelines)
            {
                if(s.ImageURL !=null && s.ImageURL != "")
                    s.ImageURL = ImageService.GenerateImageFullPath(s.ImageURL, TimelineImageWidth.ToString(), TimelineImageHeight.ToString());
            }

            view_model.Countries = DbContext.Countries.ToList();

            return view_model;
        }
    }
}