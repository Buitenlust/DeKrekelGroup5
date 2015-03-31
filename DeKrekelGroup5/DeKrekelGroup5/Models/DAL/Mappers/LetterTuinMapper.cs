using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class LetterTuinMapper : EntityTypeConfiguration<LetterTuin>
    {
        public LetterTuinMapper()
        {
            //table
            ToTable("LetterTuin");
            HasKey(b => b.Id);

            //HasMany(c => c.Items).WithRequired().Map(m => m.MapKey("LetterTuinId")).WillCascadeOnDelete(false);
            
        }
    }
}