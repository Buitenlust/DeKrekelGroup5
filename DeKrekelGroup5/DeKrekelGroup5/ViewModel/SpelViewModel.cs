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
                Thema = spel.Themaa.Themaa
            });
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
                Thema = (spel.Themaa == null ? "" : spel.Themaa.Themaa)
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Spel.Thema ?? "");
        }
    }


}