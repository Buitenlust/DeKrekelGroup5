using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class SpelViewModel:ItemViewModel
    {
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }

        public SpelViewModel() 
        {
             
        }

        public Spel MapToSpel(SpelViewModel vm, Thema thema)
        {
            return new Spel()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themaa = thema,
                Uitgever = Uitgever
            };
        }
    }

    public class SpellenLijstViewModel
    {
        public IEnumerable<SpelViewModel> Spellen { get; set; }

        public SpellenLijstViewModel(IEnumerable<Spel> spellen)
        {
            Spellen = spellen.Select(spel => new SpelViewModel(){
                Exemplaar = spel.Exemplaar,
                Titel = spel.Titel,
                Omschrijving = spel.Omschrijving, 
                Uitgever = spel.Uitgever,
                Leeftijd = spel.Leeftijd, 
                Thema = spel.Themaa.Themaa,
                Beschikbaar = spel.Beschikbaar,
                EindDatumUitlening = spel.Uitleningen.Count == 0 ? new DateTime() : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = spel.Uitleningen.Count == 0 ? false : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public SpellenLijstViewModel()
        {
            Spellen = new List<SpelViewModel>();
        }
    }
  

    public class SpelCreateViewModel
    {
        public SelectList Themas { get; set; }
        public SpelViewModel Spel { get; set; }

        public SpelCreateViewModel(IEnumerable<Thema> themas , Spel spel)
        {
            Spel = new SpelViewModel()
            {
                Exemplaar = spel.Exemplaar,
                Omschrijving = spel.Omschrijving,
                Titel = spel.Titel, 
                Uitgever = spel.Uitgever,
                Leeftijd = spel.Leeftijd,
                Thema = (spel.Themaa == null ? "" : spel.Themaa.Themaa),
                Beschikbaar = spel.Beschikbaar,
                EindDatumUitlening = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? new DateTime() : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? false : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Spel.Thema ?? "");
        }

        public SpelCreateViewModel()
        {
            Spel = new SpelViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
        }
    }


}