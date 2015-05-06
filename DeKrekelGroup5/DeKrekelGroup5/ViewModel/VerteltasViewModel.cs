using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class VerteltasViewModel : ItemViewModel
    {

        public VerteltasViewModel()
        {

        }

        public VertelTas MapToVertelTas(VerteltasViewModel vm, List<Thema> themas, List<Item> items )
        {
            return new VertelTas()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themas = themas,
                Items = items
            };
        }


        public ICollection<Item> Items { get; set; }
    }

    public class VertelTasLijstViewModel
    {
        public IEnumerable<VerteltasViewModel> VertelTassen { get; set; }

        public VertelTasLijstViewModel(IEnumerable<VertelTas> verteltassen)
        {
            VertelTassen = verteltassen.Select(v => new VerteltasViewModel()
            {
                Exemplaar = v.Exemplaar,
                Titel = v.Titel,
                Omschrijving = v.Omschrijving,
                Leeftijd = v.Leeftijd,
                Themas = v.Themas,
                Items = v.Items,
                Beschikbaar = v.Beschikbaar,

                EindDatumUitlening = v.Uitleningen.Count == 0
                    ? new DateTime()
                    : v.Uitleningen.SingleOrDefault(d => d.Id == v.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = v.Uitleningen.Count == 0
                    ? false
                    : v.Uitleningen.SingleOrDefault(d => d.Id == v.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            }
        )
            ;
        }

        public VertelTasLijstViewModel()
        {
            VertelTassen = new List<VerteltasViewModel>();
        }
    }

    public class VertelTasCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }

        public SelectList Themas { get; set; }
        public SelectList Items { get; set; }
        public VerteltasViewModel Verteltas { get; set; }

        public VertelTasCreateViewModel(IEnumerable<Thema> themas, VertelTas vertelTas, IEnumerable<Item> items )
        {
            Verteltas = new VerteltasViewModel()
            {
                Exemplaar = vertelTas.Exemplaar,
                Omschrijving = vertelTas.Omschrijving,
                Titel = vertelTas.Titel,
                Leeftijd = vertelTas.Leeftijd,
                Themas = themas.ToList(),
                Items = items.ToList(),
                Beschikbaar = vertelTas.Beschikbaar,
                EindDatumUitlening = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? new DateTime() : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? false : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            //Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (vertelTas.Themas == null || vertelTas.Themas.Count == 0) ? null : vertelTas.Themas.Select(t => t.IdThema));
        }

        public VertelTasCreateViewModel()
        {
            Verteltas = new VerteltasViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
        }
    }



}