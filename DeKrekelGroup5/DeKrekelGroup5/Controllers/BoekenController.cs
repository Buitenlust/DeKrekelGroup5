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
    public class BoekenController : Controller
    {
        private ILettertuinRepository letterTuinRepository;
        private IBeheerderRepository beheerderRepository;
        private IBibliothecarisRepository bibliothecarisRepository;

        public BoekenController(ILettertuinRepository letterTuinRepository, IBeheerderRepository beheerderRepository, IBibliothecarisRepository bibliothecarisRepository)
        {
            this.letterTuinRepository = letterTuinRepository;
            this.beheerderRepository = beheerderRepository;
            this.bibliothecarisRepository = bibliothecarisRepository;
        }

        // GET: Boeken
        public ActionResult Index(LetterTuin letterTuin, String search=null)
        {
            IEnumerable<Boek> boeken;
            if (!String.IsNullOrEmpty(search))
            {
                boeken = letterTuinRepository.FindBoek(search);
                ViewBag.Selection = "Alle boeken met " + search;
            }
            else
            {
                boeken = letterTuinRepository.FindAllBoeken().Take(25);
                ViewBag.Selection = "Alle boeken";
            }
            if (Request.IsAjaxRequest())
                return PartialView("BoekenLijst", new BoekenLijstViewModel(boeken));

            return View(new BoekenLijstViewModel(boeken));
        }

        // GET: Boeken/Details/5
        public ActionResult Details(LetterTuin letterTuin, int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = letterTuinRepository.FindItemById(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(boek);
        }

        // GET: Boeken/Create
        public ActionResult Create(Beheerder beheerder)
        {
            return View(new BoekCreateViewModel(beheerder.GetThemas(), new Boek()));
        }

        // POST: Boeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Beheerder beheerder,[Bind(Prefix = "Boek")] BoekViewModel boek)
        {
            if (ModelState.IsValid)
            {
                Boek newBoek = new Boek()
                {
                    Titel = boek.Titel,
                    Auteur = boek.Auteur,
                    Leeftijd = boek.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = beheerder.GetThemaByName(boek.Thema),
                    Omschrijving = boek.Omschrijving,
                    Exemplaar = 0,
                    Uitgever = boek.Uitgever
                };

                beheerder.AddItem(newBoek);
                beheerderRepository.DoNotDuplicateThema(newBoek);
                beheerderRepository.SaveChanges();
                TempData["Info"] = "Het boek werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Boeken/Edit/5
        public ActionResult Edit(Beheerder beheerder,int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = letterTuinRepository.FindItemById(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(new BoekCreateViewModel(letterTuinRepository.FindAllthemas(), boek));
        }

        // POST: Boeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Beheerder beheerder,[Bind(Prefix = "Boek")] BoekViewModel boek)
        {
            if (ModelState.IsValid)
            {
                Boek newBoek = new Boek()
                {
                    Titel = boek.Titel,
                    Auteur = boek.Auteur,
                    Leeftijd = boek.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = beheerder.GetThemaByName(boek.Thema),
                    Omschrijving = boek.Omschrijving,
                    Exemplaar = boek.Exemplaar,
                    Uitgever = boek.Uitgever

                };
                beheerder.AddItem(newBoek);
                beheerderRepository.DoNotDuplicateThema(newBoek);
                beheerderRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boek);
        }

        // GET: Boeken/Delete/5
        public ActionResult Delete(Beheerder beheerder, int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Boek boek = letterTuinRepository.FindItemById(id) as Boek;
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        // POST: Boeken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Beheerder beheerder, int id)
        { 
            beheerder.RemoveItem(id);
            return RedirectToAction("Index");
        }
    }
}
