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
    public class AdminTopicController : Controller
    {
         #region Privates
        private ApplicationDbContext _dbContext;
        private TopicService topicService;
        #endregion

        #region Constructor
        public AdminTopicController()
        {
            topicService = new TopicService(DBContext);

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
        public ActionResult New()
        {
            ViewBag.MainNav = Navigator.Main.TOPIC;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(Topic topic)
        {
            ViewBag.MainNav = Navigator.Main.TOPIC;

            if (ModelState.IsValid)
            {
                int new_id = topicService.AddTopic(topic);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Topic Added Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Topic Failed To Add";
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MainNav = Navigator.Main.TOPIC;
            Topic view_model = topicService.GetTopic(id);
            return View(view_model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(Topic topic)
        {
            ViewBag.MainNav = Navigator.Main.TOPIC;

            if (ModelState.IsValid)
            {
                int new_id = topicService.UpdateTopic(topic);

                if (new_id > 0)
                {
                    TempData["SuccessMessage"] = "Topic Updated Successfully";
                    return RedirectToAction("Edit", new { id = new_id });
                }
                else
                    TempData["ErrorMessage"] = "Topic Failed To Update";
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (topicService.DeleteTopic(id))
                return RedirectToAction("List");
            else
                return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult List()
        {
            ViewBag.MainNav = Navigator.Main.TOPIC;
            return View();
        }

        public JsonResult _TopicsList(int draw, int start = 0, int length = 2, int CategoryId = 0)
        {
            string search = Request.Form["search[value]"];
            string order_by = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"];
            string order_dir = Request.Form["order[0][dir]"];

            DataTableViewModel result = new DataTableViewModel();
            result.draw = draw;
            int total_count;

            result.data = topicService.GetTopics(out total_count, start, length, search, order_by, order_dir);

            result.recordsTotal = total_count;
            result.recordsFiltered = total_count;
            return Json(result);
        }
        #endregion
    }
}