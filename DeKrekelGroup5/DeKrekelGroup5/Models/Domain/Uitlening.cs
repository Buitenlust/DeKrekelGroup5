using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Uitlening
    {
        public int Id { get; set; }
        
        public DateTime StartDatum { get; set; }
        public DateTime BinnenGebracht { get; set; }
        public DateTime EindDatum { get; set; }
        public Item Itemm { get; set; }
        public Uitlener Uitlenerr { get; set; }
        public int Verlenging { get; set; }

        public void update(Uitlening uitlening)
        {
            StartDatum = uitlening.StartDatum;
            BinnenGebracht = uitlening.BinnenGebracht;
            EindDatum = uitlening.EindDatum;
            Itemm = uitlening.Itemm;
            Uitlenerr = uitlening.Uitlenerr;
            Verlenging = uitlening.Verlenging;
        }
    }
}
