using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private IGebruikerRepository gebruikersRep; 

        public BoekenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: Boeken
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            if(gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<Boek> boeken;
                if (!String.IsNullOrEmpty(search))
                {
                    boeken = gebruiker.LetterTuin.GetBoeken(search);
                    ViewBag.Selection = "Alle boeken met " + search;
                }
                else
                {
                    boeken = gebruiker.LetterTuin.GetBoeken(null);
                    ViewBag.Selection = "Alle boeken";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("BoekenLijst", new MainViewModel(gebruiker).SetNewBoekenLijstVm(boeken));

                return View(new MainViewModel(gebruiker).SetNewBoekenLijstVm(boeken));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Boeken/Details/5
        public ActionResult Details(Gebruiker gebruiker, MainViewModel mvm, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Boek boek = gebruiker.LetterTuin.GetItem(id) as Boek;
                if (boek == null)
                    return HttpNotFound();
                return View("Details", mvm.SetBoekViewModel(boek));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // GET: Boeken/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();

            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                MainViewModel mvm = new MainViewModel(gebruiker);
                mvm.SetBoekCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new Boek());
                HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Boeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "BoekCreateViewModel")] BoekCreateViewModel boek, Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && boek != null && boek.Boek.Exemplaar <=0)
            {
                try
                { 
                    boek.Boek.image = mvm.BoekCreateViewModel.Boek.image;
                    Boek newBoek = boek.Boek.MapToBoek(boek.Boek, gebruiker.LetterTuin.GetThemaByName(boek.Boek.Thema));
                    
                    gebruiker.AddItem(newBoek);
                    gebruikersRep.DoNotDuplicateThema(newBoek);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("Boek" + boek.Boek.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Items.Max(b => b.Exemplaar) });
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                
            }
            return RedirectToAction("Create");
        }

        // GET: Boeken/Edit/5
        public ActionResult Edit(Gebruiker gebruiker,int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Boek boek = gebruiker.LetterTuin.GetItem(id) as Boek;
                if (boek == null)
                    return HttpNotFound();
                MainViewModel mvm = new MainViewModel(gebruiker);
                mvm.SetBoekCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), boek);
                HttpContext.Session["main"] = mvm;
            
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // POST: Boeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "BoekCreateViewModel")] BoekCreateViewModel boek, Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage=null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);

            if (newimage != null)
            {
                return UploadImage(gebruiker,mvm,newimage);
            }
            else
            {
                if (ModelState.IsValid && boek != null && boek.Boek.Exemplaar > 0)
                {
                    try
                    {
                        boek.Boek.image = mvm.BoekCreateViewModel.Boek.image;
                        Boek newBoek = boek.Boek.MapToBoek(boek.Boek, gebruiker.LetterTuin.GetThemaByName(boek.Boek.Thema));
                        gebruiker.UpdateBoek(newBoek);
                        gebruikersRep.DoNotDuplicateThema(newBoek);
                        mvm.SetNewInfo("Boek " + boek.Boek.Titel + " werd aangepast...");
                        gebruikersRep.SaveChanges();
                        return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = newBoek.Exemplaar });
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                mvm.BoekCreateViewModel.Themas = new SelectList(gebruiker.LetterTuin.Themas.ToList());
                return View(new MainViewModel(gebruiker) { BoekCreateViewModel = boek });
            }


            
        }

        // GET: Boeken/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Boek boek = gebruiker.LetterTuin.GetItem(id) as Boek;
                if (boek == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetBoekViewModel(boek));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Boeken/Delete/5
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
                Boek boek = gebruiker.LetterTuin.GetItem(id) as Boek;
                if (boek == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(boek);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("Boek" +boek.Titel+ " werd verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPost]
        public ActionResult UploadImage(Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage)
        { 
            if (newimage != null)
            {
                int rnd = new Random().Next(99999);
                string pic = System.IO.Path.GetFileName(newimage.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"),  rnd + pic);
                ;

                // file is uploaded
                newimage.SaveAs(path);
                mvm.BoekCreateViewModel.Boek.image = rnd+pic;
                return Json(new { imagePath = rnd+pic });
            }
             
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}
