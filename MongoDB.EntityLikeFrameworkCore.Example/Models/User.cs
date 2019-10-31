using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityLikeFrameworkCore.Annotation;
using System;

namespace MongoDB.EntityLikeFrameworkCore.Example.Models
{
    [Collection("user")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime BirthDate { get; set; }
    }
}