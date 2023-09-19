using Microsoft.EntityFrameworkCore;

namespace MovieReviewConsoleApplication
{
    internal class Program
    {
        // Method to view list of movies
        public static void ViewMovies(MovieReviewContext context)
        {
            // Initialize variables to store the current page and the total number of pages
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling(context.Movies.Count() / 10.0);

            // Loop until the user chooses to go back to the main menu
            while (true)
            {
                // Clear the console and display the header
                Console.Clear();
                Console.WriteLine("View list of movies");
                Console.WriteLine("-------------------");

                // Query the database to get the movies for the current page
                List<Movie> movies = context.Movies.OrderBy(m => m.Id).Skip((currentPage - 1) * 10).Take(10).ToList();

                // Display the movies in a table format
                Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                Console.WriteLine(new string('-', 85));
                foreach (Movie? movie in movies)
                {
                    Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                }
                Console.WriteLine(new string('-', 85));

                // Display the pagination information and the options
                Console.WriteLine("Page {0} of {1}", currentPage, totalPages);
                Console.WriteLine("N - Next page | P - Previous page | B - Back to main menu | Enter movie Id for details");

                // Get the user input and validate it
                string input = Console.ReadLine();
                if (input.ToUpper() == "N")
                {
                    // If the user chooses to go to the next page, increment the current page and check if it is valid
                    currentPage++;
                    if (currentPage > totalPages)
                    {
                        // If the current page exceeds the total number of pages, display an error message and wait for a key press
                        Console.WriteLine("There is no next page. Press any key to continue.");
                        _ = Console.ReadKey();
                        currentPage--;
                    }
                }
                else if (input.ToUpper() == "P")
                {
                    // If the user chooses to go to the previous page, decrement the current page and check if it is valid
                    currentPage--;
                    if (currentPage < 1)
                    {
                        // If the current page is less than one, display an error message and wait for a key press
                        Console.WriteLine("There is no previous page. Press any key to continue.");
                        _ = Console.ReadKey();
                        currentPage++;
                    }
                }
                else if (input.ToUpper() == "B")
                {
                    // If the user chooses to go back to the main menu, break out of the loop
                    break;
                }
                else
                {
                    // If the user enters a movie Id, try to parse it and check if it is valid
                    if (int.TryParse(input, out int movieId) && context.Movies.Any(m => m.Id == movieId))
                    {
                        // If the movie Id is valid, call another method to display the movie details and options
                        ViewMovieDetails(context, movieId);
                    }
                    else
                    {
                        // If the movie Id is invalid, display an error message and wait for a key press
                        Console.WriteLine("Invalid input. Please enter a valid option or movie Id. Press any key to continue.");
                        _ = Console.ReadKey();
                    }
                }
            }
        }
        // Method to view movie details and options
        public static void ViewMovieDetails(MovieReviewContext context, int movieId)
        {
            // Loop until the user chooses to go back to the movie list
            while (true)
            {
                // Clear the console and display the header
                Console.Clear();
                Console.WriteLine("View movie details and options");
                Console.WriteLine("-----------------------------");

                // Query the database to get the movie with the given Id
                Movie? movie = context.Movies.Include(m => m.Reviews).FirstOrDefault(m => m.Id == movieId);

                // Display the movie information in a table format
                Console.WriteLine("{0,-15}{1,-30}", "Id", movie.Id);
                Console.WriteLine("{0,-15}{1,-30}", "Title", movie.Title);
                Console.WriteLine("{0,-15}{1,-30}", "Year", movie.Year);
                Console.WriteLine("{0,-15}{1,-30}", "Director", movie.Director);
                Console.WriteLine("{0,-15}{1,-30}", "Country", movie.Country);
                Console.WriteLine("{0,-15}{1,-30}", "Number of reviews", movie.Reviews.Count);
                Console.WriteLine(new string('-', 45));

                // Display the options for the movie
                Console.WriteLine("E - Edit information | D - Delete movie | A - Add review | S - See all reviews | B - Back to movie list");

                // Get the user input and validate it
                string input = Console.ReadLine();
                if (input.ToUpper() == "E")
                {
                    // If the user chooses to edit the movie information, call another method to do so
                    EditMovieInformation(context, movieId);
                }
                else if (input.ToUpper() == "D")
                {
                    // If the user chooses to delete the movie, call another method to do so
                    DeleteMovie(context, movieId);
                    // Break out of the loop since the movie is deleted
                    break;
                }
                else if (input.ToUpper() == "A")
                {
                    // If the user chooses to add a review for the movie, call another method to do so
                    AddReview(context, movieId);
                }
                else if (input.ToUpper() == "S")
                {
                    // If the user chooses to see all reviews for the movie, call another method to do so
                    SeeAllReviews(context, movieId);
                }
                else if (input.ToUpper() == "B")
                {
                    // If the user chooses to go back to the movie list, break out of the loop
                    break;
                }
                else
                {
                    // If the user enters an invalid input, display an error message and wait for a key press
                    Console.WriteLine("Invalid input. Please enter a valid option. Press any key to continue.");
                    _ = Console.ReadKey();
                }
            }
        }
        // Method to edit movie information
        public static void EditMovieInformation(MovieReviewContext context, int movieId)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Edit movie information");
            Console.WriteLine("----------------------");

