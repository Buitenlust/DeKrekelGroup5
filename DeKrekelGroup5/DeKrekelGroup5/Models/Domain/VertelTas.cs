using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Linq;

namespace DeKrekelGroup5.Models.Domain
{
    public class VertelTas : Item
    {
        public string Bevat { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Boek> Boeken { get; set; }
        public virtual ICollection<Spel> Spellen { get; set; }
        public virtual ICollection<CD> CDs { get; set; }
        public virtual ICollection<DVD> DVDs { get; set; }
    }
}
