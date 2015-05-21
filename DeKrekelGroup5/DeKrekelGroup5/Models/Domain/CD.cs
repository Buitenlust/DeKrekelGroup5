using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
   public class CD : Item
    {
        public string Artiest { get; set; }
        public string Uitgever { get; set; }
    }
}