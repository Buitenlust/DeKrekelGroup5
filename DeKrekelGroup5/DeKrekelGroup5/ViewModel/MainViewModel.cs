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

        public DVDViewModel DVDViewModel { get; set; }
        public DVDLijstViewModel DVDLijstViewModel { get; set; }
        public DVDCreateViewModel DVDCreateViewModel { get; set; }

        public SpelViewModel SpelViewModel { get; set; }
        public SpellenLijstViewModel SpellenLijstViewModel { get; set; }
        public SpelCreateViewModel SpelCreateViewModel { get; set; }

        public UitlenerViewModel UitlenerViewModel { get; set; }
        public UitlenersLijstViewModel UitlenersLijstViewModel { get; set; }
        public List<Thema> Themas { get; set; }
        public ThemaViewModel ThemaViewModel { get; set; }

        



        public MainViewModel()
        {
            GebruikerViewModel = new GebruikerViewModel();
            BoekViewModel = new BoekViewModel();
            BoekenLijstViewModel = new BoekenLijstViewModel();
            BoekCreateViewModel = new BoekCreateViewModel();
            DVDViewModel = new DVDViewModel();
            DVDLijstViewModel = new DVDLijstViewModel();
            DVDCreateViewModel = new DVDCreateViewModel();
            CDViewModel = new CDViewModel();
            CDLijstViewModel = new CDLijstViewModel();
            CDCreateViewModel = new CDCreateViewModel();
            SpelViewModel = new SpelViewModel();
            SpellenLijstViewModel = new SpellenLijstViewModel();
            SpelCreateViewModel = new SpelCreateViewModel();
            UitlenerViewModel = new UitlenerViewModel();
            UitlenersLijstViewModel = new UitlenersLijstViewModel();
            ItemViewModel = new ItemViewModel();
            InfoViewModel = new InfoViewModel();
            ThemaViewModel = new ThemaViewModel();
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
            DVDViewModel = new DVDViewModel();
            DVDLijstViewModel = new DVDLijstViewModel();
            DVDCreateViewModel = new DVDCreateViewModel();
            SpelViewModel = new SpelViewModel();
            SpellenLijstViewModel = new SpellenLijstViewModel();
            SpelCreateViewModel = new SpelCreateViewModel();
            UitlenerViewModel = new UitlenerViewModel();
            UitlenersLijstViewModel = new UitlenersLijstViewModel();
            ItemViewModel = new ItemViewModel();
            InfoViewModel = new InfoViewModel();
            ThemaViewModel = new ThemaViewModel();
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

        public object SetNewSpellenLijstVm(IEnumerable<Spel> spellen)
        {
            SpellenLijstViewModel = new SpellenLijstViewModel(spellen);
            return this;
        }

        public object SetNewCDLijstVm(IEnumerable<CD> cds)
        {
            CDLijstViewModel = new CDLijstViewModel(cds);
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
                Themas = boek.Themas,
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
                Themas = cd.Themas,
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

        public object SetDVDViewModel(DVD dvd)
        {
            DVDViewModel = new DVDViewModel()
            {
                Exemplaar = dvd.Exemplaar,
                Omschrijving = dvd.Omschrijving,
                Titel = dvd.Titel,
                Uitgever = dvd.Uitgever,
                Leeftijd = dvd.Leeftijd,
                Themas = dvd.Themas,
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

        public object SetSpelViewModel(Spel spel)
        {
            SpelViewModel = new SpelViewModel()
            {
                Exemplaar = spel.Exemplaar,
                Omschrijving = spel.Omschrijving,
                Titel = spel.Titel,
                Uitgever = spel.Uitgever,
                Leeftijd = spel.Leeftijd,
                Themas = spel.Themas,
                Beschikbaar = spel.Beschikbaar,
                EindDatumUitlening = spel.Uitleningen.Count == 0 ? new DateTime() : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = spel.Uitleningen.Count == 0 ? false : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };
            return this;
        }

        public object SetSpelCreateViewModel(IEnumerable<Thema> themas, Spel spel)
        {
            SpelCreateViewModel = new SpelCreateViewModel(themas, spel);
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
                Themas = item.Themas,
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