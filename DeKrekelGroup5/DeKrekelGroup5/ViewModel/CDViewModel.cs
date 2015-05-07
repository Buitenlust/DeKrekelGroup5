using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    
        public class CDViewModel : ItemViewModel
        {
            [Required(ErrorMessage = "Geef een Artiest in aub...")]
            [MaxLength(45, ErrorMessage = "De naam van de artiest is te lang (max. 45 tekens)")]
            public string Artiest { get; set; }
            [Required(ErrorMessage = "Geef een uitgever in aub...")]
            [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
            public string Uitgever { get; set; }

            public CDViewModel()
            {

            }

            public CD MapToCD(CDViewModel vm, Thema thema)
            {
                return new CD()
                {
                    Exemplaar = vm.Exemplaar,
                    Artiest = vm.Artiest,
                    Beschikbaar = vm.Beschikbaar,
                    Leeftijd = vm.Leeftijd,
                    Omschrijving = vm.Omschrijving,
                    Titel = vm.Titel,
                    Themaa = thema,
                    Uitgever = Uitgever
                };
            }
        }

        public class CDLijstViewModel
        {
            public IEnumerable<CDViewModel> CDs { get; set; }

            public CDLijstViewModel(IEnumerable<CD> cds)
            {
                CDs = cds.Select(c => new CDViewModel()
                {
                    Exemplaar = c.Exemplaar,
                    Titel = c.Titel,
                    Omschrijving = c.Omschrijving,
                    Artiest = c.Artiest,
                    Uitgever = c.Uitgever,
                    Leeftijd = c.Leeftijd,
                    Thema = c.Themaa.Themaa,
                    Beschikbaar = c.Beschikbaar,
                EindDatumUitlening = c.Uitleningen.Count == 0? new DateTime() : c.Uitleningen.SingleOrDefault(d => d.Id == c.Uitleningen.Max(b => b.Id)).EindDatum,
                Uitgeleend = c.Uitleningen.Count == 0? false: c.Uitleningen.SingleOrDefault(d => d.Id == c.Uitleningen.Max(b => b.Id)).BinnenGebracht.Year == 1
                });
            }

            public CDLijstViewModel()
            {
                CDs = new List<CDViewModel>();
            }
        }


        public class CDCreateViewModel
        {
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
                    Thema = (cd.Themaa == null ? "" : cd.Themaa.Themaa),
                    Beschikbaar = cd.Beschikbaar,
                    EindDatumUitlening = (cd.Uitleningen == null || cd.Uitleningen.Count == 0) ? new DateTime() : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).EindDatum,
                    Uitgeleend = (cd.Uitleningen == null || cd.Uitleningen.Count == 0) ? false : cd.Uitleningen.SingleOrDefault(d => d.Id == cd.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
                };

                Themas = new SelectList(themas, "Themaa", "Themaa", CD.Thema ?? "");
            }

            public CDCreateViewModel()
            {
                CD = new CDViewModel();
                IEnumerable<Thema> themas = new List<Thema>();
                Themas = new SelectList(themas, "", "", "");
            }
        }
    
}