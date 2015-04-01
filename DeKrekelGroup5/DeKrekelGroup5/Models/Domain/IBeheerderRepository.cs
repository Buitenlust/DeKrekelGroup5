using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IBeheerderRepository
    {
        void Remove(Boek boek);
        void Add(Boek boek);
        void SaveChanges(Boek boek);

        void Add(CD cd);
        void Remove(CD cd);
        void SaveChanges(CD cd);

        void Add(DVD dvd);
        void Remove(DVD dvd);
        void SaveChanges(DVD dvd);

        void Add(Spel spel);
        void Remove(Spel spel);
        void SaveChanges(Spel spel);

        void Add(VertelTas vertelTas);
        void SaveChanges(VertelTas verteltas);

        void Add(Uitlener uitlener);
        void SaveChanges(Uitlener uitlener);


        void DoNotDuplicateThema(Item item);
        void SaveChanges();
         
    }
}