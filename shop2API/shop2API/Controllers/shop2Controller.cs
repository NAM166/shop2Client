using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using shop2API.Models;

namespace shop2API.Controllers
{
    [RoutePrefix("shop2")]
    public class shop2Controller : ApiController
    {
        shop2Context db = new shop2Context();
        // GET: api/shop2

        [Route("Customer/all")]
        // GET: shop2/Customers/all  -  this is how to call the method in URL after localhost bit
        //This method returns data about all Customers in the catalogue sorted in Customer ID order (High to Low)
        public IEnumerable<Movy> GetAllCustomers() //this method returns a list of Customers, called "Movy" here by Visual Studio
        {
            return db.Customers.ToList().OrderByDescending(x => x.CustomerID);
            //db is name of context (step 21)
            //Movies is name of DbSet; you can find this in file MovieModel.Context.cs
            //ToList() will print all movies contained in database to a list
            //OrderByDescending has a lambda function telling the system to order descending based on release date
        }

        [Route("customers/ByCustomerID/{CustomerID}")] //the id in curly braces refers to the variable passed into the GetById method below
        // GET: shop2/customers/ByCustomerID/1
        //This method returns data about a specific movie as specified using a Customer ID
        public Movy GetByCustomerID(int CustomerID) //only returns one movie as id is unique
        {
            return db.Customers.SingleOrDefault(x => x.CustomersID == CustomersID);
            //look for movie x so that the Id of x is equal to the id passed in method through URL
            //SingleOrDefault can be used as we are sure id is unique so it should only return a single result
        }

        // get movie by ID - solution proposed by Gary for info
        //[HttpGet]
        //[Route("movies/id/{id:int}")]
        //public IHttpActionResult GetMovieByID(int id)
        //{
        //    using (MoviesContext db = new MoviesContext())
        //    {
        //        try
        //        {
        //            var movie = db.Movies.SingleOrDefault(m => m.ID == id);
        //            if (movie != null)
        //            {
        //                return Ok(movie);
        //            }
        //            else
        //            {
        //                return BadRequest("ID not found");
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            return InternalServerError();
        //        }
        //    }
        //}

        //Solution proposed by Dermot
        //[Route("movies/{keyword}")]
        //// GET: Webflix/movies/Darby
        ////This method returns a list of movies that contain a specified keyword as a whole word in their title
        //public IEnumerable<Movy> GetByKeyword(string keyword)
        //{
        //    using (MoviesContext db = new MoviesContext())
        //    {
        //        return db.Movies.Where(x => x.Title.Contains(keyword));
        //        //Note that this returns the whole movie, not just its id and title as requested
        //    }
        //}

        [Route("movies/ByKeyword/{keyword}")]
        // GET: Webflix/movies/ByKeyword/Darby
        //This method returns a list of movie IDs and titles for movies that contain a specified keyword as a whole word in their title
        public IHttpActionResult GetAllMoviesByTitleKeyword(String keyword)
        {
            // return ID and title for matches whole word only
            var results = db.Movies.Where(m => m.Title.Contains(keyword)).Select(m => new { m.Id, m.Title });
            //return movie m where the title of m contains the keyword we are looking for, but only select the id and title of m (and save in a new object)
            if (results.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(results);
            }
        }

        [Route("movies/New")]
        // POST: Webflix/movies/New
        //This method adds a new movie in the database if there is not already a movie with the same title
        public HttpResponseMessage Post([FromBody]Movy value) //replace Movy by the class used in CA
        {
            if (value == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to read input");
            }
            if (db.Movies.Count(p => p.Title.Equals(value.Title)) != 0)
            //check if there is any title in database which matches the title of the new movie to be inserted
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Movie " + value.Title + " already exists in database.");
            }

            db.Movies.Add(value);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, "Movie " + value.Title + " added to database.");
        }

        [Route("movies/Update/{id}")]
        // PUT: Webflix/movies/Update/4
        public HttpResponseMessage Put(int id, [FromBody]Movy value)
        {
            if (value == null)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "Failed to read input");
            }

            var record = db.Movies.SingleOrDefault(p => p.Id == id);

            if (record == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Failed to find that Movie");
            }

            try
            {
                record.Title = value.Title;
                record.Rating = value.Rating;
                record.RealeaseDate = value.RealeaseDate;
                record.Genre = value.Genre;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record updated");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Update operation failed with exception {0}", e.Message);
            }
        }

        [Route("movies/Delete/{id}")]
        // DELETE: Webflix/movies/Delete/2
        public HttpResponseMessage Delete(int id)
        {
            using (MoviesContext db = new MoviesContext())
            {
                var record = db.Movies.FirstOrDefault(p => p.Id == id);

                if (record == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Failed to find that Movie");
                }

                try
                {
                    db.Movies.Remove(record);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Record deleted");

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "DELETE operation failed with exception {0}", e.Message);
                }
            }
        }

    }
}
