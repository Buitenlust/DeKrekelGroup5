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

        public Spel MapToSpel(SpelViewModel vm, List<Thema> themas)
        {
            return new Spel()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themas = themas,
                ImageString = image,
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
                Themas = spel.Themas,
                image = spel.ImageString,
                Beschikbaar = spel.Beschikbaar,
                EindDatumUitlening = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? new DateTime() : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? false : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public SpellenLijstViewModel()
        {
            Spellen = new List<SpelViewModel>();
        }
    }
  

    public class SpelCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }

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
                image = spel.ImageString,
                Themas = themas.ToList(),
                Beschikbaar = spel.Beschikbaar,
                EindDatumUitlening = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? new DateTime() : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (spel.Uitleningen == null || spel.Uitleningen.Count == 0) ? false : spel.Uitleningen.SingleOrDefault(d => d.Id == spel.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (spel.Themas == null || spel.Themas.Count == 0) ? null : spel.Themas.Select(t => t.IdThema));
        }

        public SpelCreateViewModel()
        {
            Spel = new SpelViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
        }
    }


}