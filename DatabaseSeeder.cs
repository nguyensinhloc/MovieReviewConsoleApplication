using System.Text.Json;

namespace MovieReviewConsoleApplication
{
    internal class DatabaseSeeder
    {
        // Method that takes a MovieReviewContext object and a directory path as parameters
        public static void Seed(MovieReviewContext context, string directory)
        {// Check if the directory exists
            if (Directory.Exists(directory))
            {
                List<Movie>? phims = new List<Movie>();
                using (StreamReader sr = new StreamReader("MOCK_DATA.json"))
                {
                    string json = sr.ReadToEnd();
                    phims = JsonSerializer.Deserialize<List<Movie>>(json);
                }
                foreach (Movie item in phims!)
                {
                    var t1 = context.Movies.Add(item);

                    using (var contextt = new MovieReviewContext(@"Data Source= THIENLOC\\SQLEXPRESS;Initial Catalog=DANHGIAPHIM;Integrated Security=True;Encrypt=False; Trusted_Connection=True
                                                 ;TrustServerCertificate=True"))
                    {
                        var t11 = contextt.Movies.AddAsync(item);
                        var t12 = contextt.SaveChangesAsync();
                        Console.WriteLine($"da luu phim");

                    }
                }

                // Get all the JSON files in the directory
               /* string[] files = Directory.GetFiles(directory);

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
                }*/

                // Save the changes to the database
                //_ = context.SaveChanges();
            }
            else
            {
                // Throw an exception if the directory does not exist
                throw new ArgumentException("The directory does not exist.");
            }
            
        }
    }
}
