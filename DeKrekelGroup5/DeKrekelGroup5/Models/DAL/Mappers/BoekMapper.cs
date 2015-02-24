using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
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
            Property(b => b.Omschrijving).HasMaxLength(45);
            Property(b => b.Titel).HasMaxLength(45).IsRequired();
            Property(b => b.Uitgever).HasMaxLength(45);
            Property(b => b.Auteur).HasMaxLength(45);

            //primary key
            HasKey(boek => boek.Exemplaar);
        }
    }
}