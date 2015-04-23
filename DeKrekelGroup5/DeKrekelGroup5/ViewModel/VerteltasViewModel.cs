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

        public VertelTas MapToVertelTas(VerteltasViewModel vm, Thema thema)
        {
            return new VertelTas()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themaa = thema,
            };
        }

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
                Thema = v.Themaa.Themaa,
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
        public SelectList Themas { get; set; }
        public VerteltasViewModel Verteltas { get; set; }

        public VertelTasCreateViewModel(IEnumerable<Thema> themas, VertelTas vertelTas)
        {
            Verteltas = new VerteltasViewModel()
            {
                Exemplaar = vertelTas.Exemplaar,
                Omschrijving = vertelTas.Omschrijving,
                Titel = vertelTas.Titel,
                Leeftijd = vertelTas.Leeftijd,
                Thema = (vertelTas.Themaa == null ? "" : vertelTas.Themaa.Themaa),
                Beschikbaar = vertelTas.Beschikbaar,
                EindDatumUitlening = vertelTas.Uitleningen.Count == 0 ? new DateTime() : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = vertelTas.Uitleningen.Count == 0 ? false : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            Themas = new SelectList(themas, "Themaa", "Themaa", Verteltas.Thema ?? "");
        }

        public VertelTasCreateViewModel()
        {
            Verteltas = new VerteltasViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
        }
    }



}