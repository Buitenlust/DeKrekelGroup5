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
    public class CDController : Controller
    {
        private IGebruikerRepository gebruikersRep; 

        public CDController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: CDs
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            if(gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<CD> cds;
                if (!String.IsNullOrEmpty(search))
                {
                    cds = gebruiker.LetterTuin.GetCDs(search);
                    ViewBag.Selection = "Alle CDs met " + search;
                }
                else
                {
                    cds = gebruiker.LetterTuin.GetCDs(null);
                    ViewBag.Selection = "Alle boeken";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("CDLijst", new MainViewModel(gebruiker).SetNewCDLijstVm(cds));

                return View(new MainViewModel(gebruiker).SetNewCDLijstVm(cds));
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
            
        }

        // GET: CD/Details/5
        public ActionResult Details(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // GET: CD/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                return View(new MainViewModel(gebruiker).SetCDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new CD()));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: CD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "CDCreateViewModel")] CDCreateViewModel cd, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && cd != null && cd.CD.Exemplaar <=0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    CD newCD = cd.CD.MapToCD(cd.CD, gebruiker.LetterTuin.GetThemaByName(cd.CD.Thema));
                    gebruiker.AddItem(newCD);
                    gebruikersRep.DoNotDuplicateThema(newCD);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("CD" + cd.CD.Titel + " werd toegevoegd...");
                    return View("Index", mvm );
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                
            }
            return RedirectToAction("Create");
        }

        // GET: CD/Edit/5
        public ActionResult Edit(Gebruiker gebruiker,int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetCDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // POST: CD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "CDCreateViewModel")] CDCreateViewModel cd, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && cd!= null && cd.CD.Exemplaar > 0)
            {
                try
                {
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    CD newCD = cd.CD.MapToCD(cd.CD, gebruiker.LetterTuin.GetThemaByName(cd.CD.Thema));
                    gebruiker.UpdateCD(newCD);
                    gebruikersRep.DoNotDuplicateThema(newCD);
                    mvm.SetNewInfo("CD " + cd.CD.Titel + " werd aangepast...");
                    gebruikersRep.SaveChanges();
                    return View("Index", mvm);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return View(new MainViewModel(gebruiker){CDCreateViewModel = cd});
        }

        // GET: CD/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: CD/Delete/5
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
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(cd);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("CD" +cd.Titel+ " werd verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            
        }
    }
}