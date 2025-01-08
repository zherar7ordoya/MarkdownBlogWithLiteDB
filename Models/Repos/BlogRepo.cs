using System;
using LiteDB;
using System.Collections.Generic; // Required for using collections such as List

namespace Blog.Models.Repos;


public class BlogRepo : IDisposable
{
    public LiteDatabase DB { get; set; }
    public ILiteCollection<Post> Posts { get; set; }

    public BlogRepo()
    {
        DB = new LiteDatabase(@"Data/Blog.db");
        Posts = DB.GetCollection<Post>("posts");
    }

    // Implementar Dispose para liberar la base de datos
    public void Dispose()
    {
        DB?.Dispose();
    }
}