            // Query the database to get the movie with the given Id
            Movie? movie = context.Movies.FirstOrDefault(m => m.Id == movieId);

            // Display the current movie information in a table format
            Console.WriteLine("{0,-15}{1,-30}", "Id", movie.Id);
            Console.WriteLine("{0,-15}{1,-30}", "Title", movie.Title);
            Console.WriteLine("{0,-15}{1,-30}", "Year", movie.Year);
            Console.WriteLine("{0,-15}{1,-30}", "Director", movie.Director);
            Console.WriteLine("{0,-15}{1,-30}", "Country", movie.Country);
            Console.WriteLine(new string('-', 45));

            // Prompt the user to enter new values for each property of the movie
            Console.WriteLine("Enter new values for each property. Leave blank to keep the current value.");
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Year: ");
            string year = Console.ReadLine();
            Console.Write("Director: ");
            string director = Console.ReadLine();
            Console.Write("Country: ");
            string country = Console.ReadLine();

            // Validate the user input and update the movie properties accordingly
            if (!string.IsNullOrWhiteSpace(title))
            {
                // If the user enters a non-empty title, update the movie title
                movie.Title = title;
            }
            if (!string.IsNullOrWhiteSpace(year))
            {
                // If the user enters a non-empty year, try to parse it and check if it is valid
                if (int.TryParse(year, out int yearValue) && yearValue > 0)
                {
                    // If the year is valid, update the movie year
                    movie.Year = yearValue;
                }
                else
                {
                    // If the year is invalid, display an error message and wait for a key press
                    Console.WriteLine("Invalid year. Please enter a positive integer. Press any key to continue.");
                    _ = Console.ReadKey();
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(director))
            {
                // If the user enters a non-empty director, update the movie director
                movie.Director = director;
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                // If the user enters a non-empty country, update the movie country
                movie.Country = country;
            }

            // Save the changes to the database and display a confirmation message
            _ = context.SaveChanges();
            Console.WriteLine("Movie information updated successfully. Press any key to continue.");
            _ = Console.ReadKey();
        }
        // Method to delete movie
        public static void DeleteMovie(MovieReviewContext context, int movieId)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Delete movie");
            Console.WriteLine("------------");

            // Query the database to get the movie with the given Id
            Movie? movie = context.Movies.Include(m => m.Reviews).FirstOrDefault(m => m.Id == movieId);

