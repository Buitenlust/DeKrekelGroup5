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
        private IGebruikerRepository gebruikerRepository;
        private Gebruiker Gebruiker;
        

        public DVDController(IGebruikerRepository gebruikerRepository)
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

        // GET: DVDs
        public ActionResult Index(String search = null)
        {
            //LetterTuin letterTuin = Gebruiker.

            IEnumerable<DVD> dvds;
            if (!String.IsNullOrEmpty(search))
            {
                dvds = Gebruiker.LetterTuin.GetDVDs(search);
                ViewBag.Selection = "Alle boeken met " + search;
            }
            else
            {
                dvds = Gebruiker.LetterTuin.GetDVDs(null);
                ViewBag.Selection = "Alle boeken";
            }
            if (Request.IsAjaxRequest())
                return PartialView("DVDLijst", new DVDLijstViewModel(dvds));

            return View(new DVDLijstViewModel(dvds));
        }

        // GET: DVD/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            DVD dvd = Gebruiker.LetterTuin.GetItem(id) as DVD;
            if (dvd == null)
                return HttpNotFound();
            return View(dvd);
        }

        // GET: DVD/Create
        public ActionResult Create(Gebruiker gebruiker = null)
        {
            return View(new DVDCreateViewModel(Gebruiker.LetterTuin.Themas.ToList(), new Boek()));
        }

        // POST: DVD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "DVD")] DVDViewModel dvd, Gebruiker gebruiker = null)
        {
            if (ModelState.IsValid)
            {
                DVD newDVD = new DVD()
                {
                    Titel = dvd.Titel,
                    Leeftijd = dvd.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(dvd.Thema),
                    Omschrijving = dvd.Omschrijving,
                    Exemplaar = 0,
                    Uitgever = dvd.Uitgever
                };

                Gebruiker.AddItem(newDVD);
                gebruikerRepository.DoNotDuplicateThema(newDVD);
                gebruikerRepository.SaveChanges();
                TempData["Info"] = "De dvd werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: DVD/Edit/5
        public ActionResult Edit(Gebruiker gebruiker = null, int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            DVD dvd = Gebruiker.LetterTuin.GetItem(id) as DVD;
            if (dvd == null)
                return HttpNotFound();
            return View(new DVDCreateViewModel(Gebruiker.LetterTuin.Themas, dvd));
        }

        // POST: DVD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "DVD")] DVDViewModel dvd, Gebruiker gebruiker = null)
        {
            if (ModelState.IsValid)
            {
                DVD newDVD = new DVD()
                {
                    Titel = dvd.Titel,
                    Leeftijd = dvd.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(dvd.Thema),
                    Omschrijving = dvd.Omschrijving,
                    Exemplaar = dvd.Exemplaar,
                    Uitgever = dvd.Uitgever

                };
                Gebruiker.UpdateDVD(newDVD);
                gebruikerRepository.DoNotDuplicateThema(newDVD);
                gebruikerRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dvd);
        }

        // GET: DVD/Delete/5
        public ActionResult Delete(Gebruiker gebruiker = null, int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DVD dvd = Gebruiker.LetterTuin.GetItem(id) as DVD;
            if (dvd == null)
            {
                return HttpNotFound();
            }
            return View(dvd);
        }

        // POST: DVD/Delete/5
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