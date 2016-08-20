using LOGI.Areas.Admin.Models;
using LOGI.Models;
using LOGI.Models.ViewModels;
using LOGI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Areas.admin.Controllers
{
     [Authorize(Roles = "Admin")]
    public class AdminTeamController : Controller
    {
        private ApplicationDbContext _dbContext;
        private TeamService teamService;
        public AdminTeamController()
        {
            teamService = new TeamService(DBContext);
            
        }

        public AdminTeamController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            teamService = new TeamService(DBContext);
        }

      
         public ActionResult New()
         {
             ViewBag.MainNav = Navigator.Main.ABOUT;
             ViewBag.SubNav = Navigator.Sub.TEAM;
             return View();
         }

         [HttpPost]
         [ValidateInput(false)]
         public ActionResult New(TeamMember member, IEnumerable<HttpPostedFileBase> Images)
         {
             ViewBag.MainNav = Navigator.Main.ABOUT;
             ViewBag.SubNav = Navigator.Sub.TEAM;
           
             if (ModelState.IsValid)
             {
                 if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                 {
                     TempData["ErrorMessage"] = "Member Failed To Add, Invalid Image File";
                 }
                 else
                 {
                     int new_id = teamService.AddNewMember(member, Images);

                     if (new_id > 0)
                     {
                         TempData["SuccessMessage"] = "Member Added Successfully";
                         return RedirectToAction("Edit", new { id = new_id });
                     }
                     else
                         TempData["ErrorMessage"] = "Member Failed To Add";
                 }
             }

             return View(member);
         }

         public ActionResult Edit(int id)
         {
             ViewBag.MainNav = Navigator.Main.ABOUT;
             ViewBag.SubNav = Navigator.Sub.TEAM;

             TeamMember view_model = teamService.GetTeamMember(id);
             return View(view_model);
         }

         [HttpPost]
         [ValidateInput(false)]
         public ActionResult Edit(TeamMember member, IEnumerable<HttpPostedFileBase> Images)
         {
             ViewBag.MainNav = Navigator.Main.ABOUT;
             ViewBag.SubNav = Navigator.Sub.TEAM;
            
             if (ModelState.IsValid)
             {
                 if (Images != null && Images.Count() > 0 && !ImageService.IsValid(Images))
                 {
                     TempData["ErrorMessage"] = "Member Failed To Edit, Invalid Image File";
                 }
                 else
                 {
                     int new_id = teamService.UpdateMember(member, Images);

                     if (new_id > 0)
                     {
                         TempData["SuccessMessage"] = "Member Edited Successfully";
                         return RedirectToAction("Edit", new { id = new_id });
                     }
                     else
                         TempData["ErrorMessage"] = "Member Failed To Edited";
                 }
             }
             return View();
         }

         public ActionResult Delete(int id)
         {
             if(teamService.DeleteMember(id))
                return RedirectToAction("List");
             else
                 return RedirectToAction("Edit", new { id = id });
         }

        public ActionResult List(int CategoryId = 0)
        {
            ViewBag.MainNav = Navigator.Main.ABOUT;
            ViewBag.SubNav = Navigator.Sub.TEAM;
            return View();
        }

        public JsonResult _MembersList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;
           
            result.data = teamService.GetTeamMembers(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }

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
    }
}