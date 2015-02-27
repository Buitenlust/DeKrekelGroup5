using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class ThemaMapper : EntityTypeConfiguration<Thema>
    {
        public ThemaMapper()
        {
            //table
            ToTable("Thema");

            //PK
            HasKey(t => t.IdThema);



            //Props
            Property(k => k.IdThema).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_idthema", 1) { IsUnique = true }));
            //index geplaatst op thema om uniek te maken. Bijgevolg ook terug index moeten instellen op ID.
            Property(t => t.Themaa).HasMaxLength(125).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_thema",2) { IsUnique = true }));
        }
    }
}