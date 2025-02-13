using CalorieCounting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounting.Infrastructure.Database.Configurations
{
    internal class EntryConfiguration : BaseEntityTypeConfiguration<Entry>
    {
        public override void Configure(EntityTypeBuilder<Entry> builder)
        {
            builder.ToTable("entries");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd() // Auto-increment for the Id
                .HasColumnType("serial");

            builder.HasOne(e => e.Day)
                   .WithMany(d => d.Entries)
                   .HasForeignKey(e => e.DayId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Calories)
                .IsRequired();

            // Configure the Macros as a complex property
            builder.OwnsOne(e => e.Macros, m =>
            {
                m.Property(p => p.Protein)                    
                    .IsRequired();      

                m.Property(p => p.Carbs)                    
                    .IsRequired();      

                m.Property(p => p.Fat)                  
                    .IsRequired();     
            });            

            builder.Property(e => e.Time)
                .IsRequired()
                .HasColumnType("TIME");           
        }
    }
}
