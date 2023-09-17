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
    }
}
