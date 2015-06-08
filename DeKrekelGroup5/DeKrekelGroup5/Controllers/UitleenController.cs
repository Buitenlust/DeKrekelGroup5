using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class UitleenController : Controller
    {
        private IGebruikerRepository gebruikersRep; 

        public UitleenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: Uitleen
        public ActionResult Index(Gebruiker gebruiker, String search = null)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                //IEnumerable<Uitlening> uitleningen;
                //if (!String.IsNullOrEmpty(search))
                //{
                //    uitleningen = gebruiker.GetUitleningen(search).ToList();                    
                //}
                //else
                //{
                //    uitleningen = gebruiker.GetUitleningen(null).ToList();
                //}

                //if (Request.IsAjaxRequest())
                //    return PartialView("UitleningenLijst", new MainViewModel(gebruiker).SetNewUitleningenLijstVm(uitleningen));

                //return View(new MainViewModel(gebruiker).SetNewUitleningenLijstVm(uitleningen));
                return View();
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}