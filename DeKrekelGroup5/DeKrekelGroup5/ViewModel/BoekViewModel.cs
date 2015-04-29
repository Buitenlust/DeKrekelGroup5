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


        public Boek MapToBoek(BoekViewModel vm, List<Thema> themas )
        {
            return new Boek()
            {
                Exemplaar = vm.Exemplaar,
                Auteur = vm.Auteur,
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
                Themas = b.Themas,
                image = b.ImageString,
                Beschikbaar = b.Beschikbaar,
                EindDatumUitlening = (b.Uitleningen == null||  b.Uitleningen.Count == 0) ? new DateTime() : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (b.Uitleningen == null || b.Uitleningen.Count == 0) ? false : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public BoekenLijstViewModel()
        {
            Boeken = new List<BoekViewModel>();
        }
    }
  

    public class BoekCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }

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
                image = boek.ImageString,
                Themas = themas.ToList(),
                Beschikbaar = boek.Beschikbaar,
                EindDatumUitlening = (boek.Uitleningen == null||  boek.Uitleningen.Count == 0) ? new DateTime() : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (boek.Uitleningen == null || boek.Uitleningen.Count == 0) ? false : boek.Uitleningen.SingleOrDefault(d => d.Id == boek.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            //Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa",(boek.Themas == null || boek.Themas.Count == 0) ? null : boek.Themas.Select(t => t.IdThema));
        }

        public BoekCreateViewModel()
        {
            Boek = new BoekViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
        }
    }


}