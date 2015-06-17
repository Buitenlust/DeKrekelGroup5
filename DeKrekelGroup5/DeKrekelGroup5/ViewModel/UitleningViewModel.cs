using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class UitleningViewModel
    {
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Type { get; set; }
        public string StartDatum { get; set; }
        public string EindDatum { get; set; }
        public int AantalVerlengingen { get; set; }
        public string DatumBinnengebracht { get; set; }



    }

    public class UitleningenLijstViewModel
    {
        public IEnumerable<UitleningViewModel> Uitleningen { get; set; } 

        public UitleningenLijstViewModel()
        {
            
        }
        public UitleningenLijstViewModel(IEnumerable<Uitlening> uitleningen, string type=null)
        {
            Uitleningen = uitleningen.Select(u => new UitleningViewModel()
            {
                Naam = u.Uitlenerr.Naam,
                Voornaam = u.Uitlenerr.VoorNaam,
                Type = type,
                StartDatum = u.StartDatum.ToShortDateString(),
                EindDatum = u.EindDatum.ToShortDateString(),
                AantalVerlengingen = u.Verlenging,
                DatumBinnengebracht = u.BinnenGebracht.ToShortTimeString()
            });
        }
    }
}