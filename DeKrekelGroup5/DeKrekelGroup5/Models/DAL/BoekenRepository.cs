using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class BoekenRepository : IBoekenRepository
    {
        private KrekelContext context;

        public BoekenRepository(KrekelContext context) 
        {
            this.context = context;
        }

        public IQueryable<Boek> FindAll()
        {
            return context.Boeken;
        }

        public Boek FindById(int id)
        {
            return context.Boeken.Find(id);
        }

        public void Add(Boek boek)
        {
            context.Boeken.Add(boek);
        }

        public void SaveChanges(Boek boek)
        {
            context.Entry(boek.Themaa).State = EntityState.Modified;    //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(Boek boek)
        {
            context.Boeken.Remove(boek);
        }
    }
}