using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(IGebruikerRepository gebruikersrep)
        {
        }

        public ActionResult Index(Gebruiker gebruiker)
        {
            return View(new MainViewModel(gebruiker));
        }
    }
}