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


        public DVDViewModel DVDViewModel { get; set; }
        public DVDLijstViewModel DVDLijstViewModel { get; set; }
        public DVDCreateViewModel DVDCreateViewModel { get; set; }

        public SpelViewModel SpelViewModel { get; set; }

        public UitlenerViewModel UitlenerViewModel { get; set; }
        public UitlenersLijstViewModel UitlenersLijstViewModel { get; set; }

        


        public MainViewModel()
        {
            GebruikerViewModel = new GebruikerViewModel();
            BoekViewModel = new BoekViewModel();
            BoekenLijstViewModel = new BoekenLijstViewModel();
            BoekCreateViewModel = new BoekCreateViewModel();
            DVDViewModel = new DVDViewModel();
            DVDLijstViewModel = new DVDLijstViewModel();
            DVDCreateViewModel = new DVDCreateViewModel();
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
            DVDViewModel = new DVDViewModel();
            DVDLijstViewModel = new DVDLijstViewModel();
            DVDCreateViewModel = new DVDCreateViewModel();
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

        public object SetNewDVDLijstVm(IEnumerable<DVD> dvds)
        {
            DVDLijstViewModel = new DVDLijstViewModel(dvds);
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

        public object SetDVDViewModel(DVD dvd)
        {
            DVDViewModel = new DVDViewModel()
            {
                Exemplaar = dvd.Exemplaar,
                Omschrijving = dvd.Omschrijving,
                Titel = dvd.Titel,
                Uitgever = dvd.Uitgever,
                Leeftijd = dvd.Leeftijd,
                Thema = dvd.Themaa.Themaa,
                Beschikbaar = dvd.Beschikbaar,
                EindDatumUitlening = dvd.Uitleningen.Count == 0 ? new DateTime() : dvd.Uitleningen.SingleOrDefault(d => d.Id == dvd.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = dvd.Uitleningen.Count == 0 ? false : dvd.Uitleningen.SingleOrDefault(d => d.Id == dvd.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };
            return this;
        }

        public object SetDVDCreateViewModel(IEnumerable<Thema> themas, DVD dvd)
        {
            DVDCreateViewModel = new DVDCreateViewModel(themas, dvd);
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