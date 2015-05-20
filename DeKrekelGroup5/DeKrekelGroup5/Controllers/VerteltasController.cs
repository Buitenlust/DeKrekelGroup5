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
    public class VertelTasController : Controller
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
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);

            try
            {
                IEnumerable<VertelTas> verteltassen;

                if (!String.IsNullOrEmpty(search))
                {
                    verteltassen = gebruiker.LetterTuin.GetVertelTassen(search).ToList();
                    //ViewBag.Selection = "Alle boeken met " + search;
                }
                else
                {
                    verteltassen = gebruiker.LetterTuin.GetVertelTassen(null).ToList();
                    //ViewBag.Selection = "Alle boeken";
                }


                if (Request.IsAjaxRequest())
                    return PartialView("VertelTasLijst", new MainViewModel(gebruiker).SetNewVerteltasLijstVm(verteltassen));

                return View(new MainViewModel(gebruiker).SetNewVerteltasLijstVm(verteltassen));
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //GET : Verteltassen/Details/5
        public ActionResult Details(Gebruiker gebruiker, MainViewModel mvm, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            mvm.InfoViewModel.Info = null;
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                VertelTas vertelTas = gebruiker.LetterTuin.GetVertelTas(id);
                if (vertelTas == null)
                    mvm.SetNewInfo("Verteltas niet gevonden");
                return View(new MainViewModel(gebruiker).SetVerteltasViewModel(vertelTas));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        //GET: Verteltassen : create
        public ActionResult Create(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);

            try
            {
                mvm.SetVerteltasCreateViewModel(gebruiker.LetterTuin.Themas.ToList(),new VertelTas(),gebruiker.LetterTuin.Items);
                HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //POST : VertelTassen/Create
        [HttpPost]
        public ActionResult Create([Bind(Prefix = "VertelTasCreateViewModel")] VertelTasCreateViewModel vertelTas, Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && vertelTas != null && vertelTas.Verteltas.Exemplaar <= 0)
            {
                try
                {
                    vertelTas.Verteltas.image = mvm.VertelTasCreateViewModel.Verteltas.image;
                    List<Thema> themas = gebruiker.GetThemaListFromSelectedList(vertelTas.SubmittedThemas);
                    VertelTas newVertelTas = vertelTas.Verteltas.MapToVertelTas(vertelTas.Verteltas, themas,items:new List<Item>());

                    gebruiker.AddVertelTas(newVertelTas);
                    //gebruikersRep.DoNotDuplicateThema(newBoek);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("Verteltas" + vertelTas.Verteltas.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.VertelTassen.Max(b => b.Exemplaar) });
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
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            MainViewModel mvm = new MainViewModel(gebruiker);
            mvm.InfoViewModel.Info = null;
            mvm.SetGebruikerToVm(gebruiker);

            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                VertelTas vertelTas = gebruiker.LetterTuin.GetVertelTas(id);
                if (vertelTas == null)
                    return HttpNotFound();
                mvm.SetVerteltasCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new VertelTas(), gebruiker.LetterTuin.Items);
                HttpContext.Session["main"] = mvm;

                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "VertelTasCreateViewModel")] VertelTasCreateViewModel vertelTas, Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage = null)
        {

            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.InfoViewModel.Info = null;
            mvm.SetGebruikerToVm(gebruiker);
            if (newimage != null)
            {
                return UploadImage(gebruiker, mvm, newimage);
            }
            else
            {
                if (ModelState.IsValid && vertelTas != null && vertelTas.Verteltas.Exemplaar > 0)
                {
                    try
                    {
                        vertelTas.Verteltas.image = mvm.VertelTasCreateViewModel.Verteltas.image;

                        List<Thema> themas = gebruiker.GetThemaListFromSelectedList(vertelTas.SubmittedThemas);
                        VertelTas newVertelTas = vertelTas.Verteltas.MapToVertelTas(vertelTas.Verteltas, themas,new List<Item>());
                        gebruiker.UpdateVertelTas(newVertelTas);
                        //gebruikersRep.DoNotDuplicateThema(newBoek);
                        mvm.SetNewInfo("Verteltas " + vertelTas.Verteltas.Titel + " werd aangepast...");
                        gebruikersRep.SaveChanges();
                        return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = newVertelTas.Exemplaar });
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                mvm.BoekCreateViewModel.Themas = new SelectList(gebruiker.LetterTuin.Themas.ToList());
                return View(new MainViewModel(gebruiker) { VertelTasCreateViewModel = vertelTas });
            }
        }


        // GET: Verteltas/Delete
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);

            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                VertelTas vertelTas = gebruiker.LetterTuin.GetVertelTas(id);
                if (vertelTas == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetVerteltasViewModel(vertelTas));
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
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                MainViewModel mvm = new MainViewModel(gebruiker);
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                VertelTas vertelTas = gebruiker.LetterTuin.GetVertelTas(id);
                if (vertelTas == null)
                    return HttpNotFound();
                gebruiker.RemoveVertelTas(vertelTas);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("Verteltas" + vertelTas.Titel + " werd verwijderd...");
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
                string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"), rnd + pic);
                ;

                // file is uploaded
                newimage.SaveAs(path);
                mvm.BoekCreateViewModel.Boek.image = rnd + pic;
                return Json(new { imagePath = rnd + pic });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}