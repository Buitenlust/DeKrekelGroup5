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

        //// GET: Uitleningen
        //public ActionResult Index(Gebruiker gebruiker, String search = null)
        //{
        //    if (gebruiker == null)
        //        gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
        //    else
        //        gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
        //    try
        //    {
        //        //IEnumerable<Uitlening> uitleningen = gebruiker.GetUitleningenByFilters(search);

        //        if (Request.IsAjaxRequest())
        //            return PartialView("UitleningenLijst", new MainViewModel(gebruiker).SetNewUitleningenLijstVm(uitleningen));

        //        return View(new MainViewModel(gebruiker).SetNewUitleningenLijstVm(uitleningen));
        //    }
        //    catch (Exception)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        //    }
        //}


        // GET: Uitleningen/Create
        public ActionResult Create(Gebruiker gebruiker, int id=0, string search=null)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));

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
                mvm.SetBoekViewModel(gebruiker.LetterTuin.GetBoek(id));
                if (HttpContext.Session != null && Request.UrlReferrer != null)
                    HttpContext.Session["helper"] = new Helper(Request.UrlReferrer.AbsolutePath);
                if (Request.IsAjaxRequest())
                    return View("UitlenersSelectLijst", mvm);
                 
                return View(mvm); 

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
                    Boek Boek = gebruiker.LetterTuin.GetBoek(exemplaar);
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    if (uitlener != null && Boek != null)
                    {
                        mvm.SetBoekViewModel(Boek);
                        mvm.SetUitLenerViewModel(uitlener);
                    }
                    else
                    {
                        return new HttpNotFoundResult();
                    } 
                    mvm.SetNewInfo("Wenst u " + Boek.Titel + " uit te lenen aan " + uitlener.VoorNaam + "?",false,true,"Confirm");
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
                    Boek Boek = gebruiker.LetterTuin.GetBoek(mvmModel.BoekViewModel.Exemplaar);
                   MainViewModel mvm = new MainViewModel(gebruiker);

                    if (gebruiker.GetOpenUitleningByItem(Boek.Exemplaar) == null)
                    {
                        gebruiker.VoegUitleningToe(uitlener, Boek);
                        gebruikersRep.SaveChanges();
                        Helper helper = HttpContext.Session["helper"] as Helper;
                        if(helper != null)
                            return PartialView("_info", mvm.SetNewInfo(Boek.Titel + " is uitgeleend aan " + uitlener.VoorNaam + ".", false, false, helper.CallBack +"/Details/" + Boek.Exemplaar));
                        return PartialView("_info", mvm.SetNewInfo(Boek.Titel + " is uitgeleend aan " + uitlener.VoorNaam + "."));
                    }
                    else
                    {
                        mvm.SetNewInfo(Boek.Titel + " is al uitgeleend!", true);
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
                    Boek Boek = gebruiker.LetterTuin.GetBoek(exemplaar);
                    Uitlening uitlening = gebruiker.GetOpenUitleningByItem(exemplaar);
                    MainViewModel mvm = new MainViewModel(gebruiker);
                    
                    mvm.SetBoekViewModel(Boek);
                    if (HttpContext.Session != null && Request.UrlReferrer != null)
                        HttpContext.Session["helper"] = new Helper(Request.UrlReferrer.AbsolutePath);
                    if (Boek != null || uitlening != null)
                    {
                        decimal boete = gebruiker.GetBoete(uitlening); 
                        mvm.SetNewInfo(Boek.Titel + " Terugbrengen?",false,true,"ConfirmBack");
                        if (boete > 0)
                            mvm.SetNewInfo(Boek.Titel + " Terugbrengen? De boete bedraagt " + boete + " euro", false, true);
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
                    Boek Boek = gebruiker.LetterTuin.GetBoek(mvm.BoekViewModel.Exemplaar);
                    Uitlening uitlening = gebruiker.GetOpenUitleningByItem(mvm.BoekViewModel.Exemplaar);
                    mvm.SetGebruikerToVm(gebruiker);
                    gebruiker.EindeUitlening(uitlening);
                    gebruikersRep.SaveChanges();

                    Helper helper = HttpContext.Session["helper"] as Helper;
                    if (helper != null)
                        return PartialView("_info", mvm.SetNewInfo("Einde uitlening van " + Boek.Titel + " werd geregistreerd!", false, false, helper.CallBack + "/Details/" + Boek.Exemplaar));
                    return PartialView("_info", mvm.SetNewInfo("Einde uitlening van " + Boek.Titel + " werd geregistreerd!"));
                }
                catch (DbEntityValidationException dbEx)
                {
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

                if (HttpContext.Session != null)
                {
                    HttpContext.Session["main"] = mvm;
                    HttpContext.Session["helper"] = new Helper(Request.UrlReferrer.AbsolutePath);
                }
                
                Boek Boek = gebruiker.LetterTuin.GetBoek(exemplaar);
                if (gebruiker.CheckVerlenging(Boek))
                {
                    
                    mvm.SetBoekViewModel(gebruiker.LetterTuin.GetBoek(exemplaar));
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
        public ActionResult VerlengConfirmed(Gebruiker gebruiker, MainViewModel mainViewModel, Helper helper)
        {
            if (gebruiker == null || gebruiker.BibliotheekRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            if(mainViewModel !=null)
            try
            { 
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                MainViewModel mvm = new MainViewModel(gebruiker);
                Boek Boek = gebruiker.LetterTuin.GetBoek(mainViewModel.BoekViewModel.Exemplaar);
                if (gebruiker.CheckVerlenging(Boek))
                {
                    gebruiker.VerlengUitlening(gebruiker.GetOpenUitleningByItem(mainViewModel.BoekViewModel.Exemplaar).Id);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("De uitlening is verlengd");
                    if (helper != null)
                        return PartialView("_info", mvm.SetNewInfo("De uitlening is verlengd", false, false, helper.CallBack + "/Details/" + Boek.Exemplaar));
                    return PartialView("_info", mvm.SetNewInfo("De uitlening is verlengd"));
                }
                    mvm.SetNewInfo("Er zijn geen verlengingen meer mogelijk", true);
                    if (helper != null)
                        return PartialView("_info", mvm.SetNewInfo("Er zijn geen verlengingen meer mogelijk", true, false, helper.CallBack + "/Details/" + Boek.Exemplaar));
                    return PartialView("_info", mvm.SetNewInfo("Er zijn geen verlengingen meer mogelijk", true));
                
            }
            catch (Exception)
            {
                return PartialView(new MainViewModel().SetNewInfo("Er is iets ernstigs fout gelopen!", true));
            }
            return PartialView(new MainViewModel().SetNewInfo("Ik ben vergeten waar u het over heeft!", true));
        }

   
        
    }
}
