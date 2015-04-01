using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class GebruikerMapper : EntityTypeConfiguration<Gebruiker>
    {
        public GebruikerMapper()
        {
            //table
            ToTable("Gebruiker");

            //props
            Property(b => b.PaswoordHashed).HasMaxLength(64).IsRequired();
            Property(b => b.GebruikersNaam).HasMaxLength(55).IsRequired();
            Property(b => b.AdminRechten);

            //primary key
            HasKey(b => b.Id);
        }
    }
}