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
    public class VertelTasController: Controller
    {
        private IGebruikerRepository gebruikersRep;

        public VertelTasController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        //GET: verteltassen
        public ActionResult Index(Gebruiker gebruiker, String search = null)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<VertelTas> verteltassen;
                if (!String.IsNullOrEmpty(search))
                {
                    verteltassen = gebruiker.LetterTuin.GetVertelTassen(search);
                    ViewBag.Selection = "Alle verteltassen met " + search;
                }
                else
                {
                    verteltassen = gebruiker.LetterTuin.GetVertelTassen(null);
                    ViewBag.Selection = "Alle verteltassen";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("VertelTasLijst", new VertelTasLijstViewModel(verteltassen) { IsAdmin = gebruiker.AdminRechten, IsBibliothecaris = gebruiker.BibliotheekRechten });

                return View(new VertelTasLijstViewModel(verteltassen) { IsAdmin = gebruiker.AdminRechten, IsBibliothecaris = gebruiker.BibliotheekRechten });
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //GET : Verteltassen/Details/5
        public ActionResult Details(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                VertelTas vertelTas = gebruiker.LetterTuin.GetItem(id) as VertelTas;
                if (vertelTas == null)
                    return HttpNotFound();
                return View(vertelTas);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        //GET: Verteltassen : create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            try
            {
                return View(new VertelTasCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new VertelTas()));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //POST : VertelTassen/Create
        [HttpPost]
        public ActionResult Create([Bind(Prefix = "Verteltas")] VertelTasViewModel vertelTas, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && vertelTas != null && vertelTas.Exemplaar <= 0)
            {
                try
                {
                    VertelTas newVertelTas = vertelTas.MapToVerteltas(vertelTas, gebruiker.LetterTuin.GetThemaByName(vertelTas.Thema));
                    gebruiker.AddItem(newVertelTas);
                    gebruikersRep.DoNotDuplicateThema(newVertelTas);
                    gebruikersRep.SaveChanges();
                    TempData["Info"] = "Verteltas" + vertelTas.Titel + " werd toegevoegd...";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

            }
            return RedirectToAction("Create");
        }

        //GET Verteltassen/Edit/
        public ActionResult Edit(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                VertelTas vertelTas = gebruiker.LetterTuin.GetItem(id) as VertelTas;
                if (vertelTas == null)
                    return HttpNotFound();
                return View(new VertelTasCreateViewModel(gebruiker.LetterTuin.Themas, vertelTas)); 
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "VertelTas")] VertelTasViewModel vertelTas, Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && vertelTas != null && vertelTas.Exemplaar > 0)
            {
                try
                {
                    vertelTas.Uitgeleend = false;
                    vertelTas.Beschikbaar = true;
                    VertelTas newVertelTas = vertelTas.MapToVertelTas(vertelTas, gebruiker.LetterTuin.GetThemaByName(vertelTas.Thema));
                    gebruiker.UpdateVertelTas(newVertelTas);
                    gebruikersRep.DoNotDuplicateThema(newVertelTas);
                    TempData["Info"] = "VertelTas " + vertelTas.Titel + " werd aangepast...";
                    gebruikersRep.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return View(vertelTas);
        }


        // GET: Verteltas/Delete
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                VertelTas vertelTas = gebruiker.LetterTuin.GetItem(id) as VertelTas;
                if (vertelTas == null)
                    return HttpNotFound();
                return View(vertelTas);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Verteltas/Delete/5
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
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                VertelTas vertelTas = gebruiker.LetterTuin.GetItem(id) as VertelTas;
                if (vertelTas == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(vertelTas);
                gebruikersRep.SaveChanges();
                TempData["Info"] = "Verteltas" + vertelTas.Titel + " werd verwijderd...";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }


        }
    }
}