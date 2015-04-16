
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
using System.Net;

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
            TempData["error"] = "";
            if (Request.IsAjaxRequest())
                return PartialView("Login", new MainViewModel(){GebruikerViewModel =  new GebruikerViewModel() { Username = username }});

            return PartialView("Login", new MainViewModel() { GebruikerViewModel = new GebruikerViewModel() { Username = username } });
        }

        [HttpGet]
        public ActionResult Logout(Gebruiker gebruiker)
        {
            if(gebruiker != null)
                return PartialView("Logout", new MainViewModel() { GebruikerViewModel = new GebruikerViewModel() { Username = gebruiker.GebruikersNaam } });
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
      
        
        
        [HttpPost]
        public ActionResult Login([Bind(Prefix = "GebruikerViewModel")] GebruikerViewModel logon)
        {   //if AdminRechten op adminLogin, return to last page, else return adminlogin 
           
            if (Request.IsAjaxRequest())
            {
                Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);

                if (gebruiker != null && gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord))
                {
                    if (HttpContext.Session != null)
                    {
                        HttpContext.Session["gebruiker"] = gebruiker;
                    }
                    TempData["success"] = "Welkom, u bent succesvol ingelogd, "+logon.Username;
                    return PartialView("_success", new MainViewModel(){GebruikerViewModel = logon});
                }
                TempData["error"] = "Verkeerd paswoord!";
                return PartialView("_LoginPartial", new MainViewModel(){GebruikerViewModel =  new GebruikerViewModel() { Username = logon.Username }});
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            if (HttpContext.Session != null)
            {
                TempData["success"] = "U ben succesvol uitgelogd!";
                HttpContext.Session["gebruiker"] = null;
                return PartialView("_success", new MainViewModel() { GebruikerViewModel = new GebruikerViewModel() });
            }
            return Redirect((Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString());
            
        }
    }
}




