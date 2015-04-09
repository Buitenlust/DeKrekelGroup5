using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Controllers
{
    public class HomeController : Controller
    {
        private IGebruikerRepository GebruikersRep;
        public HomeController(IGebruikerRepository gebruikersrep)
        {
            GebruikersRep = gebruikersrep;

            //if (HttpContext.Session["gebruiker"] == null)
            //{
            //    HttpContext.Session["gebruiker"] = GebruikersRep.GetGebruiker(1);
            //}
        }
  
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}