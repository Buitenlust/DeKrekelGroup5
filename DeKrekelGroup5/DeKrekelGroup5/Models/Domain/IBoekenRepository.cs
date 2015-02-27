using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IBoekenRepository
    {
        IQueryable<Boek> FindAll();
        Boek FindById(int id);
        void Add(Boek boek);
        void SaveChanges(Boek boek);
        void Remove(Boek boek);
    }
}