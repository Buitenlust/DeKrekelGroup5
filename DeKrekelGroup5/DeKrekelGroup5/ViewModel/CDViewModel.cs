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
    public class CDViewModel : ItemViewModel
    {
        [Required(ErrorMessage = "Geef een artiest in aub...")]
        [MaxLength(200, ErrorMessage = "De naam van de artiest is te lang (max. 200 tekens)")]
        public string Artiest { get; set; }
        [MaxLength(200, ErrorMessage = "De naam van de uitgever is te lang (max. 200 tekens)")]
        public string Uitgever { get; set; }

        public CDViewModel()
        {

        }


        public CD MapToCD(CDViewModel vm, List<Thema> themas)
        {
            return new CD()
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

    public class CDsLijstViewModel
    {
        public IEnumerable<CDViewModel> CDs { get; set; }

        public CDsLijstViewModel(IEnumerable<CD> cden)
        {
            CDs = cden.Select(b => new CDViewModel()
            {
                Exemplaar = b.Exemplaar,
                Titel = b.Titel,
                Omschrijving = b.Omschrijving.Length > 700 ? b.Omschrijving.Substring(0, 700) + "..." : b.Omschrijving,
                Uitgever = b.Uitgever,
                Artiest = b.Artiest,
                Leeftijd = b.Leeftijd,
                Themas = b.Themas,
                image = b.ImageString,
                Beschikbaar = b.Beschikbaar,
                EindDatumUitlening = (b.Uitleningen == null || b.Uitleningen.Count == 0) ? new DateTime() : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (b.Uitleningen == null || b.Uitleningen.Count == 0) ? false : b.Uitleningen.SingleOrDefault(d => d.Id == b.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            });
        }

        public CDsLijstViewModel()
        {
            CDs = new List<CDViewModel>();
        }
    }


    public class CDCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }

        public SelectList Themas { get; set; }
        public CDViewModel CD { get; set; }

        public CDCreateViewModel(IEnumerable<Thema> themas, CD cd)
        {
            CD = new CDViewModel()
            {
                Exemplaar = cd.Exemplaar,
                Omschrijving = cd.Omschrijving,
                Titel = cd.Titel,
                Artiest = cd.Artiest,
                Uitgever = cd.Uitgever,
                Leeftijd = cd.Leeftijd,
                image = cd.ImageString,
                Themas = themas.ToList(),
                Beschikbaar = cd.Beschikbaar,
                EindDatumUitlening = (cd.Uitleningen == null || cd.Uitleningen.Count == 0) ? new DateTime() : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (cd.Uitleningen == null || cd.Uitleningen.Count == 0) ? false : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            //Themas = new SelectList(themas, "Themaa", "Themaa", CD.Thema ?? "");
            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (cd.Themas == null || cd.Themas.Count == 0) ? null : cd.Themas.Select(t => t.IdThema));
        }

        public CDCreateViewModel()
        {
            CD = new CDViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
        }
    }


}