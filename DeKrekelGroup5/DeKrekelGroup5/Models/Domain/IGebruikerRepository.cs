using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IGebruikerRepository
    {
        Gebruiker GetGebruiker(int id);
        Gebruiker GetGebruikerByName(String naam);
        IQueryable<Gebruiker> GetGebruikers();

        void AddGebruiker(Gebruiker gebruiker);
        void RemoveGebruiker(Gebruiker gebruiker);
        void EditGebruiker(Gebruiker gebruiker);

        void SaveChanges();
        void DoNotDuplicateThema(Item item);
        void DoNotDuplicateLetterTuin(Gebruiker gebruiker);
        void AddLetterTuin(LetterTuin letterTuin);
        void UpdateUitlenersEindeSchooljaar();
    }
}