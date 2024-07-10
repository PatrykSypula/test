using ASPNET_EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_EF.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dictionaries> Dictionaries { get; set; }
        public DbSet<Words> Words { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<SubscribedDictionary> SubscribedDictionary { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dictionaries>(cs =>
            {
                cs.HasMany(c => c.Words)
                    .WithOne(w => w.Dictionary)
                    .HasForeignKey(w => w.DictionaryId)
                    .OnDelete(DeleteBehavior.Cascade);

                cs.HasMany(d => d.Statistics)
                    .WithOne(s => s.Dictionary)
                    .HasForeignKey(s => s.DictionaryId)
                    .OnDelete(DeleteBehavior.Cascade);

                cs.HasMany(d => d.SubscribedDictionaries)
                    .WithOne(sc => sc.Dictionary)
                    .HasForeignKey(sc => sc.DictionaryId)
                    .OnDelete(DeleteBehavior.Cascade);

                cs.HasOne(d => d.DictionaryLevelValues)
                    .WithMany(dlv => dlv.Dictionaries)
                    .HasForeignKey(d => d.DictionaryLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
