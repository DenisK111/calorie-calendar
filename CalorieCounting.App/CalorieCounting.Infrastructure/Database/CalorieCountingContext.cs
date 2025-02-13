using CalorieCounting.Application;
using CalorieCounting.Infrastructure.Database.Configurations;
using CalorieCounting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CalorieCounting.Infrastructure.Database.Constants.ShadowPropertyNames;

namespace CalorieCounting.Infrastructure.Database
{
    public class CalorieCountingContext : DbContext
    {
        public DbSet<Day> Days { get; set; }
        public DbSet<Entry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DayConfiguration());
            modelBuilder.ApplyConfiguration(new EntryConfiguration());

            //soft delete query filter
            modelBuilder.Model.GetEntityTypes().ForEach(entityType =>
            {
                var isDeletedProperty = entityType.FindProperty(IsDeleted);
                if (isDeletedProperty is not null && isDeletedProperty.ClrType == typeof(bool))
                {
                    var param = Expression.Parameter(entityType.ClrType, IsDeleted);
                    var prop = Expression.PropertyOrField(param, IsDeleted);
                    var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);
                    entityType.SetQueryFilter(entityNotDeleted);
                }                    
            });
        }

        public override int SaveChanges()
        {
            HandleModifications();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            HandleModifications();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void HandleModifications() {

            // Get the current time (UTC)
            var currentTime = DateTime.UtcNow;

            // Loop through the tracked entities to update shadow properties
            ChangeTracker.Entries().ForEach(entry =>
            {
                // Handle entities being added
                if (entry.State == EntityState.Added)
                {
                    // Set CreatedAt and ModifiedAt for newly added entities
                    if (entry.Property(CreatedAt).IsModified == false)
                        entry.Property(CreatedAt).CurrentValue = currentTime;

                    // Set ModifiedAt for newly added entities
                    entry.Property(ModifiedAt).CurrentValue = currentTime;

                    // Set IsDeleted to false for added entities
                    entry.Property(IsDeleted).CurrentValue = false;
                }

                // Handle entities being updated
                if (entry.State == EntityState.Modified)
                {
                    // Always update the ModifiedAt property when an entity is updated
                    entry.Property(ModifiedAt).CurrentValue = currentTime;
                }

                // Handle entities being deleted (soft delete)
                if (entry.State == EntityState.Deleted)
                {
                    // Set IsDeleted to true for soft deletes
                    entry.Property(IsDeleted).CurrentValue = true;

                    // Mark the entity as unchanged so it isn't actually deleted from the database
                    entry.State = EntityState.Modified;
                }
            });
            

        }
    }
}
