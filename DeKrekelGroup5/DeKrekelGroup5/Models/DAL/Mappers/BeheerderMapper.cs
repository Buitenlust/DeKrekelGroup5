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
            //Table
            ToTable("Beheerder");

            //Key
            HasKey(be => be.Id);

            //Props
            Property(be => be.Password).IsRequired();

        }

    }
}