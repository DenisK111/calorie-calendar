using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;

namespace CalorieCounting.Infrastructure.Database.Configurations
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Configure IsDeleted as a shadow property
            builder.Property(d => "is_deleted")
                 .HasDefaultValue(false);

            builder.Property(d => "created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .ValueGeneratedOnAdd();

            builder.Property(d => "modified_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
