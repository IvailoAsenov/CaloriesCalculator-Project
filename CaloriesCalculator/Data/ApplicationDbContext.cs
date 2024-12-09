using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace CaloriesCalculator.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProgressEntry> ProgressEntries { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Добавяне на нови таблици за храните и избраните храни
        public DbSet<Food> Foods { get; set; }
        public DbSet<SelectedFood> SelectedFoods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Food>()
                .HasKey(f => f.Name); // For Food, we use Name as the primary key (this can be changed)

            builder.Entity<SelectedFood>()
                .HasKey(sf => sf.Id);

            builder.Entity<SelectedFood>()
                .HasOne<Food>()
                .WithMany() // One selected food can relate to many foods (inverse relationship is not required here)
                .HasForeignKey(sf => sf.Name) // Set foreign key to Name in SelectedFood
                .OnDelete(DeleteBehavior.Restrict); // Only restrict cascading delete
        }


    }
}
