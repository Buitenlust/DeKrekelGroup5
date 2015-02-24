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
        private DbSet<Boek> boeken;
        public BoekenRepository(KrekelContext context) 
        {
            this.context = context;
            boeken = context.Boeken;
        }

        public IQueryable<Boek> FindAll()
        {
            return boeken;
        }

        public Boek FindById(int id)
        {
            return boeken.Find(id);
        }

        public void Add(Boek boek)
        {
            boeken.Add(boek);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}