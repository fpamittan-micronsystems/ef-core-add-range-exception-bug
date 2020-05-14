using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ef_core_add_range_exception_bug
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var context = new EntityFrameworkDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.SampleModels.AddRange(
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 1, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 2, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 3, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 4, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 5, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 6, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 7, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 8, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 9, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 10, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 11, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 12, NonPk = "" });

                context.SaveChanges();
            }

            Console.WriteLine("Initial insert done.");

            using (var context = new EntityFrameworkDbContext())
            {
                var toAdd = new List<SampleModel>
                {
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 1, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 2, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 3, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 4, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 5, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 6, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 7, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 8, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 9, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 10, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 11, NonPk = "" },
                    new SampleModel { PkFirst = 3525, PkSecond = 381, PkThird = 12, NonPk = "" },
                    new SampleModel { PkFirst = 3497, PkSecond = 381, PkThird = 1, NonPk = "" },
                };

                context.SampleModels.UpdateRange(toAdd);

                var isSaved = false;
                do
                {
                    try
                    {
                        context.SaveChanges();
                        isSaved = true;
                    }
                    catch (DbUpdateException ex)
                        {
                        foreach (var entry in ex.Entries)
                        {
                            entry.State = EntityState.Detached;
                        }
                    }
                } while (!isSaved);
            }
        }
    }

    public class SampleModel
    {
        public int PkFirst { get; set; }
        public int PkSecond { get; set; }
        public int PkThird { get; set; }
        public string NonPk { get; set; }
    }

    public class EntityFrameworkDbContext : DbContext
    {
        public DbSet<SampleModel> SampleModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Username=postgres;Password=password;Database=testdb");
            //optionsBuilder.EnableSensitiveDataLogging (true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleModel>().HasKey(m => new { m.PkFirst, m.PkSecond, m.PkThird });
            modelBuilder.Entity<SampleModel>().Property(m => m.PkFirst).ValueGeneratedNever();
            modelBuilder.Entity<SampleModel>().Property(m => m.PkSecond).ValueGeneratedNever();
            modelBuilder.Entity<SampleModel>().Property(m => m.PkThird).ValueGeneratedNever();
            modelBuilder.Entity<SampleModel>().Property(m => m.NonPk).HasMaxLength(500);
        }
    }
}
