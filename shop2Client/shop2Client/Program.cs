using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http; 

namespace shop2Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        public static async Task RunAsync()  // async methods return Task or Task<T>
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56472/Webflix/");
                    //replace http://localhost:56472 above with the address that you get when you run your own WebApi Project (by clicking on Microsoft Edge)
                    //and replace Webflix with the route prefix defined in step 20

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Test get all movies
                    Console.WriteLine("All movies in database by descending release date:");
                    HttpResponseMessage response = await client.GetAsync("movies/all");  //movies/all is the route defined to reach the method GetAllMovies() in the Web Api project
                    if (response.IsSuccessStatusCode)                                    // 200.299
                    {
                        // read results 
                        var movies = await response.Content.ReadAsAsync<IEnumerable<Movie>>(); //GetAllMovies() returns a list of movies
                        foreach (var movie in movies)
                        {
                            Console.WriteLine(movie);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }

                    // Test get movie with ID 1
                    Console.WriteLine("Movie 1:");
                    response = await client.GetAsync("movies/ById/1");
                    if (response.IsSuccessStatusCode)                                                   // 200.299
                    {
                        // read results 
                        var movie = await response.Content.ReadAsAsync<Movie>();
                        Console.WriteLine(movie);
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }

                    // Test get movie IDs and titles for specified keyword
                    Console.WriteLine("Movies containing Darby in Title:");
                    response = await client.GetAsync("movies/ByKeyword/Darby");
                    if (response.IsSuccessStatusCode)  // 200.299
                    {
                        // read results 
                        var movies = await response.Content.ReadAsStringAsync();
                        foreach (var m in movies)
                        {
                            Console.WriteLine(m);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }

                    // Test POST
                    Console.WriteLine("Create new Movie");
                    Movie newMovie = new Movie() { Id = 5, Title = "Le Diner de Cons", Genre = MovieGenre.comedy, Certification = MovieCertification.universal, Rating = 8, RealeaseDate = DateTime.Parse("15/04/1998") };
                    response = await client.PostAsJsonAsync("movies/New", newMovie);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Movie {0} added", newMovie.Title);
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

