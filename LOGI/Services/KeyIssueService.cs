using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LOGI.Models.ViewModels;
using System.Text.RegularExpressions;

namespace LOGI.Services
{
    public class KeyIssueService
    {
        private ApplicationDbContext DbContext;
        public Dictionary<string, List<string>> website_languages;

        public KeyIssueService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;

            //initialize website languge mapping
            website_languages = new Dictionary<string, List<string>>();

            List<string> ar_list = new List<string>();
            ar_list.Add("ar");

            List<string> en_list = new List<string>();
            en_list.Add("en");
            en_list.Add("fr");

            website_languages.Add("en", en_list);
            website_languages.Add("ar", ar_list);
        }

        

        public List<KeyIssue> GetKeyIssues(out int total_count, int TopicID, int page = 0, int page_size = 10, string search_key = "", string order_by = "PublishDate", string order_dir = "desc",bool isInNews = false)
        {
            IQueryable<KeyIssue> result;
            if (order_by == "Title")
            {
                if (order_dir == "desc")
                    result = DbContext.KeyIssues.OrderByDescending(a => a.Title);
                else
                    result = DbContext.KeyIssues.OrderBy(a => a.Title);
            }
            else if (order_by == "IsFeatured")
            {
                if (order_dir == "desc")
                    result = DbContext.KeyIssues.OrderByDescending(a => a.IsFeatured);
                else
                    result = DbContext.KeyIssues.OrderBy(a => a.IsFeatured);
            }
            else if (order_by == "InScroller")
            {
                if (order_dir == "desc")
                    result = DbContext.KeyIssues.OrderByDescending(a => a.InScroller);
                else
                    result = DbContext.KeyIssues.OrderBy(a => a.InScroller);
            }
            else if (order_by == "IsOnline")
            {
                if (order_dir == "desc")
                    result = DbContext.KeyIssues.OrderByDescending(a => a.IsOnline);
                else
                    result = DbContext.KeyIssues.OrderBy(a => a.IsOnline);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.KeyIssues.OrderByDescending(a => a.ID);
                else
                    result = DbContext.KeyIssues.OrderBy(a => a.ID);
            }

            if(isInNews == true)
            {
                result = result.Where(k => k.IsInNews == true);
            }
            else
            {
                result = result.Where(k => k.IsInNews == null || k.IsInNews == false);
            }

            if (TopicID > 0)
                result = result.Where(k => k.Topic.ID == TopicID);

            if (search_key != "")
                result = result.Where(a => a.Title.Contains(search_key));


            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Height;
            foreach (KeyIssue s in result)
            {
                s.FeatureImageURL = ImageService.GenerateImageFullPath(s.FeatureImageURL, img_width.ToString(), img_height.ToString());
            }

            return result.ToList();
        }

        public bool IsURLExist(string url, int current_id)
        {
            return DbContext.KeyIssues.Where(k => k.FriendlyURL == url && k.ID != current_id).Count() > 0;
        }

