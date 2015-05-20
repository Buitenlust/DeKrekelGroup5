using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class VerteltasMapper : EntityTypeConfiguration<VertelTas>
    {
        public VerteltasMapper()
        {
            //table
            ToTable("Verteltas");

            //PK
            HasKey(v => v.Exemplaar);

            //Props
            Property(b => b.Omschrijving).HasMaxLength(5000);
            Property(b => b.Titel).HasMaxLength(250).IsRequired();
            Property(v => v.Bevat).HasMaxLength(250);
            Property(b => b.ImageString).HasMaxLength(254);
        }
    }
}