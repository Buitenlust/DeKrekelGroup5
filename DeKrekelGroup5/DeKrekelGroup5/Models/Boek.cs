using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models
{
    public class Boek : Item1
    {
        public long Isbn { get; set; }
        
        public Boek()
        {
        }
    }
}