        public int AddKeyIssue(KeyIssue keyissue, List<string> ArabicTags, List<string> EnglishTags, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Files = null,List<int> Countries = null)
        {
            DbContext.KeyIssues.Add(keyissue);
            try
            {
                //add tags
                keyissue.Tags = new List<Tag>();
                if (ArabicTags != null)
                    foreach (string t in ArabicTags)
                        keyissue.Tags.Add(new Tag() { Name = t, Language = "ar" });

                if (EnglishTags != null)
                    foreach (string t in EnglishTags)
                        keyissue.Tags.Add(new Tag() { Name = t, Language = "en" });

                if(Countries !=null)
                {
                    keyissue.Countries = new List<Country>();
                    foreach(int c in Countries)
                    {
                        Country country = DbContext.Countries.Where(cc=> cc.ID == c).FirstOrDefault();
                        if (country != null)
                            keyissue.Countries.Add(country);
                    }
                }

                DbContext.SaveChanges();

                if (keyissue.ID > 0 && Images != null && Images.Count() > 0 && Images.Where(i => i != null).Count() > 0)
                {
                    AddKeyIssueImages(keyissue.ID, Images);
                }

                if (Files != null && Files.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    string file_url = fs.SaveFiles(Files);
                    keyissue.FileURL = file_url;
                }
                DbContext.SaveChanges();

                return keyissue.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool AddKeyIssueImages(int ID, IEnumerable<HttpPostedFileBase> Images)
        {
            KeyIssue keyissue = DbContext.KeyIssues.Where(m => m.ID == ID).FirstOrDefault();

            if (keyissue == null)
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

                            file_name = (ID + "-key-" + image.FileName).Replace(" ", "-");


                            System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                            //save Original Image
                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                            //Save Thumnails
                            foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                                ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, IS.Width, IS.Height);

                            //Update the DB value
                            keyissue.FeatureImageURL = ImageService.GetImagesDirectory() + file_name;
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

        public KeyIssue GetKeyIssue(int id)
        {
            KeyIssue A = DbContext.KeyIssues.Where(a => a.ID == id).Include(a => a.Tags).Include(a => a.Countries).FirstOrDefault();

            if (A.FeatureImageURL != null && A.FeatureImageURL != "")
            {
                int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Width;
                int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Height;
                A.FeatureImageURL = ImageService.GenerateImageFullPath(A.FeatureImageURL, img_width.ToString(), img_height.ToString());
            }
            return A;
        }

        public KeyIssueLink GetSimpleKeyIssue(int id)
        {
            KeyIssue A = DbContext.KeyIssues.Where(a => a.ID == id).FirstOrDefault();
            if (A == null)
                return null;
            return new KeyIssueLink() {FriendlyURL = A.FriendlyURL,Title = A.Title, ID= A.ID };
        }

        public KeyIssueLink GetHomeFeaturedVideo(string language)
        {
            List<string> langs = new List<string>();
            if (website_languages.ContainsKey(language))
                langs = website_languages[language];

            KeyIssue K = DbContext.KeyIssues.Where(k => langs.Contains(k.Language) && k.IsOnline && k.IsHomeVideoFeatured && k.FeatureVideoLink != null && k.FeatureVideoLink != "").Include(k => k.Source).OrderByDescending(k => k.PublishDate).FirstOrDefault();

            if (K == null)
                return null;


            return new KeyIssueLink() { 
                FriendlyURL = K.FriendlyURL,
                ID = K.ID,
                ImageURL = K.FeatureImageURL,
                SourceName = K.Source == null ? "" : (language == "ar" ? K.Source.NameAr : K.Source.Name),
                Title = K.Title,
                VideoUrl = K.FeatureVideoLink
            };
        }

        public KeyIssueLink GetTranslatedKeyIssue(int original_id)
        {
            KeyIssue A = DbContext.KeyIssues.Where(a => a.OriginalId == original_id).FirstOrDefault();

            if (A == null)
                return null;

            return new KeyIssueLink() { FriendlyURL = A.FriendlyURL, Title = A.Title, ID = A.ID };
        }

        public void KeyIssueViewed(int id ,string language)
        {
            KeyIssue keyisuue = DbContext.KeyIssues.Where(k => k.ID == id).FirstOrDefault();

            if (keyisuue == null)
                return;

           
            keyisuue.Views++;

            DbContext.SaveChanges();
        }

        public KeyIssueViewModel GetKeyIssueByURL(string url)
        {
            KeyIssue keyissue = DbContext.KeyIssues.Where(k => k.FriendlyURL == url ).Include(k => k.Topic).Include(k => k.Author).Include(k => k.Source).Include(k => k.Tags).FirstOrDefault();

            if (keyissue == null)
                return null;

            KeyIssueViewModel view_model = new KeyIssueViewModel() { KeyIssue = keyissue };

            List<string> tags =  keyissue.Tags.Select(t=> t.Name).ToList();
            //Related KeyIssues
            view_model.RelatedReading = DbContext.KeyIssues.Include(k => k.Tags).Where(k => k.IsOnline && k.Language == keyissue.Language && k.ID != keyissue.ID && k.Tags.Where(t => tags.Any(tt => t.Name.Contains(tt))).Count() > 0).
                OrderBy(r => Guid.NewGuid()).
                Take(2).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    Title = k.Title
                }).ToList();

            //Rcommended Key Issues
            view_model.RecommendedReading = DbContext.KeyIssues.Where(k => k.IsOnline && k.Language == keyissue.Language && k.IsFeatured && k.ID != keyissue.ID).
                OrderByDescending(k => k.PublishDate).
                Take(10).
                OrderBy(r => Guid.NewGuid()).
                Take(2).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    Title = k.Title
                }).ToList();

            if (keyissue.OriginalId != null)
                view_model.Translation = GetSimpleKeyIssue(keyissue.OriginalId.Value);
            else
                view_model.Translation = GetTranslatedKeyIssue(keyissue.ID);

            return view_model;
        }

        public int UpdateKeyIssue(KeyIssue keyissue, List<string> ArabicTags, List<string> EnglishTags, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Files = null,List<int> Countries = null)
        {
            KeyIssue A = DbContext.KeyIssues.Where(a => a.ID == keyissue.ID).Include(a => a.Tags).Include(a=> a.Countries).FirstOrDefault();

            if (A == null)
                return -1;

            A.AuthorID = keyissue.AuthorID;
            A.Description = keyissue.Description;
            A.FeatureVideoLink = keyissue.FeatureVideoLink;
            A.FriendlyURL = keyissue.FriendlyURL;
            A.IsOnline = keyissue.IsOnline;
            A.MetaDescription = keyissue.MetaDescription;
            A.MetaTitle = keyissue.MetaTitle;
            A.PublishDate = keyissue.PublishDate;
            A.SourceID = keyissue.SourceID;
            A.Title = keyissue.Title;
            A.TopicID = keyissue.TopicID;
            A.IsFeatured = keyissue.IsFeatured;
            A.InScroller = keyissue.InScroller;
            A.TypeID = keyissue.TypeID;
            A.OriginalId = keyissue.OriginalId;
            A.Language = keyissue.Language;
            A.IsHomeVideoFeatured = keyissue.IsHomeVideoFeatured;

            try
            {
                foreach (Tag tag in A.Tags.ToList())
                    DbContext.Tags.Remove(tag);

                A.Tags = new List<Tag>();
                if (ArabicTags != null)
                    foreach (string t in ArabicTags)
                        A.Tags.Add(new Tag() { Name = t, Language = "ar" });
                if (EnglishTags != null)
                    foreach (string t in EnglishTags)
                        A.Tags.Add(new Tag() { Name = t, Language = "en" });
                DbContext.SaveChanges();

                foreach (Country country in A.Countries.ToList())
                    A.Countries.Remove(country);

                if (Countries != null)
                {
                    A.Countries = new List<Country>();
                    foreach (int c in Countries)
                    {
                        Country country = DbContext.Countries.Where(cc => cc.ID == c).FirstOrDefault();
                        if (country != null)
                            A.Countries.Add(country);
                    }
                }
                DbContext.SaveChanges();

                if (A.ID > 0 && Images != null && Images.Where(i => i != null).Count() > 0)
                {
                    if (A.FeatureImageURL != null && A.FeatureImageURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.FeatureImageURL));
                        foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                            ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.FeatureImageURL), IS.Width.ToString(), IS.Height.ToString());
                    }
                    AddKeyIssueImages(A.ID, Images);
                }

                if (Files != null && Files.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();
                    if (A.FileURL != null && A.FileURL != "")
                    {
                        fs.DeleteFile(A.FileURL);
                    }

                    string file_url = fs.SaveFiles(Files);
                    A.FileURL = file_url;
                    DbContext.SaveChanges();
                }

                return A.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public bool DeleteKeyissueImage(int id)
        {
            KeyIssue A = DbContext.KeyIssues.Where(a => a.ID == id).FirstOrDefault();

            if (A == null)
                return false;

            try
            {
                if (A.FeatureImageURL != null && A.FeatureImageURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.FeatureImageURL));
                    foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.FeatureImageURL), IS.Width.ToString(), IS.Height.ToString());

                    A.FeatureImageURL = null;
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteKeyIssue(int id)
        {
            KeyIssue a = DbContext.KeyIssues.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;

            DbContext.KeyIssues.Remove(a);

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

        public SearchViewModel GetSearchResult(
            string search_key,                             
            int? topicId, 
            int page, 
            int page_size, 
            int? authorId, 
            int? sourceId, 
            string orderby,
            int? typeId,
            int? countryId,
            string language,
            DateTime? fromDate,
            DateTime? toDate,
            bool isText,
            bool isPDF,
            bool isVideo,
            bool isInNews = false
            )
        {
            SearchViewModel result = new SearchViewModel();

            IQueryable<KeyIssue> res = DbContext.KeyIssues.Where(a => a.IsOnline);

            if (isInNews)
                res = res.Where(k => k.IsInNews == true);

            List<string> keywords = new List<string>();
            //search key
            if (search_key != null && search_key != "")
            {
                keywords = search_key.Trim().Split(',').ToList();
                res = res.Where(a => keywords.Any(k=> a.Title.Contains(k)) || //title contains any keywords
                    keywords.Any(k=> a.Description.Contains(k)) || //description contains any keywords
                    a.Tags.Where(t => keywords.Any(k=> t.Name.Contains(k))).Count() > 0); //any tag contains any keyword
            }
            //topic
            if (topicId != null)
                res = res.Where(a => a.TopicID == topicId);

            List<string> langs = new List<string>();
           
            //language
            if (language != null && language != "")
            {
               if (website_languages.ContainsKey(language))
                    langs = website_languages[language];
               res = res.Where(a => langs.Contains(a.Language));
            }
                
            //author
            if (authorId != null)
                res = res.Where(a => a.AuthorID == authorId);
            //Source
            if (sourceId != null)
                res = res.Where(a => a.SourceID == sourceId);
            //Type
            if (typeId != null)
                res = res.Where(a => a.TypeID == typeId);
            //Country
            if (countryId != null)
                res = res.Where(a => a.Countries.Where(c=> c.ID == countryId).Count() > 0);
            //From Date
            if(fromDate !=null)
                res = res.Where(a => DbFunctions.CreateDateTime(a.PublishDate.Year, a.PublishDate.Month, a.PublishDate.Day, a.PublishDate.Hour, a.PublishDate.Minute, a.PublishDate.Second) >= fromDate.Value);
            //To Date
            if (toDate != null)
                res = res.Where(a => DbFunctions.CreateDateTime(a.PublishDate.Year, a.PublishDate.Month, a.PublishDate.Day, a.PublishDate.Hour, a.PublishDate.Minute, a.PublishDate.Second) <= toDate.Value);
            //Is Video
            res = res.Where(a => (isVideo && a.FeatureVideoLink != null && a.FeatureVideoLink != "")
                || (isPDF && a.FileURL != null && a.FileURL != "")
                || (isText && a.Description != null && a.Description != ""));
            
            //orderby
           
            if (orderby == "relevance")
            {
                res = res.OrderByDescending(a => keywords.Count(k=> a.Title.Contains(k))).
                    ThenByDescending(a=> a.Tags.Where(t => keywords.Any(k=> t.Name.Contains(k))).Count()).
                    ThenByDescending(a => keywords.Count(k => a.Description.Contains(k)));
            }
            else if (orderby == "popular")
            {
                res = res.OrderByDescending(a => a.Views);
            }
            else
            { res = res.OrderByDescending(a => a.PublishDate); }

            

            result.TotalCount = res.Count();

            if (result.TotalCount > (page * page_size))
            {
                result.ShowNext = true;
            }
            if (result.TotalCount > page_size && page > 1)
            {
                result.ShowPrev = true;
            }

            result.DisplayFrom = ((page - 1) * page_size) + 1;
            result.DisplayTo = (page) * page_size;
            if (result.TotalCount < result.DisplayTo)
                result.DisplayTo = result.TotalCount;

            result.KeyIssues = res.Include(a => a.Author).Include(a => a.Source).Skip((page - 1) * page_size).Take(page_size).Select(a => new SearchItemViewModel()
                {
                    Author = a.Author.FullName,
                    AuthorId = a.AuthorID,
                    FriendlyURL = a.FriendlyURL,
                    ID = a.ID,
                    ImageURL = (a.FeatureImageURL == null || a.FeatureImageURL == "")?a.Source.ImageURL:a.FeatureImageURL,
                    PublishDate = a.PublishDate,
                    Title = a.Title,
                    Source = a.Source.Name,
                    SourceID = a.SourceID
                }).ToList();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SearchThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SearchThumb").FirstOrDefault().Height;

            foreach (SearchItemViewModel s in result.KeyIssues)
            {
                s.ImageURL = ImageService.GenerateImageFullPath(s.ImageURL, img_width.ToString(), img_height.ToString());
            }

            return result;
        }

        public KeyIssuesViewModel GetKeyIssuesViewModel(string language ="en")
        {
            KeyIssuesViewModel view_model = new KeyIssuesViewModel();

            view_model.Topics = DbContext.Topics.ToList();

            List<string> langs = new List<string>();
            if(website_languages.ContainsKey(language))
                langs = website_languages[language];

            //featured article
            view_model.FeatureArticle = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && (k.IsInNews == null || k.IsInNews == false) && k.IsFeatured && k.Description != null && k.Description != "" && (k.FeatureVideoLink == null || k.FeatureVideoLink == "")).Include(k => k.Source).
                OrderByDescending(k => k.PublishDate).
                Take(5).
                OrderBy(r => Guid.NewGuid()).
                Take(1).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    ImageURL = (k.FeatureImageURL == null || k.FeatureImageURL == "") ? (k.Source == null ? "" : k.Source.ImageURL) : k.FeatureImageURL,
                    IsVideo = (k.FeatureVideoLink != null && k.FeatureVideoLink != ""),
                    Title = k.Title
                }).FirstOrDefault();

            //featured video
            view_model.FeatureVideo = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && (k.IsInNews == null || k.IsInNews == false) && k.IsFeatured && k.FeatureVideoLink != null && k.FeatureVideoLink != "").Include(k => k.Source).
                OrderByDescending(k => k.PublishDate).
                Take(5).
                OrderBy(r => Guid.NewGuid()).
                Take(1).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    ImageURL = (k.FeatureImageURL == null || k.FeatureImageURL == "") ? (k.Source == null ? "" : k.Source.ImageURL) : k.FeatureImageURL,
                    IsVideo = (k.FeatureVideoLink != null && k.FeatureVideoLink != ""),
                    Title = k.Title
                }).FirstOrDefault();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Height;
           if(view_model.FeatureArticle != null)
           {
               string img = view_model.FeatureArticle.ImageURL;
               view_model.FeatureArticle.ImageURL = ImageService.GenerateImageFullPath(img, img_width.ToString(), img_height.ToString());
            }

           if (view_model.FeatureVideo != null)
           {
               string img = view_model.FeatureVideo.ImageURL;
               view_model.FeatureVideo.ImageURL = ImageService.GenerateImageFullPath(img, img_width.ToString(), img_height.ToString());
           }


           //Tweet article
           view_model.TweetArticle = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && (k.IsInNews == null || k.IsInNews == false) && k.Description != null && k.Description != "" && k.Description.Contains("<blockquote>")).
               OrderByDescending(k => k.PublishDate).
               Take(15).
               OrderBy(r => Guid.NewGuid()).
               Take(1).
               Select(k => new KeyIssueLink()
               {
                   ID = k.ID,
                   FriendlyURL = k.FriendlyURL,
                   Title = k.Title,
                   Quote = k.Description
               }).FirstOrDefault();

           if (view_model.TweetArticle != null)
               view_model.TweetArticle.Quote = PullQuote(view_model.TweetArticle.Quote);

            return view_model;
        }

        public string PullQuote(string description)
        {
            MatchCollection MC = Regex.Matches(description, @"(<blockquote>)(.*?)(</blockquote>)", RegexOptions.Singleline);
            
            if(MC.Count > 0)
            {
                string result = MC[0].Value;

                result = result.Replace("<blockquote><p>", "");
                result = result.Replace("</p></blockquote>", "");

                result = result.Replace("<blockquote>\r\n<p>", "");
                result = result.Replace("</p>\r\n</blockquote>", "");

                return result;
            }
            return "";
        }

        public List<KeyIssueLink> GetSliderKeyIssues(string language = "")
        {
            List<string> langs = new List<string>();
            if (website_languages.ContainsKey(language))
                langs = website_languages[language];

            List<KeyIssueLink> latest = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && k.FeatureImageURL != null && k.FeatureImageURL != "" && (k.IsInNews == null || k.IsInNews == false)).Include(k => k.Source).
                OrderByDescending(k => k.PublishDate).
                Take(15).
                Select(k => new KeyIssueLink() 
                { 
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    ImageURL = (k.FeatureImageURL == null || k.FeatureImageURL == "" ) ? (k.Source == null? "" : k.Source.ImageURL):k.FeatureImageURL,
                    IsVideo = (k.FeatureVideoLink != null && k.FeatureVideoLink != ""),
                    Title = k.Title,
                    InScroller = k.InScroller,
                    PublishDate = k.PublishDate
                }).ToList();

            List<KeyIssueLink> chosen = DbContext.KeyIssues.Where(k => k.InScroller && langs.Contains(k.Language) && k.FeatureImageURL != null && k.FeatureImageURL != "" && k.IsOnline && (k.IsInNews == null || k.IsInNews == false)).
                OrderByDescending(k => k.PublishDate).
                Take(5).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    ImageURL = (k.FeatureImageURL == null || k.FeatureImageURL == "") ? (k.Source == null ? "" : k.Source.ImageURL) : k.FeatureImageURL,
                    IsVideo = (k.FeatureVideoLink != null && k.FeatureVideoLink != ""),
                    Title = k.Title,
                    InScroller = k.InScroller,
                    PublishDate = k.PublishDate
                }).ToList();

            foreach (KeyIssueLink KI in latest)
            {
                if (chosen.Where(k => k.ID == KI.ID).Count() == 0)
                    chosen.Add(KI);

                if (chosen.Count() == 15)
                    break;
            }

            chosen = chosen.OrderByDescending(k => k.InScroller).ThenByDescending(k => k.PublishDate).ToList();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Height;

            int img_width2 = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Width;
            int img_height2 = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Height;
            foreach (KeyIssueLink KI in chosen)
            {
                string img = KI.ImageURL;
                KI.ImageURL = ImageService.GenerateImageFullPath(img, img_width.ToString(), img_height.ToString());
                KI.ImageURL2 = ImageService.GenerateImageFullPath(img, img_width2.ToString(), img_height2.ToString());
            }


            return chosen;
        }

        public List<KeyIssueLink> GetLatestNews(string language= "en")
        {
            List<string> langs = new List<string>();
            if (website_languages.ContainsKey(language))
                langs = website_languages[language];

            List<KeyIssueLink> latest = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && k.IsInNews == true).Include(k => k.Source).
                OrderByDescending(k => k.PublishDate).
                Take(5).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    Title = k.Title,
                    PublishDate = k.PublishDate,
                    SourceName= k.Source.Name ,
                    SourceNameAr = k.Source.NameAr,
                    SourceId = k.SourceID
                }).ToList();

            return latest;
        }

        public KeyIssueLink GetFeatureNewsVideo(string language = "en")
        {
            List<string> langs = new List<string>();
            if (website_languages.ContainsKey(language))
                langs = website_languages[language];

            KeyIssueLink latest = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && k.IsInNews == true && k.IsFeatured && (k.FeatureVideoLink != null && k.FeatureVideoLink != "")).Include(k => k.Source).
                OrderByDescending(k => k.PublishDate).
                Take(5).
                OrderBy(r => Guid.NewGuid()).
               Take(1).
                Select(k => new KeyIssueLink()
                {
                    ID = k.ID,
                    FriendlyURL = k.FriendlyURL,
                    ImageURL = (k.FeatureImageURL == null || k.FeatureImageURL == "") ? (k.Source == null ? "" : k.Source.ImageURL) : k.FeatureImageURL,
                    Title = k.Title,
                    PublishDate = k.PublishDate
                }).FirstOrDefault();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "BigThumb").FirstOrDefault().Height;
            if (latest != null)
            {
                string img = latest.ImageURL;
                latest.ImageURL = ImageService.GenerateImageFullPath(img, img_width.ToString(), img_height.ToString());
            }
            return latest;
        }
        
        public List<SourceViewModel> GetMediaSources()
        {
            List<Source> sources = DbContext.Sources.Where(s=> s.KeyIssues.Where(k => k.IsOnline && k.IsInNews).Count() > 0).ToList();
            List<SourceViewModel> result = new List<SourceViewModel>();

            foreach (Source s in sources)
            {
                result.Add(new SourceViewModel()
                {
                    ID = s.ID,
                    Name = s.Name,
                    LogoURL = ImageService.GenerateImageFullPath(s.LogoURL, "150", "80")
                });
            }

            return result;
        }

        public SearchViewModel GetAllPressReleases( string language, int page, int page_size)
        {
            List<string> langs = new List<string>();
            if(website_languages.ContainsKey(language))
                langs = website_languages[language];

            SearchViewModel result = new SearchViewModel();

            IQueryable<KeyIssue> res = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language) && k.IsInNews && k.FileURL != null && k.FileURL != "" && k.Author.FullName == "LOGI");

            res = res.OrderByDescending(a => a.PublishDate);
           result.TotalCount = res.Count();

            if (result.TotalCount > (page * page_size))
            {
                result.ShowNext = true;
            }
            if (result.TotalCount > page_size && page > 1)
            {
                result.ShowPrev = true;
            }

            result.DisplayFrom = ((page - 1) * page_size) + 1;
            result.DisplayTo = (page) * page_size;
            if (result.TotalCount < result.DisplayTo)
                result.DisplayTo = result.TotalCount;

            result.KeyIssues = res.Include(a => a.Author).Include(a => a.Source).Skip((page - 1) * page_size).Take(page_size).Select(a => new SearchItemViewModel()
                {
                    Author = a.Author.FullName,
                    AuthorId = a.AuthorID,
                    FriendlyURL = a.FriendlyURL,
                    ID = a.ID,
                    ImageURL = (a.FeatureImageURL == null || a.FeatureImageURL == "")?a.Source.ImageURL:a.FeatureImageURL,
                    PublishDate = a.PublishDate,
                    Title = a.Title,
                    Source = a.Source.Name,
                    SourceID = a.SourceID
                }).ToList();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SearchThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SearchThumb").FirstOrDefault().Height;

            foreach (SearchItemViewModel s in result.KeyIssues)
            {
                s.ImageURL = ImageService.GenerateImageFullPath(s.ImageURL, img_width.ToString(), img_height.ToString());
            }

            return result;

        }

        public string GetPressRelease(string language)
        {
            List<string> langs = new List<string>();
            if (website_languages.ContainsKey(language))
                langs = website_languages[language];

            KeyIssue latest_release = DbContext.KeyIssues.Where(k => k.IsOnline && langs.Contains(k.Language)  && k.IsInNews && k.FileURL != null && k.FileURL != "" && k.Author.FullName == "LOGI").FirstOrDefault();

            if (latest_release != null)
                return latest_release.FileURL;
            else
                return "";
            //SectionVariable var = DbContext.SectionVariables.Where(v => v.Key == "PressReleasePDF").FirstOrDefault();

            //if (var == null)
            //    return "";
            //else
            //{
            //    return var.Value;
            //}
        }
    }
}