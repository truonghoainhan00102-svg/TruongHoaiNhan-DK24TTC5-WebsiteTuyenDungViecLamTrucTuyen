using JobManagement.Models;
using System.Linq;
using System.Web.Mvc;

[Authorize]
public class CategoryController : Controller
{
    private JobManagementDBEntities db = new JobManagementDBEntities();

    // Danh sách
    public ActionResult Index()
    {
        return View(db.Categories.ToList());
    }

    // Create GET
    public ActionResult Create()
    {
        return View();
    }

    // Create POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // Edit GET
    public ActionResult Edit(int? id)
    {
        if (id == null) return HttpNotFound();

        var category = db.Categories.Find(id);
        if (category == null) return HttpNotFound();

        return View(category);
    }

    // Edit POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // Delete
    public ActionResult Delete(int? id)
    {
        if (id == null) return HttpNotFound();

        var category = db.Categories.Find(id);
        if (category == null) return HttpNotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        var category = db.Categories.Find(id);
        db.Categories.Remove(category);
        db.SaveChanges();

        return RedirectToAction("Index");
    }
}