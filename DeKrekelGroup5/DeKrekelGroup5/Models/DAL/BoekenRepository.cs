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
        //private DbSet<Boek> boeken;
        public BoekenRepository(KrekelContext context) 
        {
            this.context = context;
            //boeken = context.Boeken;
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

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Remove(Boek boek)
        {
            context.Boeken.Remove(boek);
        }
    }
}