using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class DVDViewModel : ItemViewModel
    {
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }

        public DVDViewModel() 
        {
             
        }

        public DVD MapToDVD(DVDViewModel vm, List<Thema> themas)
        {
            return new DVD()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themas = themas,
                Uitgever = Uitgever,
                ImageString = image
            };
        }
    }

    public class DVDLijstViewModel
    {
        public IEnumerable<DVDViewModel> DVDs { get; set; }

        public DVDLijstViewModel(IEnumerable<DVD> dvds)
        {
            DVDs = dvds.Select(b => new DVDViewModel()
            {
                Exemplaar = b.Exemplaar,
                Titel = b.Titel,
                Omschrijving = b.Omschrijving,
                Uitgever = b.Uitgever,
                Leeftijd = b.Leeftijd,
                Themas = b.Themas,
                image = b.ImageString,
                Beschikbaar = b.Beschikbaar,
                EindDatumUitlening = (b.Uitleningen == null || b.Uitleningen.Count == 0) ? new DateTime() : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (b.Uitleningen == null || b.Uitleningen.Count == 0) ? false : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public DVDLijstViewModel()
        {
            DVDs = new List<DVDViewModel>();
        }
    }

    public class DVDCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }
        public SelectList Themas { get; set; }
        public DVDViewModel DVD { get; set; }

        public DVDCreateViewModel(IEnumerable<Thema> themas, DVD dvd)
        {
            DVD = new DVDViewModel()
            {
                Exemplaar = dvd.Exemplaar,
                Omschrijving = dvd.Omschrijving,
                Titel = dvd.Titel,
                Uitgever = dvd.Uitgever,
                Leeftijd = dvd.Leeftijd,
                image = dvd.ImageString,
                Themas = themas.ToList(),
                Beschikbaar = dvd.Beschikbaar,
                EindDatumUitlening = (dvd.Uitleningen == null || dvd.Uitleningen.Count == 0) ? new DateTime() : dvd.Uitleningen.SingleOrDefault(d => d.Id == dvd.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (dvd.Uitleningen == null || dvd.Uitleningen.Count == 0) ? false : dvd.Uitleningen.SingleOrDefault(d => d.Id == dvd.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (dvd.Themas == null || dvd.Themas.Count == 0) ? null : dvd.Themas.Select(t => t.IdThema));
        }

        public DVDCreateViewModel()
        {
            DVD = new DVDViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
        }
    }
}