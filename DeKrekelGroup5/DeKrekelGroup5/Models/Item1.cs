using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models
{
    public class Item1
    {

        public int ExemplaarNr { get; set; }
        public String Titel { get; set; }
        public String Uitgever { get; set; }
        public String Omschrijving { get; set; }
        public int Jaar { get; set; }
        public String Genre { get; set; }
        
        public Item1()
        {
            
        }
    }
}