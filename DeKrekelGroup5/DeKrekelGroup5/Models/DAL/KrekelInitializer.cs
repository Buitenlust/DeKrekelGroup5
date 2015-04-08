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