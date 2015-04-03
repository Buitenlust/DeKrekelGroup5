
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
            TempData["Info"] = logon.Paswoord;
            Gebruiker gebruiker = gebruikers.SingleOrDefault(g => g.GebruikersNaam == logon.Username);
            if (gebruiker != null && gebruiker.PaswoordHashed == gebruiker.HashPassword(logon.Paswoord)) 
            {
                //moet nu gebruiker binden
                if (HttpContext.Session != null)
                { 
                    HttpContext.Session["gebruiker"] = gebruiker; 
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login");
        }
    }
}




