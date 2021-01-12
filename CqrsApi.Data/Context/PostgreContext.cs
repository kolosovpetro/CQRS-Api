using CqrsApi.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CqrsApi.Data.Context
{
    public class PostgreContext : DbContext
    {
        public PostgreContext()
        {
        }

        public PostgreContext(DbContextOptions<PostgreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Copy> Copies { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<Starring> Starrings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // this is only for manual migration to docker postgres container
                optionsBuilder.UseNpgsql(
                    "Server=localhost;Port=6000;User Id=postgres;Password=postgres;Database=CqrsApiDatabase;"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actors");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");
            });

            modelBuilder.Entity<Copy>(entity =>
            {
                entity.ToTable("copies");

                entity.HasIndex(e => e.MovieId, "IX_copies_movie_id");

                entity.Property(e => e.CopyId).HasColumnName("copy_id");

                entity.Property(e => e.Available).HasColumnName("available");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Copies)
                    .HasForeignKey(d => d.MovieId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name");

                entity.Property(e => e.Salary).HasColumnName("salary");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movies");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.AgeRestriction).HasColumnName("age_restriction");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasData(
                    new Movie(1, "Star Wars Episode IV: A New Hope", 1979, 12, 10f),
                    new Movie(2, "Ghostbusters", 1984, 12, 5.5f),
                    new Movie(3, "Terminator", 1984, 15, 8.5f),
                    new Movie(4, "Taxi Driver", 1976, 17, 5f),
                    new Movie(5, "Platoon", 1986, 18, 5f),
                    new Movie(6, "Frantic", 1988, 15, 8.5f),
                    new Movie(7, "Ronin", 1998, 13, 9.5f),
                    new Movie(8, "Analyze This", 1999, 16, 10.5f),
                    new Movie(9, "Leon: the Professional", 1994, 16, 8.5f),
                    new Movie(10, "Mission Impossible", 1996, 13, 8.5f));
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => new {e.ClientId, e.CopyId})
                    .HasName("rentals_pkey");

                entity.ToTable("rentals");

                entity.HasIndex(e => e.CopyId, "IX_rentals_copy_id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.CopyId).HasColumnName("copy_id");

                entity.Property(e => e.DateOfRental).HasColumnName("date_of_rental");

                entity.Property(e => e.DateOfReturn).HasColumnName("date_of_return");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.ClientId);

                entity.HasOne(d => d.Copy)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.CopyId);
            });

            modelBuilder.Entity<Starring>(entity =>
            {
                entity.HasKey(e => new {e.ActorId, e.MovieId});

                entity.ToTable("starring");

                entity.HasIndex(e => e.MovieId, "IX_starring_movie_id");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Starrings)
                    .HasForeignKey(d => d.ActorId);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Starrings)
                    .HasForeignKey(d => d.MovieId);
            });
        }
    }
}