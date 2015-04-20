using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class KrekelInitializer : DropCreateDatabaseIfModelChanges<KrekelContext>
    {
        protected override void Seed(KrekelContext context)
        {
            try
            {
                Instellingen instellingen = new Instellingen()
                {
                    BedragBoetePerDag = 10,
                    MaxVerlengingen = 2,
                    UitleenDagen = 14
                };
                LetterTuin letterTuin = new LetterTuin() {Instellingen = instellingen};
                Gebruiker anonymous = new Gebruiker(1, "Anonymous", "Anonymous", false, false, letterTuin);
                Gebruiker bibliothecaris = new Gebruiker(2, "Bibliothecaris", "Bibliothecaris", false, true, letterTuin);
                Gebruiker beheerder = new Gebruiker(3, "Beheerder", "Beheerder", true, true, letterTuin);


                //Thema thema1 = new Thema() { IdThema = 1, Themaa = "Kinderboek" };
                //Thema thema2 = new Thema() { IdThema = 1, Themaa = "Leren" };
                //beheerder.AddThema(thema1);
                //beheerder.AddThema(thema2);

                //Boek boek1 = new Boek()
                //{
                //    Exemplaar = 1,
                //    Auteur = "Alex de Wolf & Koos Meinderts",
                //    Titel = "Dit ei is van mij",
                //    Uitgever = "",
                //    Omschrijving = "Dieren vinden een ei ",
                //    Beschikbaar = true,
                //};
                //Boek boek2 = new Boek()
                //{
                //    Exemplaar = 2,
                //    Auteur = "Maranke Inck en Martijn van der linden",
                //    Titel = "slaap schildpad slaap",
                //    Uitgever = "Van In",
                //    Omschrijving = "Slapen",
                //    Leeftijd = 3,
                //    Beschikbaar = true
                //};
                //Boek boek3 = new Boek()
                //{
                //    Exemplaar = 3,
                //    Auteur = "hallo, tot ziens en dankjewel!",
                //    Titel = "Christine Sterkens",
                //    Uitgever = "Herkes - nik nak boekje",
                //    Omschrijving = "boodschappen doen ",
                //    Beschikbaar = true,
                //    Leeftijd = 4
                //};

                //beheerder.AddItem(boek1);
                //beheerder.AddItem(boek2);
                //beheerder.AddItem(boek3);


                context.Gebruikers.Add(anonymous);
                context.Gebruikers.Add(bibliothecaris);
                context.Gebruikers.Add(beheerder);
               

                context.SaveChanges();

               


            }
            catch (DbEntityValidationException e)
            {
                string s = "Fout creatie database ";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.GetValidationResult());
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(s);
            }

        }
    }
}