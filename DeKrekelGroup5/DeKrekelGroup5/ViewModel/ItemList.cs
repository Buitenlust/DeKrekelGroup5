using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemList
    {
        public IEnumerable<item> Items { get; set; }
        public IEnumerable<type> Types { get; set; }
        public string zoekTitel { get; set; }
    }
}