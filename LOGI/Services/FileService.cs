using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class FilesService
    {
        
        #region Methods
        

        public string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public string SaveFiles(IEnumerable<HttpPostedFileBase> uploaded_files)
        {
            string directory_location = HttpContext.Current.Server.MapPath("~" + GetFilesDirectory());
            try
            {
                string file_final_name = "";
                foreach (HttpPostedFileBase f in uploaded_files)
                {
                    if (f != null)
                    {
                        file_final_name = RandomString() + "_" + f.FileName;
                      
                        if (!Directory.Exists(directory_location))
                        {
                            Directory.CreateDirectory(directory_location);
                        }
                      
                        f.SaveAs(directory_location + "/" + file_final_name);
                        return GetFilesDirectory() + file_final_name;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
               
               return "";
            }

        }


        public static string GetFilesDirectory()
        {
            return "/uploaded/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month + "/";
        }


        public bool DeleteFile(string location)
        {
            
           
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~" + location)))
                    File.Delete(HttpContext.Current.Server.MapPath("~" + location));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}