            // Display the movie information in a table format
            Console.WriteLine("{0,-15}{1,-30}", "Id", movie.Id);
            Console.WriteLine("{0,-15}{1,-30}", "Title", movie.Title);
            Console.WriteLine("{0,-15}{1,-30}", "Year", movie.Year);
            Console.WriteLine("{0,-15}{1,-30}", "Director", movie.Director);
            Console.WriteLine("{0,-15}{1,-30}", "Country", movie.Country);
            Console.WriteLine("{0,-15}{1,-30}", "Number of reviews", movie.Reviews.Count);
            Console.WriteLine(new string('-', 45));

            // Prompt the user to confirm the deletion of the movie
            Console.WriteLine("Are you sure you want to delete this movie? This will also delete all its reviews. (Y/N)");

            // Get the user input and validate it
            string input = Console.ReadLine();
            if (input.ToUpper() == "Y")
            {
                // If the user confirms the deletion, remove the movie from the database and save the changes
                _ = context.Movies.Remove(movie);
                _ = context.SaveChanges();
                // Display a confirmation message and wait for a key press
                Console.WriteLine("Movie deleted successfully. Press any key to continue.");
                _ = Console.ReadKey();
            }
            else if (input.ToUpper() == "N")
            {
                // If the user cancels the deletion, display a message and wait for a key press
                Console.WriteLine("Movie deletion cancelled. Press any key to continue.");
                _ = Console.ReadKey();
            }
            else
            {
                // If the user enters an invalid input, display an error message and wait for a key press
                Console.WriteLine("Invalid input. Please enter Y or N. Press any key to continue.");
                _ = Console.ReadKey();
            }
        }
        // Method to add a review for a movie
        public static void AddReview(MovieReviewContext context, int movieId)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Add review");
            Console.WriteLine("----------");

            // Query the database to get the movie with the given Id
            Movie? movie = context.Movies.FirstOrDefault(m => m.Id == movieId);

            // Display the movie information in a table format
            Console.WriteLine("{0,-15}{1,-30}", "Id", movie.Id);
            Console.WriteLine("{0,-15}{1,-30}", "Title", movie.Title);
            Console.WriteLine("{0,-15}{1,-30}", "Year", movie.Year);
            Console.WriteLine("{0,-15}{1,-30}", "Director", movie.Director);
            Console.WriteLine("{0,-15}{1,-30}", "Country", movie.Country);
            Console.WriteLine(new string('-', 45));

            // Prompt the user to enter the details for the new review
            Console.WriteLine("Enter the details for the new review.");
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Content: ");
            string content = Console.ReadLine();
            Console.Write("Rating (1-5): ");
            string rating = Console.ReadLine();

