using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class BoekViewModel:ItemViewModel
    {
        [Required(ErrorMessage = "Geef een auteur in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de auteur is te lang (max. 45 tekens)")]
        public string Auteur { get; set; }
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }



        public Boek MapToBoek(BoekViewModel vm, Thema thema)
        {
            return new Boek()
            {
                Exemplaar = vm.Exemplaar,
                Auteur = vm.Auteur,
                Beschikbaar = vm.Beschikbaar,
                Uitgeleend = vm.Uitgeleend,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themaa = thema,
                Uitgever = Uitgever
            };
        }


    }

    public class BoekenLijstViewModel
    {
        public IEnumerable<BoekViewModel> Boeken { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBibliothecaris { get; set; }

        public BoekenLijstViewModel(IEnumerable<Boek> boeken)
        {
            Boeken = boeken.Select(b => new BoekViewModel(){
                Exemplaar = b.Exemplaar,
                Titel = b.Titel,
                Omschrijving = b.Omschrijving,
                Auteur = b.Auteur,
                Uitgever = b.Uitgever,
                Leeftijd = b.Leeftijd, 
                Thema = b.Themaa.Themaa
            });
        }
    }
  

    public class BoekCreateViewModel
    {
        public SelectList Themas { get; set; }
        public BoekViewModel Boek { get; set; }

        public BoekCreateViewModel(IEnumerable<Thema> themas , Boek boek)
        {
            Boek = new BoekViewModel()
            {
                Exemplaar = boek.Exemplaar,
                Omschrijving = boek.Omschrijving,
                Titel = boek.Titel,
                Auteur = boek.Auteur,
                Uitgever = boek.Uitgever,
                Leeftijd = boek.Leeftijd,
                Thema = (boek.Themaa == null ? "" : boek.Themaa.Themaa)
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
        }
    }


}