using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemList
    {
        public IEnumerable<Boek> boeken { get; set; }
        public IEnumerable<Spel> spellen { get; set; }
        public string zoekString { get; set; }
    }
}