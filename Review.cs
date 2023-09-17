using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewConsoleApplication
{
    internal class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        // Foreign key to reference the movie of this review
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        // Navigation property to access the movie of this review
        public virtual Movie? Movie { get; set; }
    }
}
