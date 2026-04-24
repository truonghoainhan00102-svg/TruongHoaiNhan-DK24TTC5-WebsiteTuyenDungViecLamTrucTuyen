using JobManagement.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace JobManagement.Controllers
{
    public class JobController : Controller
    {
        private JobManagementDBEntities db = new JobManagementDBEntities();
        public ActionResult Index(int? categoryId, string keyword)
        {
            // Dropdown
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", categoryId);
            ViewBag.Keyword = keyword;

            var jobs = db.Jobs.Include(j => j.Category).AsQueryable();

            // Filter theo category
            if (categoryId.HasValue)
            {
                jobs = jobs.Where(j => j.CategoryId == categoryId.Value);
            }

            // Search theo tên job
            if (!string.IsNullOrEmpty(keyword))
            {
                jobs = jobs.Where(j => j.Title.Contains(keyword));
            }

            return View(jobs.ToList());
        }
        [Authorize]
        public ActionResult MyJobs()
        {
            // Ví dụ: lấy theo user đang login (tùy bạn lưu kiểu gì)
            var userEmail = Session["Email"]?.ToString();

            var jobs = db.Jobs
                .Include(j => j.Category)
                //.Where(j => j.CreatedBy == userEmail) // nếu có cột này thì dùng
                .ToList();

            return View(jobs);
        }
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                TempData["Message"] = "Đăng tin tuyển dụng thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
            return View(job);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Job job = db.Jobs.Find(id);
            if (job == null) return HttpNotFound();

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
            return View(job);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
            return View(job);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Job job = db.Jobs.Find(id);
            if (job == null) return HttpNotFound();

            return View(job);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();

            TempData["Message"] = "Đã xóa công việc!";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Job job = db.Jobs.Include(j => j.Category).SingleOrDefault(j => j.Id == id);

            if (job == null) return HttpNotFound();

            return View(job);
        }
    }
}