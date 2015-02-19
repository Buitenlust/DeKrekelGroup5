using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Thema
    {
        public int IdThema { get; set; }
        public String Thema { get; set; }

        public virtual ICollection<Item> Items { get; set; } 
    }
}
