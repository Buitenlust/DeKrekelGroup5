using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class UitleningenController : Controller
    { 

        private IGebruikerRepository gebruikersRep; 

        public UitleningenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: Uitleningen/Create
        public ActionResult Create(Gebruiker gebruiker, int id=0, string search=null)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam); 
                IEnumerable<Uitlener> uitleners;
                MainViewModel mvm = new MainViewModel(gebruiker); 
                if (!String.IsNullOrEmpty(search))
                {
                    uitleners = gebruiker.GetUitleners(search);
                    ViewBag.Selection = "Alle gebruikers met " + search;
                }
                else
                {
                    uitleners = gebruiker.GetUitleners(null);
                    ViewBag.Selection = "Alle gebruikers:";
                }
                mvm.SetNewUitlenersLijstVm(uitleners);
                mvm.SetItemViewModel(gebruiker.LetterTuin.GetItem(id));
                if (Request.IsAjaxRequest())
                    return PartialView("UitlenersSelectLijst", mvm);
                 
                return View(mvm); 
            }
            catch (Exception)
            {
                return PartialView(new MainViewModel().SetNewInfo("Er is iets ernstigs fout gelopen!", true));
            }
        }

        // GET: Uitleningen/SelectUitlener?gebruikerId=5&&exemplaar=6
        public ActionResult SelectUitlener(Gebruiker gebruiker, int gebruikerId = 0, int exemplaar=0)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if (gebruikerId > 0 && exemplaar > 0)
                try
                {
                    gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                    Uitlener uitlener = gebruiker.GetUitlenerById(gebruikerId);
                    Item item = gebruiker.LetterTuin.GetItem(exemplaar);
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    if (uitlener != null && item != null)
                    {
                        mvm.SetItemViewModel(item);
                        mvm.SetUitLenerViewModel(uitlener);
                    }
                    else
                    {
                        return new HttpNotFoundResult();
                    } 
                    mvm.SetNewInfo("Wenst u " + item.Titel + " uit te lenen aan " + uitlener.VoorNaam + "?",false,true,"Confirm");
                    if (HttpContext.Session != null) 
                        HttpContext.Session["main"] = mvm;
                    return PartialView("_info", mvm);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        // POST: Uitleningen/Confirm?gebruikerId=5&&exemplaar=6
        public ActionResult Confirm(Gebruiker gebruiker, MainViewModel mvmModel)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));


            if (HttpContext.Session != null && mvmModel != null)
            {
                try
                {
                    gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                    Uitlener uitlener = gebruiker.GetUitlenerById(mvmModel.UitlenerViewModel.Id);
                    Item item = gebruiker.LetterTuin.GetItem(mvmModel.ItemViewModel.Exemplaar);
                   MainViewModel mvm = new MainViewModel(gebruiker);

                    if (gebruiker.GetOpenUitleningByItem(item.Exemplaar) == null)
                    {
                        gebruiker.VoegUitleningToe(uitlener, item);
                        gebruikersRep.SaveChanges();
                        mvm.SetNewInfo(item.Titel + " is uitgeleend aan " + uitlener.VoorNaam + ".");
                    }
                    else
                    {
                        mvm.SetNewInfo(item.Titel + " is al uitgeleend!", true);
                    }
                    
                    return PartialView("_info", mvm);
                }
                    catch (DbEntityValidationException)
                    {
                        mvmModel.SetNewInfo("Er is iets ernstigs fout gelopen!", true);
                    return PartialView("_info",mvmModel);
                }
                catch (Exception)
                {
                    mvmModel.SetNewInfo("Er is iets ernstigs fout gelopen!", true);
                    return PartialView("_info", mvmModel);
                }

               
            }
            return Redirect((Request.UrlReferrer == null) ? "" : Request.UrlReferrer.ToString());
        }

        [HttpGet]
        // POST: Uitleningen/EindeUitlening?exemplaar=6
        public ActionResult EindeUitlening(Gebruiker gebruiker, int exemplaar)
        {

            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if (exemplaar > 0)
                try
                {
                    gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                    Item item = gebruiker.LetterTuin.GetItem(exemplaar);
                    Uitlening uitlening = gebruiker.GetOpenUitleningByItem(exemplaar);
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    mvm.SetItemViewModel(item);
                    if (item != null || uitlening != null)
                    {
                        decimal boete = gebruiker.GetBoete(uitlening); 
                        mvm.SetNewInfo(item.Titel + " Terugbrengen?",false,true,"ConfirmBack");
                        if (boete > 0)
                            mvm.SetNewInfo(item.Titel + " Terugbrengen? De boete bedraagt " + boete + " euro", false, true, "ConfirmBack");
                    }
                    else
                    { 
                        return PartialView(new MainViewModel().SetNewInfo("Object niet gevonden", true));
                    }
                    if (HttpContext.Session != null)
                        HttpContext.Session["main"] = mvm;
                    return PartialView("_info", mvm);
                }

                catch (Exception)
                {
                    return PartialView(new MainViewModel().SetNewInfo("Er is iets ernstigs fout gelopen!", true));
                }
            return PartialView(new MainViewModel().SetNewInfo("U mag dit niet doen!!", true));
        }
        
        [HttpPost]
        // POST: Uitleningen/ConfirmBack
        public ActionResult ConfirmBack(Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if (HttpContext.Session != null)
            {
                try
                {
                    gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                    Item item = gebruiker.LetterTuin.GetItem(mvm.ItemViewModel.Exemplaar);
                    Uitlening uitlening = gebruiker.GetOpenUitleningByItem(mvm.ItemViewModel.Exemplaar);
                    mvm.SetGebruikerToVm(gebruiker);
                    gebruiker.EindeUitlening(uitlening);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("Einde uitlening van " + item.Titel + " werd geregistreerd!");
                    return PartialView("_info", mvm);
                }
                catch (DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                    mvm.SetNewInfo("Er is iets ernstigs fout gelopen!", true);
                    return PartialView("_info",mvm);
                }
                catch (Exception)
                {
                    mvm.SetNewInfo("Er is iets ernstigs fout gelopen!", true);
                    return PartialView("_info",mvm);
                }
            }
            return PartialView(new MainViewModel().SetNewInfo("Uw aanvraag is niet geldig!", true));
        }

        [HttpGet]
        // GET: Uitleningen/Verlengen?exemplaar=6
        public ActionResult Verlengen(Gebruiker gebruiker, int exemplaar)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                MainViewModel mvm = new MainViewModel(gebruiker);
                Item item = gebruiker.LetterTuin.GetItem(exemplaar);
                if (gebruiker.CheckVerlenging(item))
                {
                    
                    mvm.SetItemViewModel(gebruiker.LetterTuin.GetItem(exemplaar));
                    mvm.SetNewInfo("Bent u zeker dat u deze uitlening wil verlengen?", false, true, "VerlengConfirmed");
                }
                else
                {
                    mvm.SetNewInfo("Er zijn geen verlengingen meer mogelijk", true);
                    
                }
                return PartialView("_info", mvm);

            }
            catch (Exception)
            {
                return PartialView(new MainViewModel().SetNewInfo("Er is iets ernstigs fout gelopen!", true));
            }
        }

        [HttpPost]
        // Post: Uitleningen/verlengen
        public ActionResult VerlengConfirmed(Gebruiker gebruiker, MainViewModel mainViewModel)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if(mainViewModel !=null)
            try
            {
                
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                MainViewModel mvm = new MainViewModel(gebruiker);
                Item item = gebruiker.LetterTuin.GetItem(mainViewModel.ItemViewModel.Exemplaar);
                if (gebruiker.CheckVerlenging(item))
                {
                    gebruiker.VerlengUitlening(gebruiker.GetOpenUitleningByItem(mainViewModel.ItemViewModel.Exemplaar).Id);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("De uitlening is verlengd");
                }
                else
                {
                    mvm.SetNewInfo("Er zijn geen verlengingen meer mogelijk", true);

                }
                return PartialView("_info", mvm);
            }
            catch (Exception)
            {
                return PartialView(new MainViewModel().SetNewInfo("Er is iets ernstigs fout gelopen!", true));
            }
            return PartialView(new MainViewModel().SetNewInfo("Ik ben vergeten waar u het over heeft!", true));
        }

   
        
    }
}
