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

namespace DeKrekelGroup5.Controllers
{
    public class UitlenersController : Controller
    {
        private readonly IGebruikerRepository gebruikerRepository;

        public UitlenersController(IGebruikerRepository gebruikerRepository)
        {
            this.gebruikerRepository = gebruikerRepository;
        }

        // GET: Uitleners
        public ActionResult Index(Gebruiker gebruiker, String search=null)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                return View(new UitlenersLijstViewModel(gebruiker.GetUitleners(search).ToList()));    
            }
            catch (AccessViolationException)
            {
                return new HttpUnauthorizedResult();
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Uitleners/Details/5
        public ActionResult Details(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                Uitlener uitlener = gebruiker.GetUitlenerById(id);
                if (uitlener == null)
                    return HttpNotFound();
                return View(new UitlenerViewModel(uitlener));

            }
            catch (AccessViolationException)
            {
                return new HttpUnauthorizedResult();
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Uitleners/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            return View(new UitlenerViewModel());
        }

        // POST: Uitleners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gebruiker gebruiker, [Bind(Include = "Id,Naam,VoorNaam,Klas,Adres,Email")] UitlenerViewModel uitlener)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && uitlener != null && uitlener.Id == 0 )
            {
                try
                {
                    gebruiker.AddUitlener(uitlener.MapNaarUitlener(uitlener));
                    gebruikerRepository.SaveChanges();
                    TempData["Info"] = "Uitlener " + uitlener.Naam +" "+ uitlener.VoorNaam + " werd toegevoegd...";
                    return RedirectToAction("Index");
                }
                catch (AccessViolationException)
                {
                    return new HttpUnauthorizedResult();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }

            return View(uitlener);
        }

        // GET: Uitleners/Edit/5
        public ActionResult Edit(Gebruiker gebruiker, int id=0) //todo viewmodel
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Uitlener uitlener = gebruiker.GetUitlenerById(id);
            if (uitlener == null)
                return HttpNotFound();
            
            return View(new UitlenerViewModel(uitlener));
        }

        // POST: Uitleners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gebruiker gebruiker, [Bind(Include = "Id,Naam,VoorNaam,Klas,Adres,Email")] UitlenerViewModel uitlener)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && uitlener != null && uitlener.Id > 0)
            {
                try
                {
                    gebruiker.UpdateUitlener(uitlener.MapNaarUitlener(uitlener));
                    TempData["Info"] = "Uitlener " + uitlener.Naam + " " + uitlener.VoorNaam + " werd aangepast...";
                    gebruikerRepository.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (AccessViolationException)
                {
                    return new HttpUnauthorizedResult();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                
            }
            return View(uitlener);
        }

        // GET: Uitleners/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)       //todo heeft gebruiker nog uitleningen?
        {
             if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
             gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Uitlener uitlener = gebruiker.GetUitlenerById(id);
            if (uitlener == null)
                return HttpNotFound();
            
            return View(uitlener);
        }

        // POST: Uitleners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id=0)  //todo heeft gebruiker nog uitleningen? zoja, lijst weergeven van uitleningen.
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                    Uitlener uitlener = gebruiker.GetUitlenerById(id);
                        if (uitlener == null)
                    return HttpNotFound();
                    gebruiker.RemoveUitlener(uitlener);
                    gebruikerRepository.SaveChanges();
                    TempData["Info"] = "Uitlener " + uitlener.Naam + " " + uitlener.VoorNaam + " werd toegevoegd...";
                    return RedirectToAction("Index");
                }
                catch (AccessViolationException)
                {
                    return new HttpUnauthorizedResult();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
        } 
    }
}
