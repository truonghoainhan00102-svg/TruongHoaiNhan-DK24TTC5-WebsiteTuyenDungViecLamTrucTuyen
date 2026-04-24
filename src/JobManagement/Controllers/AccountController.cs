using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using JobManagement.Models;

namespace JobManagement.Controllers
{
    public class AccountController : Controller
    {
        private JobManagementDBEntities db = new JobManagementDBEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);

                Session["FullName"] = user.FullName;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng!";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}