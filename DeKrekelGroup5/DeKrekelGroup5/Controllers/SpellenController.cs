using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
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
    public class SpellenController : Controller
    {
        private IGebruikerRepository gebruikersRep;

        public SpellenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: Spellen
        public ActionResult Index(Gebruiker gebruiker, String search = null)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<Spel> spellen;
                if (!String.IsNullOrEmpty(search))
                {
                    spellen = gebruiker.LetterTuin.GetSpellen(search).ToList();
                    //ViewBag.Selection = "Alle spellen met " + search;
                }
                else
                {
                    spellen = gebruiker.LetterTuin.GetSpellen(null).ToList();
                    //ViewBag.Selection = "Alle spellen";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("SpellenLijst", new MainViewModel(gebruiker).SetNewSpellenLijstVm(spellen));

                return View(new MainViewModel(gebruiker).SetNewSpellenLijstVm(spellen));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Spellen/Details/5
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
                Spel spel = gebruiker.LetterTuin.GetSpel(id) as Spel;
                if (spel == null)
                    mvm.SetNewInfo("Spel niet gevonden");
                return View("Details", mvm.SetSpelViewModel(spel));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        // GET: Spellen/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {

                mvm.SetSpelCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new Spel());
                HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "SpelCreateViewModel")] SpelCreateViewModel spel, Gebruiker gebruiker, MainViewModel mvm, string image = null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && spel != null && spel.Spel.Exemplaar <= 0)
            {
                try
                {
                    if (!image.IsNullOrWhiteSpace())
                    {
                        WebClient Client = new WebClient();
                        //WebRequest request = FtpWebRequest.Create(image);

                        //using (WebResponse response = request.GetResponse())
                        //{
                        //    Stream responseStream = response.GetResponseStream();
                        //    responseStream.CopyTo();
                        //    fb.InputStream.Read() = responseStream;
                        //}

                        //HttpPostedFileBase fb = new HttpPostedFileWrapper();
                        //fb.InputStream.BeginRead(); 
                        spel.Spel.image = spel.Spel.Titel.Replace(" ", "_") + ".jpg";
                        string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"), spel.Spel.image);
                        Client.DownloadFile(image, path);
                    }
                    else
                    {
                        spel.Spel.image = mvm.SpelCreateViewModel.Spel.image;
                    }

                    List<Thema> themas = gebruiker.GetThemaListFromSelectedList(spel.SubmittedThemas);
                    Spel newSpel = spel.Spel.MapToSpel(spel.Spel, themas);

                    gebruiker.AddSpel(newSpel);
                    //gebruikersRep.DoNotDuplicateThema(newSpel);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("Spel" + spel.Spel.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Spellen.Max(b => b.Exemplaar) });
                }
                catch (NullReferenceException)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

            }
            return RedirectToAction("Create");
        }

        // GET: Spellen/Edit/5
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
                Spel spel = gebruiker.LetterTuin.GetSpel(id);
                if (spel == null)
                    return HttpNotFound();
                mvm.SetSpelCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), spel);
                HttpContext.Session["main"] = mvm;

                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SpelCreateViewModel")] SpelCreateViewModel spel, Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage = null)
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
                if (ModelState.IsValid && spel != null && spel.Spel.Exemplaar > 0)
                {
                    try
                    {
                        spel.Spel.image = mvm.SpelCreateViewModel.Spel.image;

                        List<Thema> themas = gebruiker.GetThemaListFromSelectedList(spel.SubmittedThemas);
                        Spel newSpel = spel.Spel.MapToSpel(spel.Spel, themas);
                        gebruiker.UpdateSpel(newSpel);
                        //gebruikersRep.DoNotDuplicateThema(newSpel);
                        mvm.SetNewInfo("Spel " + spel.Spel.Titel + " werd aangepast...");
                        gebruikersRep.SaveChanges();
                        return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = newSpel.Exemplaar });
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                mvm.SpelCreateViewModel.Themas = new SelectList(gebruiker.LetterTuin.Themas.ToList());
                return View(new MainViewModel(gebruiker) { SpelCreateViewModel = spel });
            }



        }

        // GET: Spellen/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Spel spel = gebruiker.LetterTuin.GetSpel(id);
                if (spel == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetSpelViewModel(spel));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: Spellen/Delete/5
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
                Spel spel = gebruiker.LetterTuin.GetSpel(id);
                if (spel == null)
                    return HttpNotFound();
                gebruiker.RemoveSpel(spel);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("Spel" + spel.Titel + " werd verwijderd...");
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
                mvm.SpelCreateViewModel.Spel.image = rnd + pic;
                return Json(new { imagePath = rnd + pic });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}
