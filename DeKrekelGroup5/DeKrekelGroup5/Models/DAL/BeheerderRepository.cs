using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class BeheerderRepository : IBeheerderRepository
    {

        private KrekelContext context;
        private DbSet<VertelTas> verteltassen; 


        public BeheerderRepository(KrekelContext context)
        {
            this.context = context;
            verteltassen = context.VertelTassen;
        }

        //boek
        public void Add(Boek boek)
        {
            context.Boeken.Add(boek);
        }

        public void SaveChanges(Boek boek)
        {
            context.Entry(boek.Themaa).State = EntityState.Modified;//Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(Boek boek)
        {
            context.Boeken.Remove(boek);
        }

        //CD
        public void Add(CD cd)
        {
            context.Cds.Add(cd);
        }

        public void SaveChanges(CD cd)
        {
            context.Entry(cd.Themaa).State = EntityState.Modified; //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(CD cd)
        {
            context.Cds.Remove(cd);
        }

        //DVD
        public void Add(DVD dvd)
        {
            context.Dvds.Add(dvd);
        }

        public void SaveChanges(DVD dvd)
        {
            context.Entry(dvd.Themaa).State = EntityState.Modified;    //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(DVD dvd)
        {
            context.Dvds.Remove(dvd);
        }


        //Spel
        public void Add(Spel spel)
        {

            context.Spellen.Add(spel);
        }

        public void SaveChanges(Spel spel)
        {
            context.Entry(spel.Themaa).State = EntityState.Modified; //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(Spel spel)
        {
            context.Spellen.Remove(spel);
        }

        //Verteltas
        public void Add(VertelTas vertelTas)
        {
            verteltassen.Add(vertelTas);
        }

        public void SaveChanges(VertelTas verteltas)
        {
            context.Entry(verteltas.Themaa).State = EntityState.Modified; //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Add(Uitlener uitlener)
        {
            context.Uitleners.Add(uitlener);
        }

        public void SaveChanges(Uitlener uitlener)
        {
            context.SaveChanges();
        }
    }
}