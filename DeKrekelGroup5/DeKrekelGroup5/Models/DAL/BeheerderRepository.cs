using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class BeheerderRepository:IBeheerderRepository
    {
        private KrekelContext context;

        public BeheerderRepository(KrekelContext context)
        {
            this.context = context;
        }


        public Beheerder FindBeheerder(string naam)
        {
            return context.Beheerders.SingleOrDefault(c => c.Naam.Equals(naam));
        }

        public void SaveChanges(Boek boek)
        {
            context.Entry(boek.Themaa).State = EntityState.Modified;    //Zorgt dat het thema niet aangemaakt wordt. 
            context.SaveChanges();
        }
    }
}