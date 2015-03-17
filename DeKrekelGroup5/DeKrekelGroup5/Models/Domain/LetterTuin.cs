using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public class LetterTuin
    {
        public List<Item> Items  { get; set; } 

        public LetterTuin()
        {
            Items = new List<Item>();
        }
    }
}