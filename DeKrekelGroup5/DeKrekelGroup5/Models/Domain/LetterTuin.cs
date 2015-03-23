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
        public IBoekenRepository BoekenRepository { get; set; }
        public ISpellenRepository SpellenRepository { get; set; }
        public IThemasRepository ThemasRepository { get; set; }

        public IEnumerable<Boek> GetBoeken(string search)
        { 
            return !String.IsNullOrEmpty(search) ? BoekenRepository.Find(search).ToList() : BoekenRepository.FindAll().Take(25).OrderBy(t => t.Titel).ToList();
        }

        public IEnumerable<Spel> GetSpellen(string search)
        {
            return !String.IsNullOrEmpty(search) ? SpellenRepository.Find(search).ToList() : SpellenRepository.FindAll().Take(25).OrderBy(t => t.Titel).ToList();
        }

        public IEnumerable<Thema> GetThemas()
        {
            return ThemasRepository.FindAll().OrderBy(n => n.Themaa);
        }

        public Thema GetThemaByName(string thema)
        {
            return ThemasRepository.FindBy(thema);
        }

        public Item GetItem(int id)
        {
            Item item = BoekenRepository.FindById(id);
            item.Themaa = ThemasRepository.FindById(item.Themaa.IdThema);
            return item;
        }
    }
}