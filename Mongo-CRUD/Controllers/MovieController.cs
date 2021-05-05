using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mongo_CRUD.Model;
using Mongo_CRUD.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(Movie movie)
        {
            var id = await movieRepository.Create(movie);

            return Ok(id.ToString());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await movieRepository.Get(ObjectId.Parse(id));

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CountAll()
        {
            var result = await movieRepository.CountAll();

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Get()
        {
            var movies = await movieRepository.Get();

            return Ok(movies);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CreateIndexForTitle()
        {
            await movieRepository.CreateIndexForTitle();

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult DoMultipleOperations()
        {
            movieRepository.DoMultipleOperations();

            return Ok();
        }


        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Update(string id, Movie movie)
        {
            var result = await movieRepository.Update(ObjectId.Parse(id), movie);

            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await movieRepository.Delete(ObjectId.Parse(id));

            return Ok(result);
        }
    }
}
