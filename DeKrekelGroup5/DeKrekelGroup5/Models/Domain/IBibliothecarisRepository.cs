using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IBibliothecarisRepository
    {
       

        //uitleningen
        IQueryable<Uitlening> FindAllUitleningen();
        Uitlening FindByIdUitlening(int id);
        void Add(Uitlening uitlening);
        void SaveChanges(Uitlening uitlening);

        //uitleners
        IQueryable<Uitlener> FindAllUitleners();
        Uitlener FindByIdUitlener(int id);
        
    }
}