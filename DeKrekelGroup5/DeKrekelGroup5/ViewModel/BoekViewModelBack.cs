using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class BoekViewModelBackup:ItemViewModel
    {
        [Required(ErrorMessage = "Geef een auteur in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de auteur is te lang (max. 45 tekens)")]
        public string Auteur { get; set; }
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }

        public BoekViewModelBackup(Boek boek) : base(boek)
        {
            Auteur = boek.Auteur;
            Uitgever = boek.Uitgever;
        }
    }

    public class BoekenViewModel
    {
        public IEnumerable<BoekViewModelBackup> Boeken { get; set; }

        public BoekenViewModel(IEnumerable<Boek> boeken)
        {
            Boeken = boeken.OfType<Boek>().Select(p => new BoekViewModelBackup(p));
        }
    }

    //public class BoekIndexViewModel
    //{
    //    public BoekIndexViewModel(Boek b)
    //    {
    //        Exemplaar = b.Exemplaar;
    //        Titel = b.Titel;
    //        Omschrijving = b.Omschrijving;
    //        Auteur = b.Auteur;
    //        Uitgever = b.Uitgever;
    //        Leeftijd = b.Leeftijd; 
    //        Thema = b.Themaa.Themaa;
    //    }

    //    public int Exemplaar { get; set; }
    //    public string Titel { get; set; }
    //    public string Omschrijving { get; set; }
    //    public string Auteur { get; set; }
    //    public string Uitgever { get; set; }
    //    public int Leeftijd { get; set; } 
    //    public string Thema { get; set; }
    //}

    public class BoekCreateViewModel
    {
        public int Exemplaar { get; set; }
        [Required(ErrorMessage = "Geef een titel in aub...")]
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public string Auteur { get; set; }
        public string Uitgever { get; set; }
        [Required(ErrorMessage = "Geef een jaartal tussen 1900 & 2100 in aub...")]
        [Display(Name = "Leeftijd")]
        [Range(0, 99)]
        public int Leeftijd { get; set; }
        [Required(ErrorMessage = "Kies een thema aub...")]
        public string Thema { get; set; }
    }

    public class BoekThemaCreateViewModel
    {
        public SelectList Themas { get; set; }
        public BoekCreateViewModel Boek { get; set; }

        public BoekThemaCreateViewModel(IEnumerable<Thema> themas , BoekCreateViewModel boek)
        {
            Themas = new SelectList(themas, "Themaa", "Themaa", boek.Thema ?? "");
            Boek = boek;
        }
    }


}