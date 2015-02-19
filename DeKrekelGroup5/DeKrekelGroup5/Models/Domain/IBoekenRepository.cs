using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public class IBoekenRepository
    {
        IQueryable<Boek> FindAll();
        Boek FindById(int id);
        void SaveChanges();
    }
}