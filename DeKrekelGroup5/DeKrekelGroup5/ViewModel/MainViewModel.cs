using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class MainViewModel
    {
        public InfoViewModel InfoViewModel { get; set; }
        public GebruikerViewModel GebruikerViewModel { get; set; }

        public ItemViewModel ItemViewModel { get; set; }

        public BoekViewModel BoekViewModel { get; set; }
        public BoekenLijstViewModel BoekenLijstViewModel { get; set; }
        public BoekCreateViewModel BoekCreateViewModel { get; set; }

        public CDViewModel CDViewModel { get; set; }
        public CDLijstViewModel CDLijstViewModel { get; set; }
        public CDCreateViewModel CDCreateViewModel { get; set; }

        public SpelViewModel SpelViewModel { get; set; }

        public UitlenerViewModel UitlenerViewModel { get; set; }
        public UitlenersLijstViewModel UitlenersLijstViewModel { get; set; }

        


        public MainViewModel()
        {
            GebruikerViewModel = new GebruikerViewModel();
            BoekViewModel = new BoekViewModel();
            BoekenLijstViewModel = new BoekenLijstViewModel();
            BoekCreateViewModel = new BoekCreateViewModel();
            CDViewModel = new CDViewModel();
            CDLijstViewModel = new CDLijstViewModel();
            CDCreateViewModel = new CDCreateViewModel();
            SpelViewModel = new SpelViewModel();
            UitlenerViewModel = new UitlenerViewModel();
            UitlenersLijstViewModel = new UitlenersLijstViewModel();
            ItemViewModel = new ItemViewModel();
            InfoViewModel = new InfoViewModel();
        }

        public MainViewModel(Gebruiker gebruiker)
        {
            if(gebruiker == null)
                gebruiker = new Gebruiker();
            GebruikerViewModel = new GebruikerViewModel()
            {
                Username = gebruiker.GebruikersNaam,
                Paswoord = gebruiker.PaswoordHashed,
                IsBibliothecaris = gebruiker.BibliotheekRechten,
                IsBeheerder = gebruiker.AdminRechten
            };
            BoekViewModel = new BoekViewModel();
            BoekenLijstViewModel = new BoekenLijstViewModel();
            BoekCreateViewModel = new BoekCreateViewModel();
            CDViewModel = new CDViewModel();
            CDLijstViewModel = new CDLijstViewModel();
            CDCreateViewModel = new CDCreateViewModel();
            SpelViewModel = new SpelViewModel();
            UitlenerViewModel = new UitlenerViewModel();
            UitlenersLijstViewModel = new UitlenersLijstViewModel();
            ItemViewModel = new ItemViewModel();
            InfoViewModel = new InfoViewModel();
        }

        public void SetGebruikerToVm(Gebruiker gebruiker)
        {
            GebruikerViewModel = new GebruikerViewModel()
            {
                Username = gebruiker.GebruikersNaam,
                Paswoord = gebruiker.PaswoordHashed,
                IsBibliothecaris = gebruiker.BibliotheekRechten,
                IsBeheerder = gebruiker.AdminRechten
            };
        }

        public object SetNewBoekenLijstVm(IEnumerable<Boek> boeken)
        {
                BoekenLijstViewModel = new BoekenLijstViewModel(boeken);
            return this;
        }

        public object SetNewCDLijstVm(IEnumerable<CD> cds)
        {
            CDLijstViewModel = new CDLijstViewModel(cds);
            return this;
        }

        public object SetNewUitlenersLijstVm(IEnumerable<Uitlener> uitleners)
        {
            UitlenersLijstViewModel = new UitlenersLijstViewModel(uitleners);
            return this;
        }

        public object SetBoekViewModel(Boek boek)
        {
            BoekViewModel = new BoekViewModel()
            {
                Exemplaar = boek.Exemplaar,
                Omschrijving = boek.Omschrijving,
                Titel = boek.Titel,
                Auteur = boek.Auteur,
                Uitgever = boek.Uitgever,
                Leeftijd = boek.Leeftijd,
                Thema = boek.Themaa.Themaa,
                Beschikbaar = boek.Beschikbaar,
                EindDatumUitlening = boek.Uitleningen.Count == 0 ? new DateTime() : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = boek.Uitleningen.Count == 0 ? false : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };
            return this;
        }

        public object SetBoekCreateViewModel(IEnumerable<Thema> themas, Boek boek)
        {
             BoekCreateViewModel = new BoekCreateViewModel(themas, boek);
            return this;
        }

        public object SetCDViewModel(CD cd)
        {
            CDViewModel = new CDViewModel()
            {
                Exemplaar = cd.Exemplaar,
                Omschrijving = cd.Omschrijving,
                Titel = cd.Titel,
                Artiest = cd.Artiest,
                Uitgever = cd.Uitgever,
                Leeftijd = cd.Leeftijd,
                Thema = cd.Themaa.Themaa,
                Beschikbaar = cd.Beschikbaar,
                EindDatumUitlening = cd.Uitleningen.Count == 0 ? new DateTime() : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = cd.Uitleningen.Count == 0 ? false : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };
            return this;
        }

        public object SetCDCreateViewModel(IEnumerable<Thema> themas, CD cd)
        {
            CDCreateViewModel = new CDCreateViewModel(themas, cd);
            return this;
        }

        public void SetItemViewModel(Item item)
        {
            ItemViewModel = new ItemViewModel()
            {
                Exemplaar = item.Exemplaar,
                Omschrijving = item.Omschrijving,
                Titel = item.Titel,
                Leeftijd = item.Leeftijd,
                Thema = item.Themaa.Themaa,
                EindDatumUitlening = item.Uitleningen.Count == 0 ? new DateTime() : item.Uitleningen.SingleOrDefault(d => d.Id == item.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = item.Uitleningen.Count == 0 ? false : item.Uitleningen.SingleOrDefault(d => d.Id == item.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            }; 
        }

        public void SetUitLenerViewModel(Uitlener uitlener)
        {
            UitlenerViewModel = new UitlenerViewModel(uitlener);
        }

        public Object SetNewInfo(String info, bool isError=false,bool isDialogBox=false, string callBackAction= null)
        {
            InfoViewModel = new InfoViewModel()
            {
                Info = info,
                IsError = isError,
                IsDialogBox = isDialogBox, 
                CallBackAction = callBackAction
            };
            return this;
        }
    }

    

}