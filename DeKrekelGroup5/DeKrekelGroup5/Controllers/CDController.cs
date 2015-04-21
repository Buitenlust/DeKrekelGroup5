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
        private IGebruikerRepository gebruikerRepository;
        private Gebruiker Gebruiker;

        public CDController(IGebruikerRepository gebruikerRepository)
        {
            this.gebruikerRepository = gebruikerRepository;

            Gebruiker = gebruikerRepository.GetGebruiker(1); //Anonymous
            if (Gebruiker == null)
            {
                Gebruiker = new Gebruiker(){AdminRechten = true, BibliotheekRechten = true, GebruikersNaam = "Anonymous", LetterTuin = new LetterTuin()};
                Gebruiker.VeranderPaswoord("Annymous");
                Gebruiker.LetterTuin.Instellingen = new Instellingen(){MaxVerlengingen = 2, BedragBoetePerDag = 1, UitleenDagen = 14};
                gebruikerRepository.AddGebruiker(Gebruiker);
                gebruikerRepository.SaveChanges();
                Gebruiker = gebruikerRepository.GetGebruiker(1);
            }
        }

        // GET: CD
        public ActionResult Index(String search = null)
        {
            IEnumerable<CD> cds;
            if (!String.IsNullOrEmpty(search))
            {
                cds = Gebruiker.LetterTuin.GetCDs(search);
                ViewBag.Selection = "Alle CDs met " + search;
            }
            else
            {
                cds = Gebruiker.LetterTuin.GetCDs(null);
                ViewBag.Selection = "Alle CDs";
            }
            if (Request.IsAjaxRequest())
                return PartialView("CDLijst", new CDLijstViewModel(cds));

            return View(new CDLijstViewModel(cds));
        }

        // GET: CDs/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            CD cd = Gebruiker.LetterTuin.GetItem(id) as CD;
            if (cd == null)
                return HttpNotFound();
            return View(cd);
        }

        // GET: CDs/Create
        public ActionResult Create(Gebruiker gebruiker = null)
        {
            return View(new CDCreateViewModel(Gebruiker.LetterTuin.Themas.ToList(), new CD()));
        }

        // POST: CDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "CD")] CDViewModel cd, Gebruiker gebruiker = null)
        {
            if (ModelState.IsValid)
            {
                CD newCD = new CD()
                {
                    Titel = cd.Titel,
                    Artiest = cd.Artiest,
                    Leeftijd = cd.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(cd.Thema),
                    Omschrijving = cd.Omschrijving,
                    Exemplaar = 0,
                    Uitgever = cd.Uitgever

                };

                Gebruiker.AddItem(newCD);
                gebruikerRepository.DoNotDuplicateThema(newCD);
                gebruikerRepository.SaveChanges();
                TempData["Info"] = "De CD werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: CDs/Edit/5
        public ActionResult Edit(Gebruiker gebruiker = null, int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            CD cd = Gebruiker.LetterTuin.GetItem(id) as CD;
            if (cd == null)
                return HttpNotFound();
            return View(new CDCreateViewModel(Gebruiker.LetterTuin.Themas,cd));
        }

        // POST: Boeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "CD")] CDViewModel cd, Gebruiker gebruiker = null)
        {
            if (ModelState.IsValid)
            {
                CD newCD = new CD()
                {
                    Titel = cd.Titel,
                    Artiest = cd.Artiest,
                    Leeftijd = cd.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(cd.Thema),
                    Omschrijving = cd.Omschrijving,
                    Exemplaar = cd.Exemplaar,
                    Uitgever = cd.Uitgever

                };
                Gebruiker.UpdateCD(newCD);
                gebruikerRepository.DoNotDuplicateThema(newCD);
                gebruikerRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cd);
        }

        // GET: CD/Delete/5
        public ActionResult Delete(Gebruiker gebruiker = null, int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CD cd = Gebruiker.LetterTuin.GetItem(id) as CD;
            if (cd == null)
            {
                return HttpNotFound();
            }
            return View(cd);
        }

        // POST: CD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id)
        {
            Gebruiker.RemoveItem(id);
            gebruikerRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}