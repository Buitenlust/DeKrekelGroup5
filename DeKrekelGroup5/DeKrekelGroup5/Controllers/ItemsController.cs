using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Discovery;
using DeKrekelGroup5.Models;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class ItemsController : Controller
    { 
        private IItemRepository ii;
        private IThemasRepository tr;
        private LetterTuin letterTuin;
        private Beheerder beheerder;

        public ItemsController(IItemRepository iitemRepository,IThemasRepository themasRepository)
        {
            letterTuin = new LetterTuin(iitemRepository, themasRepository);
            beheerder = new Beheerder(iitemRepository, themasRepository);
            ii = iitemRepository;
            tr = themasRepository;
        }

        // GET: Nada, Nothing, Njet
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Items/Boeken
        public ActionResult Boeken(String search = null)
        {
            ViewBag.Selection = search == null ? "Alle boeken:" : "Alle boeken met " + search + ":";
            if (Request.IsAjaxRequest())
                return PartialView("ItemsLijst", new ItemsViewModel(letterTuin.GetBoeken(search)));

            return View("Index", new ItemsViewModel(letterTuin.GetBoeken(search)));
        }

        // GET: Items/Spellen
        public ActionResult Spellen(String search = null)
        {
            ViewBag.Selection = search == null ? "Alle spellen:" : "Alle spellen met " + search + ":";
            if (Request.IsAjaxRequest())
                return PartialView("ItemsLijst", new ItemsViewModel(letterTuin.GetSpellen(search)));
            return View("Index", new ItemsViewModel(letterTuin.GetSpellen(search)));
        }



        // GET: items/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Item item = ii.FindById(id);
            
            if (item == null)
                return HttpNotFound();
            
            item.Themaa = letterTuin.GetThemabyId(item.Themaa.IdThema);

            return View(new ItemViewModel(item));
        }

        // GET: Items/Create
        public ActionResult Create(string item="boek")
        {
            switch (item)
            {
                case "boek":
                    return View(new CreateItem(letterTuin.GetThemas(), new Boek()));
                case "spel":
                    return View(new CreateItem(letterTuin.GetThemas(), new Spel()));
                default: 
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string item, [Bind(Prefix = "ItemVm")] ItemViewModel itemVm)
        {
            if (ModelState.IsValid)
            {
                switch (item)
                {
                    case "boek": 
                        return View("Details", new ItemViewModel(beheerder.AddItem(itemVm, new Boek()))); 
                    case "spel":
                        return View("Details", new ItemViewModel(beheerder.AddItem(itemVm, new Spel()))); 
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return RedirectToAction("Create");
        }

        // GET: Boeks/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Boek boek = (Boek) ii.FindById(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(new BoekThemaCreateViewModel(tr.FindAll().OrderBy(n => n.Themaa), boek.ConvertToBoekCreateViewModel()));
        }

        // POST: Boeks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Boek")] BoekCreateViewModel boek)
        {
            Boek bk = null;
            if (ModelState.IsValid)
            {
                bk = (Boek) ii.FindById(boek.Exemplaar);

                if (bk == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    bk.Exemplaar = boek.Exemplaar;
                    bk.Auteur = boek.Auteur;
                    bk.Leeftijd = boek.Leeftijd;
                    bk.Omschrijving = boek.Omschrijving;
                    bk.Titel = boek.Titel;
                    bk.Uitgever = boek.Uitgever; 
                    bk.Themaa = (String.IsNullOrEmpty(boek.Thema) ? null : tr.FindBy(boek.Thema));

                    bk.Update(bk);
                    ii.ModifiedThema(bk);
                    ii.SaveChanges();
                    TempData["Info"] = "Het boek werd aangepast...";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

                return RedirectToAction("Index");
            }
            return View(boek);
        }

        // GET: Boeks/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 

            Boek boek = (Boek) ii.FindById(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        // POST: Boeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Boek boek = (Boek) ii.FindById(id);
            ii.Remove(boek);
            ii.ModifiedThema(boek);
            ii.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
