using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using DeKrekelGroup5.Models.DAL.Mappers;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class KrekelContext : DbContext
    {
        public KrekelContext(): base("name=dekrekelDB"){ }
        public KrekelContext(string connStringName) : base(connStringName) { }

        public DbSet<LetterTuin> LetterTuinen { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BoekMapper());
            modelBuilder.Configurations.Add(new SpelMapper());
            modelBuilder.Configurations.Add(new ThemaMapper());
            modelBuilder.Configurations.Add(new VerteltasMapper());
            modelBuilder.Configurations.Add(new UitleningMapper());
            modelBuilder.Configurations.Add(new UitlenerMapper());
            modelBuilder.Configurations.Add(new CDMapper());
            modelBuilder.Configurations.Add(new DVDMapper());
            modelBuilder.Configurations.Add(new LetterTuinMapper());
            modelBuilder.Configurations.Add(new GebruikerMapper());

            base.OnModelCreating(modelBuilder);
        }

        

        
    }
}