using Mongo_CRUD.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_CRUD.Repositories
{
    public interface IMovieRepository
    { 
        Task<ObjectId> Create(Movie movie); 
        Task<Movie> Get(ObjectId objectId);
        Task<IEnumerable<Movie>> Get(); 
        Task<long> CountAll();
        Task<bool> Update(ObjectId objectId, Movie movie);
        Task<bool> Delete(ObjectId objectId);
        Task CreateIndexForTitle();

        void DoMultipleOperations();
    }
}
