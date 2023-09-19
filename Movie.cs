using System.ComponentModel.DataAnnotations;

namespace MovieReviewConsoleApplication
{
    internal class Movie
    {
        // No data annotations needed
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }
        public string? Director { get; set; }
        public string? Country { get; set; }

        // Navigation property to access the reviews of this movie
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
