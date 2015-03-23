using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Beheerder : LetterTuin
    {
        public Beheerder(IBoekenRepository boekenRepository, ISpellenRepository spellenRepository, IThemasRepository themasRepository) : base(boekenRepository, spellenRepository, themasRepository)
        {
        }

        public void AddBoek(BoekViewModel boek)
        {
            Boek newBoek = new Boek
            {
                Exemplaar = boek.Exemplaar,
                Auteur = boek.Auteur,
                Leeftijd = boek.Leeftijd,
                Omschrijving = boek.Omschrijving,
                Titel = boek.Titel,
                Uitgever = boek.Uitgever,
                Themaa = (String.IsNullOrEmpty(boek.Thema) ? null : GetThemaByName(boek.Thema))
             };
            BoekenRepository.Add(newBoek);
            BoekenRepository.DoNotDuplicateThema(newBoek);
            BoekenRepository.SaveChanges();
        }

        public void AddSpel(SpelViewModel spel)
        {
            Spel newSpel = new Spel
            {
                Exemplaar = spel.Exemplaar,
                Uitgever = spel.Uitgever,
                Leeftijd = spel.Leeftijd,
                Omschrijving = spel.Omschrijving,
                Titel = spel.Titel,
                Themaa = (String.IsNullOrEmpty(spel.Thema) ? null : GetThemaByName(spel.Thema))
            };

            SpellenRepository.Add(newSpel);
            SpellenRepository.DoNotDuplicateThema(newSpel);
            SpellenRepository.SaveChanges();
        }

        public void EditBoek(BoekViewModel boek)
        {
            Boek newBoek = null;
            if (boek.Exemplaar != 0)
                newBoek = GetBoek(boek.Exemplaar);
            if (newBoek != null)
            {
                newBoek.Auteur = boek.Auteur;
                newBoek.Leeftijd = boek.Leeftijd;
                newBoek.Omschrijving = boek.Omschrijving;
                newBoek.Titel = boek.Titel;
                newBoek.Uitgever = boek.Uitgever;
                newBoek.Themaa = (String.IsNullOrEmpty(boek.Thema) ? null : GetThemaByName(boek.Thema));
                //newBoek.Update(newBoek);
                BoekenRepository.DoNotDuplicateThema(newBoek);
                BoekenRepository.SaveChanges();
            }
        }

        public void EditSpel(SpelViewModel spel)
        {
            Spel newSpel = null;
            if (spel.Exemplaar != 0)
                newSpel = GetSpel(spel.Exemplaar);
            if (newSpel != null)
            { 
                newSpel.Leeftijd = spel.Leeftijd;
                newSpel.Omschrijving = spel.Omschrijving;
                newSpel.Titel = spel.Titel;
                newSpel.Uitgever = spel.Uitgever;
                newSpel.Themaa = (String.IsNullOrEmpty(spel.Thema) ? null : GetThemaByName(spel.Thema));
                SpellenRepository.DoNotDuplicateThema(newSpel);
                SpellenRepository.SaveChanges();
            }
        } 

        public void VerwijderBoek(int id)
        {
            BoekenRepository.Remove(GetBoek(id));
            BoekenRepository.SaveChanges();
        }

        public void VerwijderSpel(int id)
        {
            SpellenRepository.Remove(GetSpel(id));
            SpellenRepository.SaveChanges();
        }

        
    }
}