using JobManagement.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobManagement.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private JobManagementDBEntities db = new JobManagementDBEntities();

        public ActionResult Apply(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Job job = db.Jobs.Find(id);
            if (job == null) return HttpNotFound();

            return View(job);
        }
        [Authorize]
        public ActionResult Applicants(int jobId)
        {
            var applications = db.Applications
                .Where(a => a.JobId == jobId)
                .Select(a => new ApplicantViewModel
                {
                    Id = a.Id,
                    FullName = a.User.FullName,
                    Email = a.User.Email,
                    CoverLetter = a.CoverLetter,
                    CVPath = a.CVPath,
                    ApplyDate = a.ApplyDate,
                    Status = a.Status
                })
                .ToList();

            ViewBag.JobId = jobId;

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(int id, string coverLetter, HttpPostedFileBase cvFile)
        {
            string userEmail = User.Identity.Name;
            var currentUser = db.Users.SingleOrDefault(u => u.Email == userEmail);

            if (currentUser == null) return RedirectToAction("Login", "Account");

            string savedFileName = "";

            if (cvFile != null && cvFile.ContentLength > 0)
            {
                string directoryPath = Server.MapPath("~/Uploads/CVs/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(cvFile.FileName);
                string fullPath = Path.Combine(directoryPath, fileName);

                cvFile.SaveAs(fullPath);
                savedFileName = "/Uploads/CVs/" + fileName;
            }

            Application app = new Application()
            {
                JobId = id,
                UserId = currentUser.Id,
                CoverLetter = coverLetter,
                CVPath = savedFileName,
                ApplyDate = DateTime.Now,
                Status = "Chờ duyệt"
            };

            db.Applications.Add(app);
            db.SaveChanges();

            TempData["Message"] = "Ứng tuyển thành công! Vui lòng chờ nhà tuyển dụng liên hệ.";
            return RedirectToAction("Index", "Home");
        }
    }
}