
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
using DeKrekelGroup5.ViewModel;
using DeKrekelGroup5.Models.Domain;
using System.Collections.Generic;

namespace DeKrekelGroup5.Controllers
{
   
    public class LoginController : Controller
    {
        private IEnumerable<Gebruiker> gebruikers;

        public LoginController(IGebruikerRepository gebruikersrep, Gebruiker gebruiker)
        {
            gebruikers = gebruikersrep.GetGebruikers();
        }

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login(string username)
        {

            return View(new LoginViewModel() { Username = username });
        }

        // GET: /Account/BibliothecarisLogin
        [HttpGet]
        public ActionResult BibliothecarisLogin(string username)
        {

            return View(new LoginViewModel() { Username = username });
        }

        // GET: /Account/AdminLogin
        [HttpGet]
        public ActionResult AdminLogin(string username)
        {

            return View(new LoginViewModel() { Username = username });
        }

        [HttpGet]
        public ActionResult Logout(Gebruiker gebruiker)
        {
            if(gebruiker != null)
                return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel logon)
        {
            TempData["Info"] = logon.Paswoord;
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord)) 
            {
                //moet nu gebruiker binden
                if (HttpContext.Session != null)
                { 
                    HttpContext.Session["gebruiker"] = gebruiker; 
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult BibliothecarisLogin(LoginViewModel logon)
        {   
            
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.BibliotheekRechten==true && gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord))
            {
                //moet nu gebruiker binden
                if (HttpContext.Session != null)
                {
                    TempData["Info"] = "Succesvol ingelogd als Bibliothecaris";
                    HttpContext.Session["gebruiker"] = gebruiker;
                }
                return Redirect((Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString());
            }
            return RedirectToAction("BibliothecarisLogin");
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginViewModel logon)
        {   //if AdminRechten op adminLogin, return to last page, else return adminlogin 
            
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.AdminRechten == true &&  gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord))
            {
                TempData["Info"] = "Succesvol ingelogd!";
                if (HttpContext.Session != null)
                {
                    TempData["Info"] = "Succesvol ingelogd als Bibliothecaris";
                    HttpContext.Session["gebruiker"] = gebruiker;
                }
                return RedirectToAction("Index", "Home");
            }
            TempData["Info"] = "Verkeerd paswoord!";
            return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public ActionResult LogoutConfirmed()
        {
            if (HttpContext.Session != null)
            {
                TempData["Info"] = "Succesvol uitgelogd";
                HttpContext.Session["gebruiker"] = null;
            }
            return Redirect((Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString());
            
        }
    }
}




