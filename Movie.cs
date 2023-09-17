using System.ComponentModel.DataAnnotations;

namespace MovieReviewConsoleApplication
{
    internal class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Director { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Country { get; set; }

        // Navigation property to access the reviews of this movie
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
