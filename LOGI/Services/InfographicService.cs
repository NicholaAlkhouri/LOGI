using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LOGI.Services
{
    public class InfographicService
    {
        private ApplicationDbContext DbContext;

        public InfographicService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        internal List<InfographicCategory> GetCategories(out int total_count, int page, int page_size, string search_key, string order_by, string order_dir)
        {
            IQueryable<InfographicCategory> result;
            if (order_by == "Name")
            {
                if (order_dir == "desc")
                    result = DbContext.InfographicCategories.OrderByDescending(a => a.Name);
                else
                    result = DbContext.InfographicCategories.OrderBy(a => a.Name);
            }
            else if (order_by == "NameAr")
            {
                if (order_dir == "desc")
                    result = DbContext.InfographicCategories.OrderByDescending(a => a.NameAr);
                else
                    result = DbContext.InfographicCategories.OrderBy(a => a.NameAr);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.InfographicCategories.OrderByDescending(a => a.ID);
                else
                    result = DbContext.InfographicCategories.OrderBy(a => a.ID);
            }

            if (search_key != "")
                result = result.Where(a => a.Name.Contains(search_key) || a.NameAr.Contains(search_key));


            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            return result.ToList();
        }

        public int AddCategory(InfographicCategory category)
        {
            DbContext.InfographicCategories.Add(category);
            try
            {
               
                DbContext.SaveChanges();

                return category.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public InfographicCategory GetCategory(int id)
        {
            InfographicCategory category = DbContext.InfographicCategories.Where(c => c.ID == id).FirstOrDefault();
            if (category != null)
                return category;

            return null;
        }

        public int UpdateCategory(InfographicCategory category)
        {
            InfographicCategory C = DbContext.InfographicCategories.Where(c => c.ID == category.ID).FirstOrDefault();

            if (C == null)
                return -1;

            C.Name = category.Name;
            C.NameAr = category.NameAr;
            C.Description = category.Description;
            C.DescriptionAr = category.DescriptionAr;
            C.Order = category.Order;


            try
            {
                DbContext.SaveChanges();

                return C.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public bool DeleteCategory(int id)
        {
            InfographicCategory a = DbContext.InfographicCategories.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;

            DbContext.InfographicCategories.Remove(a);

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

        public bool DeleteInfographic(int id)
        {
            Infographic A = DbContext.Infographics.Where(aa => aa.ID == id).FirstOrDefault();

            if (A == null)
                return false;

            DbContext.Infographics.Remove(A);

            try
            {
                if (A.ImageURL != null && A.ImageURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL));
                }

                if (A.ThumbURL != null && A.ThumbURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURL));
                }
                   
                if (A.ImageURLAr != null && A.ImageURLAr != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURLAr));
                }
               
                if (A.ThumbURLAr != null && A.ThumbURLAr != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURLAr));
                }

                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public SelectList GetSelectListCategories(int? selected_value)
        {

            return new SelectList((from s in DbContext.InfographicCategories.OrderBy(t => t.Name).ToList()
                                   select new
                                   {
                                       ID_Dipendente = s.ID,
                                       CategoryName = s.Name + " (" + s.NameAr + ")"
                                   }),
                                "ID_Dipendente",
                                "CategoryName",
                                selected_value);
        }

        public bool IsURLExist(string url, int current_id)
        {
            return DbContext.Infographics.Where(k => (k.FriendlyURL == url || k.FriendlyURLAr == url) && k.ID != current_id).Count() > 0;
        }

        public int AddInfographic(Infographic infographic, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Thumbs = null, IEnumerable<HttpPostedFileBase> ImagesAr = null, IEnumerable<HttpPostedFileBase> ThumbsAr = null)
        {
            DbContext.Infographics.Add(infographic);
            try
            {
                DbContext.SaveChanges();

                if (infographic.ID > 0 && Images != null && Images.Count() > 0 && Images.Where(i => i != null).Count() > 0)
                {
                    AddInfographicImages(infographic.ID, Images,1);
                }

                if (infographic.ID > 0 && Thumbs != null && Thumbs.Count() > 0 && Thumbs.Where(i => i != null).Count() > 0)
                {
                    AddInfographicImages(infographic.ID, Thumbs,2);
                }
                if (infographic.ID > 0 && ImagesAr != null && ImagesAr.Count() > 0 && ImagesAr.Where(i => i != null).Count() > 0)
                {
                    AddInfographicImages(infographic.ID, ImagesAr,3);
                }
                if (infographic.ID > 0 && ThumbsAr != null && ThumbsAr.Count() > 0 && ThumbsAr.Where(i => i != null).Count() > 0)
                {
                    AddInfographicImages(infographic.ID, ThumbsAr,4);
                }


                return infographic.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool AddInfographicImages(int ID, IEnumerable<HttpPostedFileBase> Images, int type)
        {
            Infographic infographic = DbContext.Infographics.Where(m => m.ID == ID).FirstOrDefault();

            if (infographic == null)
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

                            file_name = (ID + "-INFO-" + type.ToString() + image.FileName).Replace(" ", "-");


                            System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                            //save Original Image
                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                            //Update the DB value
                            if(type == 1) //full english image
                            {
                                infographic.ImageURL = ImageService.GetImagesDirectory() + file_name;
                            }else if(type == 2) //thumb english image
                            {
                                infographic.ThumbURL = ImageService.GetImagesDirectory() + file_name;
                            }
                            else if (type == 3) //Full Arabic  image
                            {
                                infographic.ImageURLAr = ImageService.GetImagesDirectory() + file_name;
                            }
                            else if (type == 4) //thumb Arabic image
                            {
                                infographic.ThumbURLAr = ImageService.GetImagesDirectory() + file_name;
                            }
                            
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

        public int UpdateInfographic(Infographic infographic, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Thumbs = null, IEnumerable<HttpPostedFileBase> ImagesAr = null, IEnumerable<HttpPostedFileBase> ThumbsAr = null)
        {
            Infographic A = DbContext.Infographics.Where(a => a.ID == infographic.ID).FirstOrDefault();

            if (A == null)
                return -1;

            A.FriendlyURL = infographic.FriendlyURL;
            A.FriendlyURLAr = infographic.FriendlyURLAr;
            A.InfographicCategoryID = infographic.InfographicCategoryID;
            A.Name = infographic.Name;
            A.NameAr = infographic.NameAr;
            A.Order = infographic.Order;
            A.MetaDescriptionAr = infographic.MetaDescriptionAr;
            A.MetaDescription = infographic.MetaDescription;

            try
            {
                DbContext.SaveChanges();
                if (A.ID > 0 && Images != null && Images.Where(i => i != null).Count() > 0)
                {
                    if (A.ImageURL != null && A.ImageURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL));
                    }
                    AddInfographicImages(A.ID, Images, 1);
                }

                if (A.ID > 0 && Thumbs != null && Thumbs.Where(i => i != null).Count() > 0)
                {
                    if (A.ThumbURL != null && A.ThumbURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURL));
                    }
                    AddInfographicImages(A.ID, Thumbs, 2);
                }

                if (A.ID > 0 && ImagesAr != null && ImagesAr.Where(i => i != null).Count() > 0)
                {
                    if (A.ImageURLAr != null && A.ImageURLAr != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURLAr));
                    }
                    AddInfographicImages(A.ID, ImagesAr, 3);
                }

                if (A.ID > 0 && ThumbsAr != null && ThumbsAr.Where(i => i != null).Count() > 0)
                {
                    if (A.ThumbURLAr != null && A.ThumbURLAr != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURLAr));
                    }
                    AddInfographicImages(A.ID, ThumbsAr, 4);
                }
                return A.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public Infographic GetInfographic(int id)
        {
            Infographic A = DbContext.Infographics.Where(a => a.ID == id).FirstOrDefault();
            if (A == null)
                return null;
            
            return A;
        }

        internal List<Infographic> GetInfographics(out int total_count, int page, int page_size, string search_key, string order_by, string order_dir, int CategoryId)
        {
            IQueryable<Infographic> result;

           
            if (order_by == "Name")
            {
                if (order_dir == "desc")
                    result = DbContext.Infographics.OrderByDescending(a => a.Name);
                else
                    result = DbContext.Infographics.OrderBy(a => a.Name);
            }
            else if (order_by == "NameAr")
            {
                if (order_dir == "desc")
                    result = DbContext.Infographics.OrderByDescending(a => a.NameAr);
                else
                    result = DbContext.Infographics.OrderBy(a => a.NameAr);
            }
            else if (order_by == "Order")
            {
                if (order_dir == "Order")
                    result = DbContext.Infographics.OrderByDescending(a => a.NameAr);
                else
                    result = DbContext.Infographics.OrderBy(a => a.NameAr);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Infographics.OrderByDescending(a => a.ID);
                else
                    result = DbContext.Infographics.OrderBy(a => a.ID);
            }

            if (CategoryId != 0)
            {
                result = result.Where(I => I.InfographicCategoryID == CategoryId);

            }


            if (search_key != "")
                result = result.Where(a => a.Name.Contains(search_key) || a.NameAr.Contains(search_key));


            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            return result.ToList();
        }

        public bool DeleteInfographicImage(int id, int Type)
        {
            Infographic A = DbContext.Infographics.Where(a => a.ID == id).FirstOrDefault();

            if (A == null)
                return false;

            try
            {
                if (Type == 1 && A.ImageURL != null && A.ImageURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL));

                    A.ImageURL = null;
                }

                if (Type == 2 && A.ThumbURL != null && A.ThumbURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURL));

                    A.ThumbURL = null;
                }

                if (Type == 3 && A.ImageURLAr != null && A.ImageURLAr != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURLAr));

                    A.ImageURLAr = null;
                }

                if (Type == 4 && A.ThumbURLAr != null && A.ThumbURLAr != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ThumbURLAr));

                    A.ThumbURLAr = null;
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public InfographicSetViewModel GetInfographicsViewModel(string language, int order)
        {
            InfographicSetViewModel result = new InfographicSetViewModel();
            result.Categories = new List<InfographicCategoryViewModel>();

            //get all categories included
            List<InfographicCategory> categories = DbContext.InfographicCategories.Where(c => c.Order == order || order == 0).Include(c => c.Infographics).ToList();

            foreach(InfographicCategory cat in categories)
            {
                InfographicCategoryViewModel cat_viewmodel = new InfographicCategoryViewModel();
                if (language.Contains("ar"))
                {
                    cat_viewmodel.Description = cat.DescriptionAr;
                    cat_viewmodel.Name = cat.NameAr;
                }
                else
                {
                    cat_viewmodel.Description = cat.Description;
                    cat_viewmodel.Name = cat.Name;
                }
                cat_viewmodel.Order = cat.Order;
                cat_viewmodel.Infographics = new List<InfographicViewModel>();

                foreach(Infographic info in cat.Infographics)
                {
                    InfographicViewModel info_viewmodel = new InfographicViewModel();
                    if (language.Contains("ar"))
                    {
                        info_viewmodel.FriendlyURL = info.FriendlyURLAr;
                        info_viewmodel.ID = info.ID;
                        info_viewmodel.ImageURL = info.ImageURLAr;
                        info_viewmodel.Name = info.NameAr;
                        info_viewmodel.Order = info.Order;
                        info_viewmodel.ThumbURL = info.ThumbURLAr;
                        if (info_viewmodel.FriendlyURL == null || info_viewmodel.FriendlyURL == "")
                            info_viewmodel.FriendlyURL = info.FriendlyURL;
                        if (info_viewmodel.ImageURL == null || info_viewmodel.ImageURL == "")
                            info_viewmodel.ImageURL = info.ImageURL;
                        if (info_viewmodel.Name == null || info_viewmodel.Name == "")
                            info_viewmodel.Name = info.Name;
                        if (info_viewmodel.ThumbURL == null || info_viewmodel.ThumbURL == "")
                            info_viewmodel.ThumbURL = info.ThumbURL;
                    }
                    else
                    {
                        info_viewmodel.FriendlyURL = info.FriendlyURL;
                        info_viewmodel.ID = info.ID;
                        info_viewmodel.ImageURL = info.ImageURL;
                        info_viewmodel.Name = info.Name;
                        info_viewmodel.Order = info.Order;
                        info_viewmodel.ThumbURL = info.ThumbURL;
                    }

                    cat_viewmodel.Infographics.Add(info_viewmodel);
                }

                result.Categories.Add(cat_viewmodel);
            }

            return result;
        }

        public InfographicViewModel GetInfographicViewModel(string id, string language)
        {
            Infographic infographic;
            int info_id;
            bool parse_result = Int32.TryParse(id, out info_id);
            bool isArabic = false;

            if (parse_result) //id provided
            {
                infographic = DbContext.Infographics.Where(i => i.ID == info_id).Include(i => i.InfographicCategory).FirstOrDefault();
                if (language == "ar")
                    isArabic = true;
            }else //friendly url
            {
                infographic = DbContext.Infographics.Where(i => i.FriendlyURL == id).Include(i => i.InfographicCategory).FirstOrDefault();

                if(infographic == null)
                {
                    isArabic = true;
                    infographic = DbContext.Infographics.Where(i => i.FriendlyURLAr == id).Include(i => i.InfographicCategory).FirstOrDefault();
                }
            }

            if (infographic == null)
                return null;

            InfographicViewModel result = new InfographicViewModel();
            result.CategoryOrder = infographic.InfographicCategory.Order;
            if(isArabic)
            {
                result.FriendlyURL = infographic.FriendlyURLAr;
                result.ID = infographic.ID;
                result.ImageURL = infographic.ImageURLAr;
                result.ThumbURL = infographic.ThumbURLAr;
                result.Order = infographic.Order;
                result.Name = infographic.NameAr;
                result.MetaDescription = infographic.MetaDescriptionAr;

                if( result.FriendlyURL == null ||  result.FriendlyURL == "")
                    result.FriendlyURL = infographic.FriendlyURL;

                if (result.ImageURL == null || result.ImageURL == "")
                    result.ImageURL = infographic.ImageURL;

                if (result.ThumbURL == null || result.ThumbURL == "")
                    result.ThumbURL = infographic.ThumbURL;

                if (result.Name == null || result.Name == "")
                    result.Name = infographic.Name;

                if (result.MetaDescription == null || result.MetaDescription == "")
                    result.MetaDescription = infographic.MetaDescription;
            }
            else
            {
                result.FriendlyURL = infographic.FriendlyURL;
                result.ID = infographic.ID;
                result.ImageURL = infographic.ImageURL;
                result.ThumbURL = infographic.ThumbURL;
                result.Order = infographic.Order;
                result.Name = infographic.Name;
                result.MetaDescription = infographic.MetaDescription;
            }

            return result;
        }
    }
}