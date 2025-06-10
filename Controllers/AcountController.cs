using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EventManagement.Controllers
{
    public class AccountController : Controller
    {

        private static Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "user1", "pass1" },
            { "user2", "pass2" }
        };
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (users.ContainsKey(username) && users[username] == password)
            {
                Session["Username"] = username;
                return RedirectToAction("Index", "Inventory");
            }
            else
            {
                ViewBag.Error = "Invalid username or Password";
                return View();
            }
        }
    }
}