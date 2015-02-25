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
            Property(v => v.Bevat).HasMaxLength(45);
            //Property(v => v.Boeken).  --------- Weet voorlopig niet of deze collections wel gemapt kunnen worden
            //Property(v => v.Spellen).
        }
    }
}