using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Blog.Models.Repos;
using Blog.Models;
using Blog.Models.Requests;
using LiteDB;


namespace Blog.Controllers;


[Route("/PostManager")]
public class PostManagerController : Controller
{
    public IActionResult Index()
    {
        using (var blogRepo = new BlogRepo())
        {
            var posts = blogRepo.Posts.FindAll().ToList();
            return View(posts);
        }
    }

    [HttpGet, Route("new/post")]
    public IActionResult NewPost()
    {
        return View();
    }

    [HttpPost, Route("new/post")]
    public IActionResult NewPost([FromForm] PostRequest PostReq)
    {
        using (var blogRepo = new BlogRepo())
        {
            if (blogRepo.Posts.Exists(p => p.Title == PostReq.Title))
            {
                return Content("Already exists");
            }

            Post Post = new()
            {
                Content = PostReq.Content,
                Title = PostReq.Title,
                Public = true,
            };

            var image = PostReq.Image;

            if (image != null)
            {
                var imgPath = Path.Join("wwwroot/assets/images", image.FileName);

                if (image.Length > 0)
                {
                    using FileStream fs = new(imgPath, FileMode.Create);
                    image.CopyTo(fs);
                }

                if (System.IO.File.Exists(imgPath))
                {
                    Post.CoverImagePath = imgPath;
                }
            }

            blogRepo.Posts.EnsureIndex(p => p.Title, true);
            blogRepo.Posts.EnsureIndex(p => p._id, true);
            blogRepo.Posts.Insert(Post);

            if (blogRepo.Posts.Exists(p => p.Title == PostReq.Title))
            {
                return StatusCode(201);
            }
        }
        return StatusCode(400);
    }

    [HttpGet, Route("/PostManager/edit/{postTitle}")]
    public IActionResult PostEdit([FromRoute] string postTitle)
    {
        postTitle = postTitle.Replace("-", " ");
        using (var blogRepo = new BlogRepo())
        {
            Post Post = blogRepo.Posts.Find(p => p.Title == postTitle).FirstOrDefault();
            return View(Post);
        }
    }

    [HttpDelete, Route("post/delete")]
    public IActionResult DeletePost([FromBody] string postTitle)
    {
        using (var blogRepo = new BlogRepo())
        {
            // blogRepo.Posts.Delete(Query.EQ("Title", postTitle));
            blogRepo.Posts.Delete(postTitle);
            if (!blogRepo.Posts.Exists(p => p.Title == postTitle))
            {
                return StatusCode(200);
            }
        }
        return StatusCode(400);
    }

    [HttpPut, Route("post/update")]
    public IActionResult UpdatePost([FromBody] Post newPost, [FromBody] string postTitle)
    {
        using (var blogRepo = new BlogRepo())
        {
            var postId = blogRepo.Posts.Find(p => p.Title == postTitle).FirstOrDefault()?._id;
            if (postId == null)
            {
                return StatusCode(404); // Not found
            }

            bool status = blogRepo.Posts.Update(postId, newPost);
            if (status)
            {
                return StatusCode(200);
            }
        }
        return StatusCode(400);
    }
}
