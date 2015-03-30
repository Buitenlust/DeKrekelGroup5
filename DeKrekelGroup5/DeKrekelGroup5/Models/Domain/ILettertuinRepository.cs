using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface ILettertuinRepository
    {
        //boek
        IQueryable<Boek> FindAllBoeken();
        IQueryable<Boek> FindBoek(String search);
        Boek FindByIdBoek(int id);
        
        

        //CD
        IQueryable<CD> FindAllCds();
        IQueryable<CD> FindCd(String search);
        CD FindByIdCd(int id);
        
        
        

        // DVD
        IQueryable<DVD> findAllDvds();
        IQueryable<DVD> FindDvd(String search);
        DVD findByIdDvd(int id);
        
        
        

        // Spel
        IQueryable<Spel> FindAllSpellen();
        IQueryable<Spel> FindSpel(String search);
        Spel FindByIdSpel(int id);
        

        // Verteltas

        IQueryable<VertelTas> FindAllVerteltassen();
        VertelTas findByIdVerteltas(int id);
        


    }
}