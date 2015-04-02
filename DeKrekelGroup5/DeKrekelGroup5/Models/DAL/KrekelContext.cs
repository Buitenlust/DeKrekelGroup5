﻿using System;
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
        //public DbSet<Item> Items { get; set; }
        //public DbSet<Boek> Boeken { get; set; }
        //public DbSet<Spel> Spellen { get; set; }
        //public DbSet<Thema> Themas { get; set; }
        //public DbSet<VertelTas> VertelTassen { get; set; }
        //public DbSet<Uitlening> Uitleningen { get; set; }
        //public DbSet<Uitlener> Uitleners { get; set; }
        //public DbSet<CD> Cds { get; set; }
        //public DbSet<DVD> Dvds { get; set; } 


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