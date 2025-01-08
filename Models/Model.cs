using LiteDB;
using System;


namespace Blog.Models;


public class Model
{
    [BsonId]
    public ObjectId _id { get; set; }  // El identificador único de LiteDB
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public bool Deleted { get; set; } = false;
}