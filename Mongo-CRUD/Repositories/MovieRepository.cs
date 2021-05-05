using Mongo_CRUD.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mongo_CRUD.Repositories
{
    public class MovieRepository : IMovieRepository
    {

        private readonly IMongoCollection<Movie> movies;
        IMongoClient client;

        public MovieRepository(IMongoClient client)
        {
            var database = client.GetDatabase("Movies");
            var collection = database.GetCollection<Movie>(nameof(Movie));
            movies = collection;
            this.client = client;

        }

        public async Task<ObjectId> Create(Movie movie)
        {
            await movies.InsertOneAsync(movie);

            return movie.Id;
        }

        public async Task<bool> Delete(ObjectId objectId)
        {
            var filter = Builders<Movie>.Filter.Eq(c => c.Id, objectId);
            var result = await movies.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        public Task<Movie> Get(ObjectId objectId)
        {
            var filter = Builders<Movie>.Filter.Eq(c => c.Id, objectId);
            var result = movies.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Movie>> Get()
        {
            var result = await movies.Find(_ => true).ToListAsync();

            return result;
        }

        public async Task<long> CountAll()
        {
            var count = await movies.CountDocumentsAsync(_ => true);

            return count;
        }

        public async Task<bool> Update(ObjectId objectId, Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq(c => c.Id, objectId);
            var update = Builders<Movie>.Update
                .Set(m => m.Title, movie.Title)
                .Set(m => m.Year, movie.Year)
                .Set(m => m.ImdbRating, movie.ImdbRating)
                .Set(m => m.Director, movie.Director)
                .Set(m => m.PersonalRating, movie.PersonalRating)
                .Set(m => m.Description, movie.Description);
            var result = await movies.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task CreateIndexForTitle()
        {
            var indexKeysDefinition = Builders<Movie>.IndexKeys.Ascending(a => a.Title);
            await movies.Indexes.CreateOneAsync(new CreateIndexModel<Movie>(indexKeysDefinition));

        }

        public void DoMultipleOperations()
        {
            using var session = client.StartSession(); 
            var cancellationToken = CancellationToken.None;
            var result = session.WithTransaction(
                (s, ct) =>
                {
                    movies.DeleteMany(a => a.ImdbRating < 8, cancellationToken: ct);
                    movies.InsertOne(new Movie
                    {
                        Title = "Test Movie",
                        ImdbRating = 5.5,
                        Year = 2021,
                        PersonalRating = 9,
                        Director = "Christopher Nolan",
                        Description = "this is a test movie inserted from a transaction"
                    },
                        cancellationToken: ct);
                    return "Deleted bad movies and added a new one";
                },
                null,
                cancellationToken);
        }

        public enum SortField
        {
            Year,
            ImdbRating,
            PersonalRating
        }
    }
}
