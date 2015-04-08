using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public class Instellingen
    {
        [Key]
        public int Id { get; set; }
        public decimal BedragBoetePerDag { get; set; }
        public int UitleenDagen { get; set; }
        public int MaxVerlengingen { get; set; }

        public Instellingen()
        {
            
        }

        public Instellingen(int id, decimal bedragBoetePerDag, int uitleenDagen, int maxVerlengingen)
        {
            Id = id;
            BedragBoetePerDag = bedragBoetePerDag;
            UitleenDagen = uitleenDagen;
            MaxVerlengingen = maxVerlengingen;
        }

    }
}