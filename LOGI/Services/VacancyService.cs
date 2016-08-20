using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class VacancyService
    {
        private ApplicationDbContext DbContext;

        public VacancyService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<VacancyViewModel> GetVacancies(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<Vacancy> result;
            if (order_by == "JobNumber")
            {
                if (order_dir == "desc")
                    result = DbContext.Vacancies.OrderByDescending(a => a.JobNumber);
                else
                    result = DbContext.Vacancies.OrderBy(a => a.JobNumber);
            }
            else if (order_by == "Title")
            {
                if (order_dir == "desc")
                    result = DbContext.Vacancies.OrderByDescending(a => a.Title);
                else
                    result = DbContext.Vacancies.OrderBy(a => a.Title);
            }
            else if (order_by == "RoleSummary")
            {
                if (order_dir == "desc")
                    result = DbContext.Vacancies.OrderByDescending(a => a.RoleSummary);
                else
                    result = DbContext.Vacancies.OrderBy(a => a.RoleSummary);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Vacancies.OrderByDescending(a => a.DeadLine);
                else
                    result = DbContext.Vacancies.OrderBy(a => a.DeadLine);
            }

            if (search_key != "")
                result = result.Where(a => a.Title.Contains(search_key));

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            List<VacancyViewModel> final = new List<VacancyViewModel>();

            foreach (Vacancy va in result.ToList())
            {
                final.Add(
                    new VacancyViewModel() {
                        ID = va.ID,
                        DeadLine = va.DeadLine.ToString("dd/MMMM/yyyy"),
                        JobNumber = va.JobNumber,
                        RoleSummary = va.RoleSummary,
                        Title = va.Title
                    });
            }

            return final;
        }

        public List<VacancyApplicationViewModel> GetVacancyApplication(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc",int vacancy_id = 0)
        {
            IQueryable<VacancyApplication> result = DbContext.VacancyApplications;

            if (vacancy_id != 0)
                result = result.Where(v => v.Vacancy.ID == vacancy_id);

            if (order_by == "FullName")
            {
                if (order_dir == "desc")
                    result = result.OrderByDescending(a => a.FullName);
                else
                    result = result.OrderBy(a => a.FullName);
            }
            else if (order_by == "Email")
            {
                if (order_dir == "desc")
                    result = result.OrderByDescending(a => a.Email);
                else
                    result = result.OrderBy(a => a.Email);
            }
            else if (order_by == "Phone")
            {
                if (order_dir == "desc")
                    result = result.OrderByDescending(a => a.Phone);
                else
                    result = result.OrderBy(a => a.Phone);
            }
            else if (order_by == "Country")
            {
                if (order_dir == "desc")
                    result = result.OrderByDescending(a => a.Country);
                else
                    result = result.OrderBy(a => a.Country);
            }
            else
            {
                if (order_dir == "desc")
                    result = result.OrderByDescending(a => a.ApplyDate);
                else
                    result = result.OrderBy(a => a.ApplyDate);
            }

            if (search_key != "")
                result = result.Where(a => a.FullName.Contains(search_key) || a.Email.Contains(search_key));

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            List<VacancyApplicationViewModel> final = new List<VacancyApplicationViewModel>();

            foreach (VacancyApplication va in result.ToList())
            {
                final.Add(
                    new VacancyApplicationViewModel()
                    {
                        ID = va.ID,
                        ApplyDate = va.ApplyDate.ToString("dd/MMMM/yyyy"),
                        Country = va.Country,
                        Email = va.Email,
                        FullName = va.FullName,
                        Message = va.Message,
                        Phone = va.Phone,
                        ResumeURL = va.ResumeURL
                    });
            }

            return final;
        }

        public VacancyApplication GetVacancyApplicationById(int id)
        {
            VacancyApplication vac_app = DbContext.VacancyApplications.Where(v => v.ID == id).FirstOrDefault();

            return vac_app;
        }

        public int AddVacancy(Vacancy vacancy)
        {
            DbContext.Vacancies.Add(vacancy);
            try
            {
                DbContext.SaveChanges();

                //generate the job number
                Vacancy latest = DbContext.Vacancies.Where(v => v.JobNumber != null && v.JobNumber != "").OrderByDescending(v => v.ID).FirstOrDefault();
                if (latest == null)
                    vacancy.JobNumber = GenerateJobNubmer("");
                else
                    vacancy.JobNumber = GenerateJobNubmer(latest.JobNumber);
                
                DbContext.SaveChanges();
                
                return vacancy.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public string GenerateJobNubmer(string latest)
        {
            if(latest == "")
                return DateTime.Now.Year.ToString().Substring(2, 2)+"-001";
            else
                return DateTime.Now.Year.ToString().Substring(2, 2) + "-" + (Convert.ToInt32(latest.Substring(3,3))+1).ToString().PadLeft(3,'0');

        }

        public Vacancy GetVacancy(int id)
        {
            Vacancy V = DbContext.Vacancies.Where(a => a.ID == id).FirstOrDefault();

            return V;
        }

        public int UpdateVacancy(Vacancy vacancy)
        {
            Vacancy A = DbContext.Vacancies.Where(a => a.ID == vacancy.ID).FirstOrDefault();

            if (A == null)
                return -1;

            A.DeadLine = vacancy.DeadLine;
            A.DesiredCharacteristics = vacancy.DesiredCharacteristics;
            A.DesiredCharacteristicsAr = vacancy.DesiredCharacteristicsAr;
            A.EssentialResponsibilities = vacancy.EssentialResponsibilities;
            A.EssentialResponsibilitiesAr = vacancy.EssentialResponsibilitiesAr;
            A.OutcomeAr = vacancy.OutcomeAr;
            A.Outcome = vacancy.Outcome;
            A.ProjectDeadline = vacancy.ProjectDeadline;
            A.ProjectDeadlineAr = vacancy.ProjectDeadlineAr;
            A.Qualifications = vacancy.Qualifications;
            A.QualificationsAr = vacancy.QualificationsAr;
            A.RoleSummary = vacancy.RoleSummary;
            A.RoleSummaryAr = vacancy.RoleSummaryAr;
            A.Salary = vacancy.Salary;
            A.SalaryAr = vacancy.SalaryAr;
            A.Title = vacancy.Title;
            A.TitleAr = vacancy.TitleAr;

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

        public bool DeleteVacancy(int id)
        {
            Vacancy a = DbContext.Vacancies.Where(aa => aa.ID == id).Include(v=> v.VacancyApplications).FirstOrDefault();

            if (a == null)
                return false;

            foreach (VacancyApplication va in a.VacancyApplications.ToList())
            {
                DeleteVacancyApplication(va.ID);
            }

            DbContext.Vacancies.Remove(a);

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

        public bool DeleteVacancyApplication(int id)
        {
            VacancyApplication va = DbContext.VacancyApplications.Where(v => v.ID == id).FirstOrDefault();

            if (va == null)
                return false;

            try
            {
                if(va.ResumeURL !=null && va.ResumeURL != "" )
                {
                    FilesService fs = new FilesService();
                    fs.DeleteFile(va.ResumeURL);
                }

                DbContext.VacancyApplications.Remove(va);
                DbContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public JoinLOGIViewModel GetJoinLOGIViewModel()
        {
            JoinLOGIViewModel result = new JoinLOGIViewModel();

            List<Vacancy> vacs = DbContext.Vacancies.Where(a => DbFunctions.CreateDateTime(a.DeadLine.Year, a.DeadLine.Month, a.DeadLine.Day, a.DeadLine.Hour, a.DeadLine.Minute, a.DeadLine.Second) >= DateTime.Now).ToList();

            result.Vacancies = new List<VacancyViewModel>();
            foreach(Vacancy va in vacs)
            {
                result.Vacancies.Add(
                    new VacancyViewModel()
                    {
                       DeadLine = va.DeadLine.ToString("dd/MMMM/yyyy"),
                       ID  = va.ID,
                       JobNumber = va.JobNumber,
                       RoleSummary = va.RoleSummary,
                       Title = va.Title,
                       TitleAr = va.TitleAr,
                       RoleSummaryAr = va.RoleSummaryAr
                    });
            }
            return result;
        }

        public int AppplyToVacancy(int vacancy_id, VacancyApplication application, IEnumerable<HttpPostedFileBase> resume = null)
        {
            Vacancy vacancy = DbContext.Vacancies.Where(v => v.ID == vacancy_id).FirstOrDefault();

            if (vacancy == null)
                return -1;

            if (vacancy.VacancyApplications == null)
                vacancy.VacancyApplications = new List<VacancyApplication>();

            application.ApplyDate = DateTime.Now;

            vacancy.VacancyApplications.Add(application);
            try
            {
                DbContext.SaveChanges();

                if (resume != null && resume.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    string file_url = fs.SaveFiles(resume);
                    application.ResumeURL = file_url;
                }
                DbContext.SaveChanges();

                return application.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int AppplyAsExpert(Expert expert, IEnumerable<HttpPostedFileBase> resume = null)
        {
            expert.ApplyDate = DateTime.Now;

            DbContext.Experts.Add(expert);
            try
            {
                DbContext.SaveChanges();

                if (resume != null && resume.Where(f => f != null).Count() > 0)
                {
                    FilesService fs = new FilesService();

                    string file_url = fs.SaveFiles(resume);
                    expert.ResumeURL = file_url;
                }
                DbContext.SaveChanges();

                return expert.ID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<ExpertViewModel> GetExperts(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<Expert> result;
            if (order_by == "ApplyDate")
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.ApplyDate);
                else
                    result = DbContext.Experts.OrderBy(a => a.ApplyDate);
            }
            else if (order_by == "FullName")
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.FullName);
                else
                    result = DbContext.Experts.OrderBy(a => a.FullName);
            }
            else if (order_by == "Email")
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.Email);
                else
                    result = DbContext.Experts.OrderBy(a => a.Email);
            }
            else if (order_by == "Expertise")
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.Expertise);
                else
                    result = DbContext.Experts.OrderBy(a => a.Expertise);
            }
            else if (order_by == "Education")
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.Education);
                else
                    result = DbContext.Experts.OrderBy(a => a.Education);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.Experts.OrderByDescending(a => a.ApplyDate);
                else
                    result = DbContext.Experts.OrderBy(a => a.ApplyDate);
            }

            if (search_key != "")
                result = result.Where(a => a.FullName.Contains(search_key));

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            List<ExpertViewModel> final = new List<ExpertViewModel>();

            foreach (Expert va in result.ToList())
            {
                final.Add(
                    new ExpertViewModel()
                    {
                        ID = va.ID,
                        ApplyDate = va.ApplyDate.ToString("dd/MMMM/yyyy"),
                          Education = va.Education,
                          Email = va.Email,
                          Expertise = va.Expertise,
                          FullName = va.FullName,
                    });
            }

            return final;
        }

        public Expert GetExpert(int id)
        {
            Expert E = DbContext.Experts.Where(a => a.ID == id).FirstOrDefault();

            return E;
        }

        public bool DeleteExpert(int id)
        {
            Expert a = DbContext.Experts.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;
            
            

            try
            {
                if (a.ResumeURL != null && a.ResumeURL != "")
                {
                    FilesService fs = new FilesService();
                    fs.DeleteFile(a.ResumeURL);
                }

                DbContext.Experts.Remove(a);

                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}