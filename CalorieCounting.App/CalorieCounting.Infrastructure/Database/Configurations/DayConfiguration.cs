using CalorieCounting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CalorieCounting.Infrastructure.Database.Constants.ShadowPropertyNames;

namespace CalorieCounting.Infrastructure.Database.Configurations
{
    internal class DayConfiguration : BaseEntityTypeConfiguration<Day>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Day> builder)
        {
            builder.ToTable("days");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
            .ValueGeneratedOnAdd() // Auto-increment for the Id
            .HasColumnType("serial"); // Explicitly set to serial type in PostgreSQL            

            /// Adding a partial unique index for the combination of Date and IsDeleted = false
            var dateName = builder.Property(b => b.Date).Metadata.Name;
            builder.HasIndex([dateName,IsDeleted])
                .HasFilter($"{IsDeleted} = false")
                .IsUnique();

            builder.Property(d => d.Date)
                .IsRequired()
                .HasColumnType("DATE");                

            builder.Property(d => d.Weight)
                .IsRequired();            
        }
    }
}
