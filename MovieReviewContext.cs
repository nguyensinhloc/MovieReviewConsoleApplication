using Microsoft.EntityFrameworkCore;

namespace MovieReviewConsoleApplication
{
    internal class MovieReviewContext : DbContext
    {
        // Constructor that takes a connection string as a parameter
        public MovieReviewContext(string connectionString) : base(GetOptions(connectionString))
        {

        }

        // Method that creates a DbContextOptions object from a connection string
        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
        }

        // DbSet properties to access the tables in the database
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Override the OnModelCreating method to configure the model using fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Movie entity
            modelBuilder.Entity<Movie>()
            .HasKey(m => m.Id) // Specify the primary key
            .HasAlternateKey(m => m.Title); // Create a unique index on the title column

            modelBuilder.Entity<Movie>()
                .Property(m => m.Title) // Configure the title property
                .IsRequired() // Make it required
                .HasMaxLength(100); // Set the maximum length to 100

            modelBuilder.Entity<Movie>()
                .Property(m => m.Year) // Configure the year property
                .IsRequired(); // Make it required

            modelBuilder.Entity<Movie>()
                .Property(m => m.Director) // Configure the director property
                .IsRequired() // Make it required
                .HasMaxLength(50); // Set the maximum length to 50

            modelBuilder.Entity<Movie>()
                .Property(m => m.Country) // Configure the country property
                .IsRequired() // Make it required
                .HasMaxLength(50); // Set the maximum length to 50

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews) // Configure the relationship with Review entity
                .WithOne(r => r.Movie) // Specify that one movie has many reviews and one review has one movie
                .HasForeignKey(r => r.MovieId) // Specify the foreign key in Review entity
                .OnDelete(DeleteBehavior.Cascade); // Specify that deleting a movie will also delete its reviews

            // Configure the Review entity
            modelBuilder.Entity<Review>()
                .HasKey(r => r.Id); // Specify the primary key

            modelBuilder.Entity<Review>()
                .Property(r => r.Title) // Configure the title property
                .IsRequired() // Make it required
                .HasMaxLength(100); // Set the maximum length to 100

            modelBuilder.Entity<Review>()
                .Property(r => r.Content) // Configure the content property
                .IsRequired(); // Make it required

            modelBuilder.Entity<Review>()
                .Property(r => r.Rating) // Configure the rating property
                .IsRequired() // Make it required
                .HasCheckConstraint("CK_Rating", "[Rating] BETWEEN 1 AND 5"); // Set the check constraint

            base.OnModelCreating(modelBuilder); // Call the base method at the end
        }
    }
}
