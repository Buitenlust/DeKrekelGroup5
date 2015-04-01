using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class LettertuinRepository : ILettertuinRepository
    {
        private KrekelContext context;

        public LettertuinRepository(KrekelContext context)
        {
            this.context = context; 
        }

        public LetterTuin GetLetterTuin(int id)
        {
            return context.LetterTuinen.Find(id);
        }

        //public IQueryable<Item> FindAllItems()
        //{
        //    return context.Items;
        //}

        //public Item FindItemById(int id)
        //{
        //    return context.Items.Find(id);
        //}

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        ////Boek

        //public IQueryable<Boek> FindAllBoeken()
        //{
        //    return context.Boeken;
        //}

        //public IQueryable<Boek> FindBoek(string search)
        //{
        //    return context.Boeken.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
        //                                     p.Uitgever.ToLower().Contains(search.ToLower()) ||
        //                                     p.Auteur.ToLower().Contains(search.ToLower()) ||
        //                                     p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
        //                                     p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        //}

        //public Boek FindByIdBoek(int id)
        //{
        //    return context.Boeken.Find(id);
        //}

        

        ////CD
        //public IQueryable<CD> FindAllCds()
        //{
        //    return context.Cds;
        //}

        //public IQueryable<CD> FindCd(string search)
        //{
        //    return context.Cds.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
        //                                  p.Uitgever.ToLower().Contains(search.ToLower()) ||
        //                                  p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
        //                                  p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        //}

        //public CD FindByIdCd(int id)
        //{
        //    return context.Cds.Find(id);
        //}


        ////DVD
        //public IQueryable<DVD> findAllDvds()
        //{
        //    return context.Dvds;
        //}


        

        //public IQueryable<DVD> FindDvd(string search)
        //{
        //    return context.Dvds.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
        //                                     p.Uitgever.ToLower().Contains(search.ToLower()) ||
        //                                     p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
        //                                     p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        //}

        //public DVD findByIdDvd(int id)
        //{
        //    return context.Dvds.Find(id);

        //}
       

        ////Spel
        //public IQueryable<Spel> FindAllSpellen()
        //{
        //    return context.Spellen;
        //}

        //public IQueryable<Spel> FindSpel(string search)
        //{
        //    return context.Spellen.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
        //                                     p.Uitgever.ToLower().Contains(search.ToLower()) ||
        //                                     p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
        //                                     p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        //}

        //public Spel FindByIdSpel(int id)
        //{
        //    return context.Spellen.Find(id);
        //}

        

        ////Verteltas


        //public IQueryable<VertelTas> FindAllVerteltassen()
        //{
        //    return context.VertelTassen;
        //}

        //public VertelTas findByIdVerteltas(int id)
        //{
        //    return context.VertelTassen.Find(id);
        //}

        //public IQueryable<Thema> FindAllthemas()
        //{
        //    return context.Themas;
        //}

        //public Thema FindThema(int id)
        //{
        //    return context.Themas.Find(id);
        //}

        public void DoNotDuplicateThema(Item item)
        {
            context.Entry(item.Themaa).State = EntityState.Modified;
        }

        public void AddLetterTuin(LetterTuin letterTuin)
        {
            context.LetterTuinen.Add(letterTuin);
        }
    }
}