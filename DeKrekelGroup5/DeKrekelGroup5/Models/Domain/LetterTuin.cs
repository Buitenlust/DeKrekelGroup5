using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DeKrekelGroup5.ViewModel;
using Ninject.Activation;

namespace DeKrekelGroup5.Models.Domain
{
    public class LetterTuin
    { 
        public IItemRepository ii { get; private set; }
        public IThemasRepository tr { get; private set; }


        public LetterTuin(IItemRepository iitemRepository,IThemasRepository themasRepository)
        {
            ii = iitemRepository; 
            tr = themasRepository;
        }


        public IEnumerable<Boek> GetBoeken(string search)
        {
            IEnumerable<Boek> boeken;
            if (!String.IsNullOrEmpty(search))
            {
                boeken = ii.FindBoek(search).ToList();
            }
            else
            {
                boeken = ii.FindAll().OfType<Boek>().ToList().Take(25).OrderBy(t => t.Titel);
            }
            return boeken;
        }

        public IEnumerable<Spel> GetSpellen(string search)
        {
            IEnumerable<Spel> spellen;
            if (!String.IsNullOrEmpty(search))
            {
                spellen = ii.FindSpel(search).ToList();
            }
            else
            {
                spellen = ii.FindAll().OfType<Spel>().ToList().Take(25).OrderBy(t => t.Titel);
            }
            return spellen;
        }

        public IEnumerable<Thema> GetThemas()
        {
            return tr.FindAll().OrderBy(n => n.Themaa);
        }

        public Thema GetThemabyId(int id)
        {
            return tr.FindById(id);
        }
    }
}