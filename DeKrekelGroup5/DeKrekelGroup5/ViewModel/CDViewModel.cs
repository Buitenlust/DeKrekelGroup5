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
                    Thema = c.Themaa.Themaa
                });
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
                    Thema = (cd.Themaa == null ? "" : cd.Themaa.Themaa)
                };

                Themas = new SelectList(themas, "Themaa", "Themaa", CD.Thema ?? "");
            }
        }
    
}