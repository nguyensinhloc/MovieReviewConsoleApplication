using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewConsoleApplication
{
    internal class Review
    {
        // No data annotations needed
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }

        // Foreign key to reference the movie of this review
        public int MovieId { get; set; }

        // Navigation property to access the movie of this review
        public virtual Movie? Movie { get; set; }
    }
}
