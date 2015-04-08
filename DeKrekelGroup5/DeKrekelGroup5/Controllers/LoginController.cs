
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
        private IGebruikerRepository GebruikersRep { get; set; }
        private IEnumerable<Gebruiker> gebruikers;

        public LoginController(IGebruikerRepository gebruikersrep)
        {
            GebruikersRep = gebruikersrep;
            gebruikers = gebruikersrep.GetGebruikers();

            if (HttpContext.Session["gebruiker"] == null)
            {
                HttpContext.Session["gebruiker"] = GebruikersRep.GetGebruiker(1);
            }
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
        public ActionResult Logout()
        {

            return View(new LogoutViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel logon)
        {//maak 3 varianten: 
            //if AdminRechten op adminLogin, else return loginview opnieuw 
            //else if BibliotheekRechten op BibLogin, else return loginview opnieuw 
            //voor logoutview of if (HttpContext.Session["gebruiker"] = null): HttpContext.Session["gebruiker"] = gebruiker; waarbij gerbuiker gewone user is 
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
        {   //if AdminRechten op adminLogin, return loginview opnieuw 
            //else if BibliotheekRechten op BibLogin, else return loginview opnieuw 
            TempData["Info"] = logon.Paswoord;
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.AdminRechten==false && gebruiker.BibliotheekRechten==true && gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord))
            {
                //moet nu gebruiker binden
                if (HttpContext.Session != null)
                {
                    HttpContext.Session["gebruiker"] = gebruiker;
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("BibliothecarisLogin");
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginViewModel logon)
        {   //if AdminRechten op adminLogin, return to last page, else return adminlogin 
            TempData["Info"] = logon.Paswoord;
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.AdminRechten == true &&  gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord))
            {
                //moet nu gebruiker binden
                if (HttpContext.Session != null)
                {
                    HttpContext.Session["gebruiker"] = gebruiker;
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("AdminLogin");
        }

        [HttpPost]
        public ActionResult Logout(LogoutViewModel logoff)
        {// if yes, set user to standard user en goto previous page, if no: return to previous page
            TempData["Info"] = "gewone gebruiker";
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logoff.Username);

            if (gebruiker != null)
            {
                if (HttpContext.Session != null)
                {
                    HttpContext.Session["gebruiker"] = gebruiker;
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            //return to previous page
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}




