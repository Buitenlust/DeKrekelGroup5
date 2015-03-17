using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public class Beheerder : LetterTuin
    {
        public int BeheerderId { get; set; }
        public string Naam { get; set; }

        public Beheerder()
        {
            
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

 
    }
}