            // Validate the user input and create a new review object accordingly
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(rating))
            {
                // If the user enters non-empty values for all fields, try to parse the rating and check if it is valid
                if (int.TryParse(rating, out int ratingValue) && ratingValue >= 1 && ratingValue <= 5)
                {
                    // If the rating is valid, create a new review object with the given values and the movie Id
                    Review review = new()
                    {
                        Title = title,
                        Content = content,
                        Rating = ratingValue,
                        MovieId = movieId
                    };

                    // Add the review to the database and save the changes
                    _ = context.Reviews.Add(review);
                    _ = context.SaveChanges();
                    // Display a confirmation message and wait for a key press
                    Console.WriteLine("Review added successfully. Press any key to continue.");
                    _ = Console.ReadKey();
                }
                else
                {
                    // If the rating is invalid, display an error message and wait for a key press
                    Console.WriteLine("Invalid rating. Please enter an integer between 1 and 5. Press any key to continue.");
                    _ = Console.ReadKey();
                }
            }
            else
            {
                // If the user enters an empty value for any field, display an error message and wait for a key press
                Console.WriteLine("Invalid input. Please enter non-empty values for all fields. Press any key to continue.");
                _ = Console.ReadKey();
            }
        }
        // Method to see all reviews for a movie
        public static void SeeAllReviews(MovieReviewContext context, int movieId)
        {
            // Initialize variables to store the current page and the total number of pages
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling(context.Reviews.Where(r => r.MovieId == movieId).Count() / 10.0);

            // Loop until the user chooses to go back to the movie details
            while (true)
            {
                // Clear the console and display the header
                Console.Clear();
                Console.WriteLine("See all reviews");
                Console.WriteLine("---------------");

                // Query the database to get the movie with the given Id
                Movie? movie = context.Movies.FirstOrDefault(m => m.Id == movieId);

                // Display the movie information in a table format
                Console.WriteLine("{0,-15}{1,-30}", "Id", movie.Id);
                Console.WriteLine("{0,-15}{1,-30}", "Title", movie.Title);
                Console.WriteLine("{0,-15}{1,-30}", "Year", movie.Year);
                Console.WriteLine("{0,-15}{1,-30}", "Director", movie.Director);
                Console.WriteLine("{0,-15}{1,-30}", "Country", movie.Country);
                Console.WriteLine(new string('-', 45));

                // Query the database to get the reviews for the current page
                List<Review> reviews = context.Reviews.Where(r => r.MovieId == movieId).OrderBy(r => r.Id).Skip((currentPage - 1) * 10).Take(10).ToList();

                // Display the reviews in a table format
                Console.WriteLine("{0,-5}{1,-30}{2,-50}{3,-10}", "Id", "Title", "Content", "Rating");
                Console.WriteLine(new string('-', 95));
                foreach (Review? review in reviews)
                {
                    Console.WriteLine("{0,-5}{1,-30}{2,-50}{3,-10}", review.Id, review.Title, review.Content, review.Rating);
                }
                Console.WriteLine(new string('-', 95));

                // Display the pagination information and the options
                Console.WriteLine("Page {0} of {1}", currentPage, totalPages);
                Console.WriteLine("N - Next page | P - Previous page | B - Back to movie details");

                // Get the user input and validate it
                string input = Console.ReadLine();
                if (input.ToUpper() == "N")
                {
                    // If the user chooses to go to the next page, increment the current page and check if it is valid
                    currentPage++;
                    if (currentPage > totalPages)
                    {
                        // If the current page exceeds the total number of pages, display an error message and wait for a key press
                        Console.WriteLine("There is no next page. Press any key to continue.");
                        _ = Console.ReadKey();
                        currentPage--;
                    }
                }
                else if (input.ToUpper() == "P")
                {
                    // If the user chooses to go to the previous page, decrement the current page and check if it is valid
                    currentPage--;
                    if (currentPage < 1)
                    {
                        // If the current page is less than one, display an error message and wait for a key press
                        Console.WriteLine("There is no previous page. Press any key to continue.");
                        _ = Console.ReadKey();
                        currentPage++;
                    }
                }
                else if (input.ToUpper() == "B")
                {
                    // If the user chooses to go back to the movie details, break out of the loop
                    break;
                }
                else
                {
                    // If the user enters an invalid input, display an error message and wait for a key press
                    Console.WriteLine("Invalid input. Please enter a valid option. Press any key to continue.");
                    _ = Console.ReadKey();
                }
            }
        }
        // Method to add a new movie to the database
        public static void AddMovie(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Add new movie");
            Console.WriteLine("-------------");

            // Prompt the user to enter the details for the new movie
            Console.WriteLine("Enter the details for the new movie.");
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Year: ");
            string year = Console.ReadLine();
            Console.Write("Director: ");
            string director = Console.ReadLine();
            Console.Write("Country: ");
            string country = Console.ReadLine();

            // Validate the user input and create a new movie object accordingly
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(year) && !string.IsNullOrWhiteSpace(director) && !string.IsNullOrWhiteSpace(country))
            {
                // If the user enters non-empty values for all fields, try to parse the year and check if it is valid
                if (int.TryParse(year, out int yearValue) && yearValue > 0)
                {
                    // If the year is valid, create a new movie object with the given values
                    Movie movie = new()
                    {
                        Title = title,
                        Year = yearValue,
                        Director = director,
                        Country = country
                    };

                    // Add the movie to the database and save the changes
                    _ = context.Movies.Add(movie);
                    _ = context.SaveChanges();
                    // Display a confirmation message and wait for a key press
                    Console.WriteLine("Movie added successfully. Press any key to continue.");
                    _ = Console.ReadKey();
                }
                else
                {
                    // If the year is invalid, display an error message and wait for a key press
                    Console.WriteLine("Invalid year. Please enter a positive integer. Press any key to continue.");
                    _ = Console.ReadKey();
                }
            }
            else
            {
                // If the user enters an empty value for any field, display an error message and wait for a key press
                Console.WriteLine("Invalid input. Please enter non-empty values for all fields. Press any key to continue.");
                _ = Console.ReadKey();
            }
        }
        // Method to filter movies by various criteria
        public static void FilterMovies(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Filter movies by");
            Console.WriteLine("----------------");

            // Display the options for filtering
            Console.WriteLine("A - Name | B - Year of manufacture | C - Director | D - Nation | E - Back to main menu");

            // Get the user input and validate it
            string input = Console.ReadLine();
            if (input.ToUpper() == "A")
            {
                // If the user chooses to filter by name, call another method to do so
                FilterMoviesByName(context);
            }
            else if (input.ToUpper() == "B")
            {
                // If the user chooses to filter by year of manufacture, call another method to do so
                FilterMoviesByYear(context);
            }
            else if (input.ToUpper() == "C")
            {
                // If the user chooses to filter by director, call another method to do so
                FilterMoviesByDirector(context);
            }
            else if (input.ToUpper() == "D")
            {
                // If the user chooses to filter by nation, call another method to do so
                FilterMoviesByNation(context);
            }
            else if (input.ToUpper() == "E")
            {
                // If the user chooses to go back to the main menu, return from the method
                return;
            }
            else
            {
                // If the user enters an invalid input, display an error message and wait for a key press
                Console.WriteLine("Invalid input. Please enter a valid option. Press any key to continue.");
                _ = Console.ReadKey();
            }
        }
        // Method to filter movies by name
        public static void FilterMoviesByName(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Filter movies by name");
            Console.WriteLine("---------------------");

            // Prompt the user to enter a name or a part of a name to search for
            Console.WriteLine("Enter a name or a part of a name to search for:");
            string name = Console.ReadLine();

            // Validate the user input and query the database to get the movies that match the name
            if (!string.IsNullOrWhiteSpace(name))
            {
                // If the user enters a non-empty name, use the Contains method to perform a case-insensitive search
                List<Movie> movies = context.Movies.Where(m => m.Title.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                // Check if any movies are found
                if (movies.Count > 0)
                {
                    // If some movies are found, display them in a table format
                    Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                    Console.WriteLine(new string('-', 85));
                    foreach (Movie? movie in movies)
                    {
                        Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                    }
                    Console.WriteLine(new string('-', 85));
                }
                else
                {
                    // If no movies are found, display a message
                    Console.WriteLine("No movies found with the given name.");
                }
            }
            else
            {
                // If the user enters an empty name, display an error message
                Console.WriteLine("Invalid input. Please enter a non-empty name.");
            }

            // Wait for a key press before returning to the filter menu
            Console.WriteLine("Press any key to continue.");
            _ = Console.ReadKey();
        }
        // Method to filter movies by year of manufacture
        public static void FilterMoviesByYear(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Filter movies by year of manufacture");
            Console.WriteLine("------------------------------------");

            // Prompt the user to enter a year or a range of years to search for
            Console.WriteLine("Enter a year or a range of years to search for (e.g. 2020 or 2010-2020):");
            string year = Console.ReadLine();

            // Validate the user input and query the database to get the movies that match the year
            if (!string.IsNullOrWhiteSpace(year))
            {
                // If the user enters a non-empty year, try to parse it as an integer or a range of integers
                if (int.TryParse(year, out int yearValue) && yearValue > 0)
                {
                    // If the year is a valid positive integer, use the Equals method to perform an exact match
                    List<Movie> movies = context.Movies.Where(m => m.Year.Equals(yearValue)).ToList();

                    // Check if any movies are found
                    if (movies.Count > 0)
                    {
                        // If some movies are found, display them in a table format
                        Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                        Console.WriteLine(new string('-', 85));
                        foreach (Movie? movie in movies)
                        {
                            Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                        }
                        Console.WriteLine(new string('-', 85));
                    }
                    else
                    {
                        // If no movies are found, display a message
                        Console.WriteLine("No movies found with the given year.");
                    }
                }
                else if (year.Contains("-") && int.TryParse(year.Split("-")[0], out int startYear) && int.TryParse(year.Split("-")[1], out int endYear) && startYear > 0 && endYear > 0 && startYear <= endYear)
                {
                    // If the year is a valid range of positive integers, use the Between method to perform a range match
                    List<Movie> movies = context.Movies.Where(m => m.Year.Between(startYear, endYear)).ToList();

                    // Check if any movies are found
                    if (movies.Count > 0)
                    {
                        // If some movies are found, display them in a table format
                        Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                        Console.WriteLine(new string('-', 85));
                        foreach (Movie? movie in movies)
                        {
                            Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                        }
                        Console.WriteLine(new string('-', 85));
                    }
                    else
                    {
                        // If no movies are found, display a message
                        Console.WriteLine("No movies found within the given range of years.");
                    }
                }
                else
                {
                    // If the year is invalid, display an error message
                    Console.WriteLine("Invalid input. Please enter a positive integer or a range of positive integers separated by a dash.");
                }
            }
            else
            {
                // If the user enters an empty year, display an error message
                Console.WriteLine("Invalid input. Please enter a non-empty value.");
            }

            // Wait for a key press before returning to the filter menu
            Console.WriteLine("Press any key to continue.");
            _ = Console.ReadKey();
        }
        // Method to filter movies by director
        public static void FilterMoviesByDirector(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Filter movies by director");
            Console.WriteLine("-------------------------");

            // Prompt the user to enter a director or a part of a director name to search for
            Console.WriteLine("Enter a director or a part of a director name to search for:");
            string director = Console.ReadLine();

            // Validate the user input and query the database to get the movies that match the director
            if (!string.IsNullOrWhiteSpace(director))
            {
                // If the user enters a non-empty director, use the Contains method to perform a case-insensitive search
                List<Movie> movies = context.Movies.Where(m => m.Director.Contains(director, StringComparison.OrdinalIgnoreCase)).ToList();

                // Check if any movies are found
                if (movies.Count > 0)
                {
                    // If some movies are found, display them in a table format
                    Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                    Console.WriteLine(new string('-', 85));
                    foreach (Movie? movie in movies)
                    {
                        Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                    }
                    Console.WriteLine(new string('-', 85));
                }
                else
                {
                    // If no movies are found, display a message
                    Console.WriteLine("No movies found with the given director.");
                }
            }
            else
            {
                // If the user enters an empty director, display an error message
                Console.WriteLine("Invalid input. Please enter a non-empty director.");
            }

            // Wait for a key press before returning to the filter menu
            Console.WriteLine("Press any key to continue.");
            _ = Console.ReadKey();
        }
        // Method to filter movies by nation
        public static void FilterMoviesByNation(MovieReviewContext context)
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Filter movies by nation");
            Console.WriteLine("-----------------------");

            // Prompt the user to enter a nation or a part of a nation name to search for
            Console.WriteLine("Enter a nation or a part of a nation name to search for:");
            string nation = Console.ReadLine();

            // Validate the user input and query the database to get the movies that match the nation
            if (!string.IsNullOrWhiteSpace(nation))
            {
                // If the user enters a non-empty nation, use the Contains method to perform a case-insensitive search
                List<Movie> movies = context.Movies.Where(m => m.Country.Contains(nation, StringComparison.OrdinalIgnoreCase)).ToList();

                // Check if any movies are found
                if (movies.Count > 0)
                {
                    // If some movies are found, display them in a table format
                    Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", "Id", "Title", "Year", "Director", "Country");
                    Console.WriteLine(new string('-', 85));
                    foreach (Movie? movie in movies)
                    {
                        Console.WriteLine("{0,-5}{1,-30}{2,-10}{3,-20}{4,-20}", movie.Id, movie.Title, movie.Year, movie.Director, movie.Country);
                    }
                    Console.WriteLine(new string('-', 85));
                }
                else
                {
                    // If no movies are found, display a message
                    Console.WriteLine("No movies found with the given nation.");
                }
            }
            else
            {
                // If the user enters an empty nation, display an error message
                Console.WriteLine("Invalid input. Please enter a non-empty nation.");
            }

            // Wait for a key press before returning to the filter menu
            Console.WriteLine("Press any key to continue.");
            _ = Console.ReadKey();
        }
        // Method to exit the application
        public static void ExitApplication()
        {
            // Clear the console and display a farewell message
            Console.Clear();
            Console.WriteLine("Thank you for using the Movie Review Application. Have a nice day!");

            // Wait for a key press before exiting
            Console.WriteLine("Press any key to exit.");
            _ = Console.ReadKey();

            // Exit the application
            Environment.Exit(0);
        }
        // Method to display the main menu and get the user choice
        public static int MainMenu()
        {
            // Clear the console and display the header
            Console.Clear();
            Console.WriteLine("Welcome to the Movie Review Application");
            Console.WriteLine("---------------------------------------");

            // Display the options for the main menu
            Console.WriteLine("1. View list of movies");
            Console.WriteLine("2. Add new movie");
            Console.WriteLine("3. Filter movies by");
            Console.WriteLine("4. Import from JSON files");
            Console.WriteLine("5. Exit application");

            // Prompt the user to enter a choice
            Console.WriteLine("Please enter your choice (1-5):");

            // Get the user input and validate it
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 5)
            {
                // If the user input is a valid integer between 1 and 5, return it as the choice
                return choice;
            }
            else
            {
                // If the user input is invalid, display an error message and wait for a key press
                Console.WriteLine("Invalid input. Please enter a valid option. Press any key to continue.");
                _ = Console.ReadKey();
                // Return -1 as an invalid choice
                return -1;
            }
        }
        // Main method to run the application
        public static void Main(string[] args)
        {
            // Create a connection string to connect to the database
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MovieReviewDb;Trusted_Connection=True;";

            // Create a MovieReviewContext object to access the database
            using MovieReviewContext context = new(connectionString);
            
            // Loop until the user chooses to exit the application
            while (true)
            {
                // Call the MainMenu method to display the main menu and get the user choice
                int choice = MainMenu();

                // Perform different actions based on the user choice
                switch (choice)
                {
                    case 1:
                        // If the user chooses to view list of movies, call the ViewMovies method
                        ViewMovies(context);
                        break;
                    case 2:
                        // If the user chooses to add new movie, call the AddMovie method
                        AddMovie(context);
                        break;
                    case 3:
                        // If the user chooses to filter movies by, call the FilterMovies method
                        FilterMovies(context);
                        break;
                    case 4:
                        //If the user chooses to import from JSON files
                        // Ask the user to enter the directory path
                        Console.WriteLine("Enter the directory path where the JSON files are located:");

                        // Get the user input and store it in a string variable
                        string? directory = Console.ReadLine();

                        // Call the Seed method to populate the database with sample data from the user-inputted directory
                        DatabaseSeeder.Seed(context, directory);
                        break;
                    case 5:
                        // If the user chooses to exit the application, call the ExitApplication method
                        ExitApplication();
                        break;
                    default:
                        // If the user enters an invalid choice, display an error message and continue the loop
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
