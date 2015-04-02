using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DeKrekelGroup5.Models;

namespace DeKrekelGroup5.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {


        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login(string username)
        {
            return View(new LoginViewModel() { Username = username });
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel logon)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}