using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Spel : Item
    {
        public string Uitgever { get; set; }

        public void Update(Spel spel)
        {
            base.Update(spel);
            Uitgever = spel.Uitgever;
        }
    }
}
