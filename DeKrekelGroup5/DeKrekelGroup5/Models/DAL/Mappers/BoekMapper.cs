using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class BoekMapper : EntityTypeConfiguration<Boek>
    {
        public BoekMapper()
        {
            //table
            ToTable("Boek");

            //props
            Property(b => b.Omschrijving).HasMaxLength(5000);
            Property(b => b.Titel).HasMaxLength(250).IsRequired();
            Property(b => b.Uitgever).HasMaxLength(250);
            Property(b => b.Auteur).HasMaxLength(250);
            Property(b => b.ImageString).HasMaxLength(254);


            //primary key
            HasKey(boek => boek.Exemplaar);  
        }
    }
}