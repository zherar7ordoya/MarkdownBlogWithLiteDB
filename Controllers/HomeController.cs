using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Blog.Models.Repos;
using System.Linq;
namespace Blog.Controllers{
    [Route("/")]
    public class HomeController : Controller{
        public IActionResult Index(){
            //BlogRepo blogRepo = new BlogRepo();
            ////blogRepo.CreateExamplePosts();
            //return View(blogRepo.Posts.Find(
            //    p => p.Public == true && 
            //    p.Created <= DateTime.Now && 
            //    p.Deleted == false)
            //);

            // Usamos 'using' para asegurarnos de que la base de datos se libere correctamente.
            using (var blogRepo = new BlogRepo())
            {
                // Consultamos los datos de la base de datos dentro del bloque 'using'.
                var posts = blogRepo.Posts.Find(p => p.Public == true &&
                                                p.Created <= DateTime.Now &&
                                                p.Deleted == false).ToList();

                // Pasamos solo los datos (posts) a la vista, no la instancia de 'BlogRepo'.
                return View(posts);
            }
        }
    }
}