using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class DVDController : Controller
    {
        private IGebruikerRepository gebruikersRep; 

        public DVDController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: DVD
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            if(gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<DVD> dvds;
                if (!String.IsNullOrEmpty(search))
                {
                    dvds = gebruiker.LetterTuin.GetDVDs(search);
                    ViewBag.Selection = "Alle DVDs met " + search;
                }
                else
                {
                    dvds = gebruiker.LetterTuin.GetDVDs(null);
                    ViewBag.Selection = "Alle dvds";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("DVDLijst", new MainViewModel(gebruiker).SetNewDVDLijstVm(dvds));

                return View(new MainViewModel(gebruiker).SetNewDVDLijstVm(dvds));
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
            
        }

        // GET: DVD/Details/5
        public ActionResult Details(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                DVD dvd = gebruiker.LetterTuin.GetItem(id) as DVD;
                if (dvd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetDVDViewModel(dvd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // GET: DVD/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                return View(new MainViewModel(gebruiker).SetDVDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new DVD()));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: DVD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "DVDCreateViewModel")] DVDCreateViewModel dvd, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && dvd != null && dvd.DVD.Exemplaar <=0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    DVD newDVD = dvd.DVD.MapToDVD(dvd.DVD, gebruiker.LetterTuin.GetThemaByName(dvd.DVD.Thema));
                    gebruiker.AddItem(newDVD);
                    gebruikersRep.DoNotDuplicateThema(newDVD);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("DVD" + dvd.DVD.Titel + " werd toegevoegd...");
                    return View("Index", mvm );
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                
            }
            return RedirectToAction("Create");
        }

        // GET: DVD/Edit/5
        public ActionResult Edit(Gebruiker gebruiker,int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                DVD dvd = gebruiker.LetterTuin.GetItem(id) as DVD;
                if (dvd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetDVDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), dvd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // POST: DVD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "DVDCreateViewModel")] DVDCreateViewModel dvd, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && dvd!= null && dvd.DVD.Exemplaar > 0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    DVD newDVD = dvd.DVD.MapToDVD(dvd.DVD, gebruiker.LetterTuin.GetThemaByName(dvd.DVD.Thema));
                    gebruiker.UpdateDVD(newDVD);
                    gebruikersRep.DoNotDuplicateThema(newDVD);
                    mvm.SetNewInfo("DVD " + dvd.DVD.Titel + " werd aangepast...");
                    gebruikersRep.SaveChanges();
                    return View("Index", mvm);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return View(new MainViewModel(gebruiker){DVDCreateViewModel = dvd});
        }

        // GET: DVD/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                DVD dvd = gebruiker.LetterTuin.GetItem(id) as DVD;
                if (dvd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetDVDViewModel(dvd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: DVD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                MainViewModel mvm = new MainViewModel(gebruiker);
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                DVD dvd = gebruiker.LetterTuin.GetItem(id) as DVD;
                if (dvd == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(dvd);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("DVD" +dvd.Titel+ " werd verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            
        }
    }
}