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
    public class AccountController : Controller
    {
        private string bibPass { get;  set;}

        private string adminPass {get;  set;}

        public AccountController(string biblioPass, string adminisPass) 
        {
            bibPass = biblioPass;
            adminPass = adminisPass;
        }
        public void Bibliothecaris()
        {
            //Bibliothecaris biblio = new Bibliothecaris();
        }

        public void Admin()
        {
            //Admin admin = new Admin();
        }

        public void ComparePasswords(string paswoord)
        {//hash het paswoord
            //string pass = Gebruiker.hashPassword(paswoord);
            //vergelijk pass met paswoorden uit gebruikerslijst in lettertuin
            //if match, ?
        }

        // GET: huidige pagina
        public ActionResult BibLogin()
        {
            //if (ViewBag.Message == bibPass)
            //{
            //    return View();
            //    Bibliothecaris();
            //}
            //else
            //{
            return View();
            //}
        }

        //GET: huidige pagina
        public ActionResult AdminLogin()
        {
            //if (ViewBag.Message == adminPass)
            //{
            //    return View();
            //    Admin();
            //}
            //else
            //{
                return View();
            //}
        }
    }


}