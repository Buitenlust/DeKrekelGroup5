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
    public class SpellenController : Controller
    {
        private IGebruikerRepository gebruikersRep; 

        public SpellenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: Spellen
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            if(gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<Spel> spellen;
                if (!String.IsNullOrEmpty(search))
                {
                    spellen = gebruiker.LetterTuin.GetSpellen(search);
                    ViewBag.Selection = "Alle spellen met " + search;
                }
                else
                {
                    spellen = gebruiker.LetterTuin.GetSpellen(null);
                    ViewBag.Selection = "Alle spellen";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("SpellenLijst", new MainViewModel(gebruiker).SetNewSpellenLijstVm(spellen));

                return View(new MainViewModel(gebruiker).SetNewSpellenLijstVm(spellen));
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
            
        }

        // GET: Spellen/Details/5
        public ActionResult Details(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Spel spel = gebruiker.LetterTuin.GetItem(id) as Spel;
                if (spel == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetSpelViewModel(spel));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // GET: Spellen/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                return View(new MainViewModel(gebruiker).SetSpelCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new Spel()));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "SpelCreateViewModel")] SpelCreateViewModel spel, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && spel != null && spel.Spel.Exemplaar <=0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    Spel newSpel = spel.Spel.MapToSpel(spel.Spel, gebruiker.LetterTuin.GetThemaByName(spel.Spel.Thema));
                    gebruiker.AddItem(newSpel);
                    gebruikersRep.DoNotDuplicateThema(newSpel);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("Spel" + spel.Spel.Titel + " werd toegevoegd...");
                    return View("Index", mvm );
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                
            }
            return RedirectToAction("Create");
        }

        // GET: Spellen/Edit/5
        public ActionResult Edit(Gebruiker gebruiker,int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Spel spel = gebruiker.LetterTuin.GetItem(id) as Spel;
                if (spel == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetSpelCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), spel));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "SpelCreateViewModel")] SpelCreateViewModel spel, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && spel!= null && spel.Spel.Exemplaar > 0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    Spel newSpel = spel.Spel.MapToSpel(spel.Spel, gebruiker.LetterTuin.GetThemaByName(spel.Spel.Thema));
                    gebruiker.UpdateSpel(newSpel);
                    gebruikersRep.DoNotDuplicateThema(newSpel);
                    mvm.SetNewInfo("Spel " + spel.Spel.Titel + " werd aangepast...");
                    gebruikersRep.SaveChanges();
                    return View("Index", mvm);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return View(new MainViewModel(gebruiker){SpelCreateViewModel = spel});
        }

        // GET: Spellen/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Spel spel = gebruiker.LetterTuin.GetItem(id) as Spel;
                if (spel == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetSpelViewModel(spel));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Spellen/Delete/5
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
                Spel spel = gebruiker.LetterTuin.GetItem(id) as Spel;
                if (spel == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(spel);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("Spel" +spel.Titel+ " werd verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            
        }
    }
}