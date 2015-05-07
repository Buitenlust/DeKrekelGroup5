using DeKrekelGroup5.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class UitleningViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een beginDatum in aub...")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDatum { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BinnenGebracht { get; set; }
        [Required(ErrorMessage = "Vul een EindDatum in aub...")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EindDatum { get; set; }
        [Required(ErrorMessage = "Item is vereist...")]
        public Item Itemm { get; set; }
        [Required(ErrorMessage = "Uitlener is vereist...")]
        public Uitlener Uitlenerr { get; set; }
        public int Verlenging { get; set; }

        public UitleningViewModel()
        {

        }

        public UitleningViewModel(Uitlening uitlening)
        {
            Id = uitlening.Id;
            StartDatum = uitlening.StartDatum;
            BinnenGebracht = uitlening.BinnenGebracht;
            EindDatum = uitlening.EindDatum;
            Itemm = uitlening.Itemm;
            Uitlenerr = uitlening.Uitlenerr;
            Verlenging = uitlening.Verlenging;
        }

        public Uitlening MapToUitlening(UitleningViewModel vm)
        {
            return new Uitlening(){
                                    Id = vm.Id,
                                    StartDatum = vm.StartDatum,
                                    BinnenGebracht = vm.BinnenGebracht,
                                    EindDatum = vm.EindDatum,
                                    Itemm = vm.Itemm,
                                    Uitlenerr = vm.Uitlenerr,
                                    Verlenging = vm.Verlenging
                                    };
        }
    }

    public class UitleningenLijstViewModel
    {
        public IEnumerable<UitleningViewModel> Uitleningen { get; set; }

        public UitleningenLijstViewModel(IEnumerable<Uitlening> uitleningen)
        {
            Uitleningen = uitleningen.Select(b => new UitleningViewModel()
                {
                    Id = b.Id,
                    StartDatum = b.StartDatum,
                    BinnenGebracht = b.BinnenGebracht,
                    EindDatum = b.EindDatum,
                    Itemm = b.Itemm,
                    Uitlenerr = b.Uitlenerr,
                    Verlenging = b.Verlenging
                });
        }

        public UitleningenLijstViewModel()
        {
            Uitleningen = new List<UitleningViewModel>();
        }
    }
}