using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Item
    {
        public int Exemplaar { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
    }
}
