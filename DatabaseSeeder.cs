using System.Text.Json;

namespace MovieReviewConsoleApplication
{
    internal class DatabaseSeeder
    {
        // Method that takes a MovieReviewContext object and a directory path as parameters
        public static void Seed(MovieReviewContext context, string directory)
        {
            // Check if the directory exists
            if (Directory.Exists(directory))
            {
                // Get all the JSON files in the directory
                string[] files = Directory.GetFiles(directory, "*.json");

                // Loop through each file
                foreach (string file in files)
                {
                    // Read the file content as a string
                    string json = File.ReadAllText(file);

                    // Deserialize the string into a Movie object
                    Movie? movie = JsonSerializer.Deserialize<Movie>(json);

                    // Check if the movie is not null and has a valid title
                    if (movie != null && !string.IsNullOrEmpty(movie.Title))
                    {
                        // Add the movie to the Movies DbSet
                        _ = context.Movies.Add(movie);
                    }
                }

                // Save the changes to the database
                _ = context.SaveChanges();
            }
            else
            {
                // Throw an exception if the directory does not exist
                throw new ArgumentException("The directory does not exist.");
            }
        }
    }
}
