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

        public LetterTuin(IBoekenRepository boekenRepository, ISpellenRepository spellenRepository, IThemasRepository themasRepository)
        {
            BoekenRepository = boekenRepository;
            SpellenRepository = spellenRepository;
            ThemasRepository = themasRepository;
        }

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

        public Boek GetBoek(int id)
        {
            Boek boek = BoekenRepository.FindById(id);
            return boek;
        }

        public Spel GetSpel(int id)
        {
            Spel spel = SpellenRepository.FindById(id);
            return spel;
        }
    }
}