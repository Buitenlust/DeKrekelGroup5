using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using LINQtoCSV;

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
        public ActionResult Index(Gebruiker gebruiker, MainViewModel mvm, String search=null)
        { 
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            try
            {
                mvm.SetNewUitlenersLijstVm(gebruiker.GetUitleners(search).ToList());
                if (Request.IsAjaxRequest())
                    return PartialView("UitlenersLijst", mvm);
                return View(mvm);
            }
            catch (Exception)
            {
                return PartialView(mvm.SetNewInfo("De server kan uw aanvraag niet behandelen.", true));
            }
        }

        // GET: Uitleners/Details/5
        public ActionResult Details(Gebruiker gebruiker, MainViewModel mvm, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            if (id <= 0)
                return PartialView(mvm.SetNewInfo("Gebruiker niet gevonden.", true));
            try
            {
                Uitlener uitlener = gebruiker.GetUitlenerById(id);
                if (uitlener == null)
                    return PartialView(mvm.SetNewInfo("Gebruiker niet gevonden.", true));
                mvm.SetUitLenerViewModel(uitlener);
                return View(mvm);
            }
            catch (Exception)
            {
                return PartialView(mvm.SetNewInfo("De server kan uw aanvraag niet behandelen.", true));
            }
        }

        // GET: Uitleners/Create
        public ActionResult Create(Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            mvm.SetUitLenerViewModel(new Uitlener());
            return View(mvm);
        }

        // POST: Uitleners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gebruiker gebruiker, MainViewModel mvm, [Bind(Prefix= "UitlenerViewModel")] UitlenerViewModel uvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (ModelState.IsValid && uvm != null && uvm.Id == 0 )
            {
                try
                {
                    gebruiker.AddUitlener(uvm.MapNaarUitlener(uvm));
                    gebruikerRepository.SaveChanges();
                    mvm.SetNewInfo("De Student zijn gegevens werden aangepast");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Uitleners.Max(u => u.Id)});
                }
                catch (Exception)
                {
                    return PartialView(mvm.SetNewInfo("Er is een fout opgetreden", true));
                }
            }
            mvm.UitlenerViewModel = uvm;
            return View(mvm);
        }

        // GET: Uitleners/Edit/5
        public ActionResult Edit(Gebruiker gebruiker, MainViewModel mvm, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (id <= 0)
                return PartialView(mvm.SetNewInfo("Deze student bestaat niet", true));
            Uitlener uitlener = gebruiker.GetUitlenerById(id);
            if (uitlener == null)
                return PartialView(mvm.SetNewInfo("Student niet gevonden", true));
            mvm.SetUitLenerViewModel(uitlener);
            return View(mvm);
        }

        // POST: Uitleners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gebruiker gebruiker, MainViewModel mvm, [Bind(Prefix = "UitlenerViewModel")] UitlenerViewModel uvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (ModelState.IsValid  && uvm.Id > 0)
            {
                try
                {
                    gebruiker.UpdateUitlener(uvm.MapNaarUitlener(uvm)); 
                    gebruikerRepository.SaveChanges();
                    mvm.SetNewInfo("De Student zijn gegevens werden aangepast");
                    gebruiker.GetUitlenerById(mvm.UitlenerViewModel.Id);
                    return RedirectToAction("Details", new {gebruiker = gebruiker, mvm = mvm, id = uvm.Id});
                }
                catch (Exception)
                {
                    return PartialView(mvm.SetNewInfo("Er is een fout opgetreden", true));
                }
            }
            mvm.UitlenerViewModel = uvm;
            return View(mvm);
        }

        // GET: Uitleners/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, MainViewModel mvm, int id=0)       //todo heeft gebruiker nog uitleningen?
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (id <= 0)
                return PartialView(mvm.SetNewInfo("Verkeerde Id", true));
            try
            {
                Uitlener uitlener = gebruiker.GetUitlenerById(id);
                if (uitlener == null)
                    return PartialView(mvm.SetNewInfo("Student niet gevonden", true));
                mvm.SetUitLenerViewModel(uitlener);
                return View(mvm);
            }
            catch (Exception)
            {
                return PartialView(mvm.SetNewInfo("Er is een fout opgetreden", true));
            }
        }

        // POST: Uitleners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker,MainViewModel mvm, int id=0)  //todo heeft gebruiker nog uitleningen? zoja, lijst weergeven van uitleningen.
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (id <= 0)
                return PartialView(mvm.SetNewInfo("Verkeerde Id", true));
            try
            {
                Uitlener uitlener = gebruiker.GetUitlenerById(id);
                if (uitlener == null)
                    return PartialView(mvm.SetNewInfo("Student niet gevonden", true));
                gebruiker.RemoveUitlener(uitlener);
                gebruikerRepository.SaveChanges();
                mvm.SetNewInfo("Student verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return PartialView(mvm.SetNewInfo("Er is een fout opgetreden", true));
            }
        }


        // GET: Uitleners/CreateGroup
        public ActionResult CreateGroup(Gebruiker gebruiker, MainViewModel mvm)     
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
             
                return View(mvm);
        }


        [HttpPost]
        public ActionResult UploadData(Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase csv)
        {
            if (csv != null)
            {
                string pic = System.IO.Path.GetFileName(csv.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Data"), DateTime.Today.Year + DateTime.Today.Month + pic);
                csv.SaveAs(path);

                try
                {
                    CsvFileDescription inputFileDescription = new CsvFileDescription
                            {
                                SeparatorChar = ';',
                                TextEncoding = Encoding.GetEncoding(28591),
                                FirstLineHasColumnNames = true,
                                EnforceCsvColumnAttribute = true
                            };
                    CsvContext cc = new CsvContext();
                    IEnumerable<Uitlener> uitleners = cc.Read<Uitlener>(path, inputFileDescription);
                    IEnumerable<Uitlener> uit = uitleners;
                    Response.ContentType = "text/html; charset=utf-8";
                    mvm.SetNewUitlenersLijstVm(uit.ToList());
                    HttpContext.Session["main"] = mvm;
                    return Json(new { msg = "ok" });
                }
                catch (Exception)
                {
                    return PartialView(mvm.SetNewInfo("Er is een fout opgetreden", true));
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public ActionResult GroupDetails(Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 
            if (mvm.UitlenersLijstViewModel.Uitleners != null)
            {
                return PartialView("ImportUitlenersLijst", mvm);
            }
            return RedirectToAction("UploadData", mvm);
        }

        [HttpGet]
        public ActionResult SaveGroupUsers(Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(mvm.SetNewInfo("U dient eerst in te loggen", true));
            gebruiker = gebruikerRepository.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker); 

            if (mvm.UitlenersLijstViewModel.Uitleners != null)
            {

                gebruikerRepository.UpdateUitlenersEindeSchooljaar();
                gebruikerRepository.SaveChanges();
                foreach (UitlenerViewModel uitlener in mvm.UitlenersLijstViewModel.Uitleners)
                {
                    Uitlener Olduitlener = gebruiker.LetterTuin.Uitleners.Where(u => u.Naam == uitlener.Naam).FirstOrDefault(u => u.VoorNaam == uitlener.VoorNaam);

                    if (Olduitlener != null) 
                        Olduitlener.Klas = uitlener.Klas;
                    else
                    {
                        gebruiker.AddUitlener(uitlener.MapNaarUitlener(uitlener));
                    }
                }
                gebruikerRepository.SaveChanges();
                mvm.SetNewInfo("Lijst opgeslagen");
                mvm.SetNewUitlenersLijstVm(null);
                return View("index", mvm);
            }
            return RedirectToAction("UploadData", mvm);
        }



    }

}
