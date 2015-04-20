using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public BoekViewModel() 
        {
             
        }


        public Boek MapToBoek(BoekViewModel vm, Thema thema)
        {
            return new Boek()
            {
                Exemplaar = vm.Exemplaar,
                Auteur = vm.Auteur,
                Beschikbaar = vm.Beschikbaar,
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

        public BoekenLijstViewModel(IEnumerable<Boek> boeken)
        {
            Boeken = boeken.Select(b => new BoekViewModel(){
                Exemplaar = b.Exemplaar,
                Titel = b.Titel,
                Omschrijving = b.Omschrijving,
                Auteur = b.Auteur,
                Uitgever = b.Uitgever,
                Leeftijd = b.Leeftijd, 
                Thema = b.Themaa.Themaa,
                Beschikbaar = b.Beschikbaar,
                EindDatumUitlening = b.Uitleningen.Count == 0? new DateTime() : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = b.Uitleningen.Count == 0? false: b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public BoekenLijstViewModel()
        {
            Boeken = new List<BoekViewModel>();
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
                Thema = (boek.Themaa == null ? "" : boek.Themaa.Themaa),
                Beschikbaar = boek.Beschikbaar,
                EindDatumUitlening = boek.Uitleningen.Count == 0 ? new DateTime() : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = boek.Uitleningen.Count == 0 ? false : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
        }

        public BoekCreateViewModel()
        {
            Boek = new BoekViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
        }
    }


}