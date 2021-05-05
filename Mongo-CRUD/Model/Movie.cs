using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo_CRUD.Model
{
    public class Movie
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("imdbRating")]
        public double ImdbRating { get; set; }

        [BsonElement("personalRating")]
        public int PersonalRating { get; set; }

        [BsonElement("director")]
        public string Director { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

    }
}
