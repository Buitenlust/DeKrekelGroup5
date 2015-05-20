using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class ThemasController : Controller
    {
        private IGebruikerRepository gebruikersRep;

        public ThemasController(IGebruikerRepository lt)
        {
            gebruikersRep = lt; 
        }

        // GET: Themas
        public ActionResult Index(Gebruiker gebruiker, MainViewModel mvm, string search=null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
           
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                mvm.SetGebruikerToVm(gebruiker); 
                IEnumerable<Thema> themas;
                mvm.Themas = gebruiker.LetterTuin.GetThemas(search).ToList(); 
                
                if (Request.IsAjaxRequest())
                    return PartialView("ThemaLijst", mvm);
                mvm.InfoViewModel.Info = null;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Themas/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                mvm.SetGebruikerToVm(gebruiker);
                //HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Themas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "ThemaViewModel")] ThemaViewModel themaVm, Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if (ModelState.IsValid && themaVm !=null && !themaVm.Themaa.IsNullOrWhiteSpace())
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                gebruiker.AddThema(new Thema(){Themaa =  themaVm.Themaa});
                gebruikersRep.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mvm);
        }

        // GET: Themas/Edit/5
        public ActionResult Edit(Gebruiker gebruiker, int id = 0)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                mvm.SetGebruikerToVm(gebruiker);
                mvm.ThemaViewModel = new ThemaViewModel(gebruiker.LetterTuin.GetThemaById(id));
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Themas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "ThemaViewModel")] ThemaViewModel themaVm, Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            
            if (ModelState.IsValid)
            {
                try
                {
                    gebruiker.UpdateThema(themaVm);

                    gebruikersRep.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
                
                return RedirectToAction("Index");
            }
            return View(mvm);
        }

        // GET: Themas/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, MainViewModel mvm, int id = 0)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if (id > 0)
                try
                {
                    gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                    mvm.ThemaViewModel = new ThemaViewModel(gebruiker.LetterTuin.GetThemaById(id));
                    mvm.SetGebruikerToVm(gebruiker);
                    if (mvm.ThemaViewModel.Themaa != null)
                    {
                        mvm.SetNewInfo("Wenst u " + mvm.ThemaViewModel.Themaa + " te verwijderen?", false, true, "DeleteConfirmed");
                        if (HttpContext.Session != null)
                            HttpContext.Session["main"] = mvm;
                        return PartialView("_info", mvm);
                    }
                    return new HttpNotFoundResult();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Themas/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker,MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            Thema thema = gebruiker.LetterTuin.GetThemaById(mvm.ThemaViewModel.IdThema);
            gebruiker.RemoveThema(thema);
            gebruikersRep.SaveChanges();
            return PartialView("_info", mvm.SetNewInfo(thema.Themaa + " is verwijderd!", false, false, "/Themas/Index")); 
        }
    }
}
