using JobManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace JobManagement.Controllers
{
    public class HomeController : Controller
    {
        private JobManagementDBEntities db = new JobManagementDBEntities();

        public ActionResult Index()
        {
            var latestJobs = db.Jobs.Include(j => j.Category)
                                    .OrderByDescending(j => j.CreatedAt)
                                    .Take(6)
                                    .ToList();
            return View(latestJobs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}