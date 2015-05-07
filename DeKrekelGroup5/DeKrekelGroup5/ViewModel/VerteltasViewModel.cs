using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class VertelTasViewModel:ItemViewModel
    {

        public VertelTasViewModel()
        {

        }

        public VertelTas MapToVertelTas(VertelTasViewModel vm, List<Thema> themas, List<Item> items )
        {
            return new VertelTas()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themas = themas,
                Items = items,
                ImageString = image
            };
        }


        public ICollection<Item> Items { get; set; }

        public List<Boek> Boeken { get; set; }

        public List<Spel> Spellen { get; set; }

        public List<CD> CDs { get; set; }

        public List<DVD> DVDs { get; set; }
    }

    public class VertelTasLijstViewModel
    {
        public IEnumerable<VertelTasViewModel> VertelTassen { get; set; }

        public VertelTasLijstViewModel(IEnumerable<VertelTas> verteltassen)
        {
            VertelTassen = verteltassen.Select(v => new VertelTasViewModel()
            {
                Exemplaar = v.Exemplaar,
                Titel = v.Titel,
                Omschrijving = v.Omschrijving,
                Leeftijd = v.Leeftijd,
                Themas = v.Themas,
                image = v.ImageString,
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
            VertelTassen = new List<VertelTasViewModel>();
        }
    }

    public class VertelTasCreateViewModel
    {
        public MultiSelectList AllThemas { get; set; }
        public List<int> SubmittedThemas { get; set; }
        public SelectList Themas { get; set; }
        public MultiSelectList AllBoeken { get; set; }
        public List<int> SubmittedBoeken { get; set; }
        public SelectList Boeken { get; set; }
        public MultiSelectList AllSpellen { get; set; }
        public List<int> SubmittedSpellen { get; set; }
        public SelectList Spellen { get; set; }
        public MultiSelectList AllCDs { get; set; }
        public List<int> SubmittedCDs { get; set; }
        public SelectList CDs { get; set; }
        public MultiSelectList AllDVDs { get; set; }
        public List<int> SubmittedDVDs { get; set; }
        public SelectList DVDs { get; set; }
        public VertelTasViewModel Verteltas { get; set; }

        public VertelTasCreateViewModel(IEnumerable<Thema> themas, VertelTas vertelTas, IEnumerable<Item> items )
        {
            Verteltas = new VertelTasViewModel()
            {
                Exemplaar = vertelTas.Exemplaar,
                Omschrijving = vertelTas.Omschrijving,
                Titel = vertelTas.Titel,
                Leeftijd = vertelTas.Leeftijd,
                Themas = themas.ToList(),
                image = vertelTas.ImageString,
                Boeken = items.OfType<Boek>().ToList(),
                Spellen = items.OfType<Spel>().ToList(),
                CDs = items.OfType<CD>().ToList(),
                DVDs = items.OfType<DVD>().ToList(),
                Beschikbaar = vertelTas.Beschikbaar,
                EindDatumUitlening = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? new DateTime() : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? false : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            //Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (vertelTas.Themas == null || vertelTas.Themas.Count == 0) ? null : vertelTas.Themas.Select(t => t.IdThema));
        }

        public VertelTasCreateViewModel()
        {
            Verteltas = new VertelTasViewModel();
            IEnumerable<Thema> themas = new List<Thema>();
            Themas = new SelectList(themas, "", "", "");
            AllThemas = new SelectList(themas, "", "", "");
            SubmittedThemas = new List<int>();
            IEnumerable<Boek> boeken = new List<Boek>();
            Boeken = new SelectList(boeken, "", "", "");
            AllBoeken = new SelectList(boeken,"","","");
            SubmittedBoeken = new List<int>();
            IEnumerable<Spel> spellen = new List<Spel>();
            Spellen = new SelectList(spellen, "", "", "");
            AllSpellen = new SelectList(spellen, "", "", "");
            SubmittedSpellen = new List<int>();
            IEnumerable<CD> Cds = new List<CD>();
            CDs = new SelectList(Cds, "", "", "");
            AllCDs = new SelectList(CDs, "", "", "");
            SubmittedCDs = new List<int>();
            IEnumerable<DVD> Dvds = new List<DVD>();
            DVDs = new SelectList(Dvds, "", "", "");
            AllDVDs = new SelectList(DVDs, "", "", "");
            SubmittedDVDs = new List<int>();
        }

    }



}