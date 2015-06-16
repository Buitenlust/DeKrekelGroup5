using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class VertelTasViewModel : ItemViewModel
    {

        public VertelTasViewModel()
        {

        }

        public VertelTas MapToVertelTas(VertelTasViewModel vm, List<Thema> themas, List<Boek> boeken, List<DVD> dvds, List<CD> cds, List<Spel> spellen)
        {
            return new VertelTas()
            {
                Exemplaar = vm.Exemplaar,
                Beschikbaar = vm.Beschikbaar,
                Leeftijd = vm.Leeftijd,
                Omschrijving = vm.Omschrijving,
                Titel = vm.Titel,
                Themas = themas,
                Boeken = boeken,
                Spellen = spellen,
                DVDs = dvds,
                CDs = cds,
                ImageString = image
            };
        }

 

        public ICollection<Boek> Boeken { get; set; }

        public ICollection<Spel> Spellen { get; set; }

        public ICollection<CD> CDs { get; set; }

        public ICollection<DVD> DVDs { get; set; }
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
                Boeken = v.Boeken,
                Spellen = v.Spellen,
                DVDs = v.DVDs,
                CDs = v.CDs, 
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
        public SelectList Items { get; set; }
        public MultiSelectList AllItems { get; set; }
        public List<int> SubmittedItems { get; set; }

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

        public VertelTasCreateViewModel(IEnumerable<Thema> themas, VertelTas vertelTas, IEnumerable<Boek> boeken, IEnumerable<DVD> dvds,IEnumerable<CD> cds,IEnumerable<Spel> spellen)
        {
            Verteltas = new VertelTasViewModel()
            {
                Exemplaar = vertelTas.Exemplaar,
                Omschrijving = vertelTas.Omschrijving,
                Titel = vertelTas.Titel,
                Leeftijd = vertelTas.Leeftijd,
                Themas = themas.ToList(),
                image = vertelTas.ImageString,
                Boeken = boeken.ToList(),
                Spellen = spellen.ToList(),
                CDs = cds.ToList(),
                DVDs = dvds.ToList(),
                Beschikbaar = vertelTas.Beschikbaar,
                EindDatumUitlening = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? new DateTime() : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).EindDatum,
                Uitgeleend = (vertelTas.Uitleningen == null || vertelTas.Uitleningen.Count == 0) ? false : vertelTas.Uitleningen.SingleOrDefault(d => d.Id == vertelTas.Uitleningen.Max(c => c.Id)).BinnenGebracht.Year == 1
            };

            //Themas = new SelectList(themas, "Themaa", "Themaa", Boek.Thema ?? "");
            AllThemas = new MultiSelectList(themas.ToList(), "IdThema", "Themaa", (vertelTas.Themas == null || vertelTas.Themas.Count == 0) ? null : vertelTas.Themas.Select(t => t.IdThema));
            AllBoeken = new MultiSelectList(boeken, "Exemplaar", "Titel", (vertelTas.Boeken == null || vertelTas.Boeken.Count == 0) ? null : vertelTas.Boeken.Select(b => b.Exemplaar));
            AllSpellen = new MultiSelectList(spellen, "Exemplaar", "Titel", (vertelTas.Spellen == null || !vertelTas.Spellen.Any()) ? null : vertelTas.Spellen.Select(s => s.Exemplaar));
            AllCDs = new MultiSelectList(cds, "Exemplaar", "Titel", (vertelTas.CDs == null || !vertelTas.CDs.Any()) ? null : vertelTas.CDs.Select(c => c.Exemplaar));
            AllDVDs = new MultiSelectList(dvds.ToList(), "Exemplaar", "Titel", (vertelTas.DVDs == null || !vertelTas.DVDs.Any()) ? null : vertelTas.DVDs.Select(d => d.Exemplaar));
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
            AllBoeken = new SelectList(boeken, "", "", "");
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