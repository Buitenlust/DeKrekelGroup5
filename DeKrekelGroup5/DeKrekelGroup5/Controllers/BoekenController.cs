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
        private LetterTuin letterTuin;
        private Beheerder beheerder;
 

        public BoekenController(IBoekenRepository boekenRepository, IThemasRepository themasRepository)
        {
            letterTuin = new LetterTuin();
            beheerder = new Beheerder();
        }

        // GET: Boeken
        public ActionResult Index(String search=null)
        {
            IEnumerable<Boek> boeken;
            if (!String.IsNullOrEmpty(search))
            {
                boeken = letterTuin.GetBoeken(search);
                ViewBag.Selection = "Alle boeken met " + search;
            }
            else
            {
                boeken = letterTuin.GetBoeken(search).Take(25);
                ViewBag.Selection = "Alle boeken";
            }
            if (Request.IsAjaxRequest())
                return PartialView("BoekenLijst", new BoekenLijstViewModel(boeken));

            return View(new BoekenLijstViewModel(boeken));
        }

        // GET: Boeken/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = letterTuin.GetItem(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(boek);
        }

        // GET: Boeken/Create
        public ActionResult Create()
        {
            return View(new BoekCreateViewModel(beheerder.GetThemas(), new Boek()));
        }

        // POST: Boeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Boek")] BoekViewModel boek)
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
                TempData["Info"] = "Het boek werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Boeken/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = beheerder.GetItem(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(new BoekCreateViewModel(beheerder.GetThemas(), boek));
        }

        // POST: Boeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Boek")] BoekViewModel boek)
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

                return RedirectToAction("Index");
            }
            return View(boek);
        }

        // GET: Boeken/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Boek boek = beheerder.GetItem(id) as Boek;
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        // POST: Boeken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            beheerder.RemoveItem(id);
            return RedirectToAction("Index");
        }
    }
}
