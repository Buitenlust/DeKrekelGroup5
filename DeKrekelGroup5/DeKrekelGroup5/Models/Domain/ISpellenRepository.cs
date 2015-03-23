using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface ISpellenRepository
    {
        IQueryable<Spel> FindAll();
        IQueryable<Spel> Find(String search);
        Spel FindById(int id);
        void DoNotDuplicateThema(Spel spel);
        void Add(Spel spel);
        void SaveChanges();
        void Remove(Spel spel);
    }
}