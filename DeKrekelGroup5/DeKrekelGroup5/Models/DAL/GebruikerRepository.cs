using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private KrekelContext context;

        public GebruikerRepository(KrekelContext context)
        {
            this.context = context; 
        }

        public Gebruiker GetGebruiker(int id)
        {
            return context.Gebruikers.Include(l => l.LetterTuin.Instellingen).FirstOrDefault(i => i.Id==id);
        }

        public Gebruiker GetGebruikerByName(string naam)
        {
            return context.Gebruikers.Include(l => l.LetterTuin.Instellingen).FirstOrDefault(i => i.GebruikersNaam == naam);
        }

        public IQueryable<Gebruiker> GetGebruikers()
        {
            return context.Gebruikers.Include(l=>l.LetterTuin);
        }

        public void AddGebruiker(Gebruiker gebruiker)
        {
            context.Gebruikers.Add(gebruiker);
        }

        public void RemoveGebruiker(Gebruiker gebruiker)
        {
            context.Gebruikers.Remove(gebruiker);
        }

        public void EditGebruiker(Gebruiker gebruiker)
        {
            context.Gebruikers.AddOrUpdate(gebruiker);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void DoNotDuplicateThema(Item item)
        {
            context.Entry(item.Themaa).State = EntityState.Modified;
        }

        public void DoNotDuplicateLetterTuin(Gebruiker gebruiker)
        {
            context.Entry(gebruiker.LetterTuin).State = EntityState.Modified;
        }


        public void AddLetterTuin(LetterTuin letterTuin)
        {
            context.LetterTuinen.Add(letterTuin);
        }

        public void UpdateUitlenersEindeSchooljaar()
        {
            context.Database.ExecuteSqlCommand("UPDATE Uitlener SET Klas = {0}", "Geen");
        }
    }
}