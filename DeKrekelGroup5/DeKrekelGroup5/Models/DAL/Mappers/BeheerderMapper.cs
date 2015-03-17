using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class BeheerderMapper : EntityTypeConfiguration<Beheerder>
    {

        public BeheerderMapper()
        {
            //table
            ToTable("Beheerder");

            //props
            Property(b => b.Naam).HasMaxLength(55).IsRequired();
            
            //primary key
            HasKey(b => b.BeheerderId );
        }
    }
}