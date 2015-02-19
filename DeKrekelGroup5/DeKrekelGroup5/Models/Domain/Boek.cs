using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Boek : Item
    {
        public string Auteur { get; set; }
        public string Uitgever { get; set; }
        public int Jaar { get; set; }
        public long isbn { get; set; }
    }
}
