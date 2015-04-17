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
                Thema = b.Themaa.Themaa
            });
        }
    }

    public class DVDCreateViewModel
    {
        public SelectList Themas { get; set; }
        public DVDViewModel DVD { get; set; }

        public DVDCreateViewModel(IEnumerable<Thema> themas, DVD dvd)
        {
            Boek = new BoekViewModel()
            {
                Exemplaar = dvd.Exemplaar,
                Omschrijving = dvd.Omschrijving,
                Titel = dvd.Titel,
                Uitgever = dvd.Uitgever,
                Leeftijd = dvd.Leeftijd,
                Thema = (dvd.Themaa == null ? "" : dvd.Themaa.Themaa)
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
        }
    }
}