using System;
using System.Linq;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog
{
    class Program
    {
        static void Main(string[] args)
        {
            // Utilização de using para dispose automático da classe.
            using var context = new BlogDataContext();

            var user = new User
            {
                Name = "Xablau",
                Slug = "xablau",
                Email = "xablau@email.com",
                Bio = "Master of chaos and codes",
                Image = "http://i.imgur.com/LgKYu37.png",
                PasswordHash = "$2y$10$HTzRb6KfMkrzUYrhEbvuq.9Qf8Bo2gatafj9wi2c890P6sjNNCBWm"
            };

            var category = new Category
            {
                Name = "Infra",
                Slug = "infra"
            };

            var post = new Post
            {
                Author = user,
                Category = category,
                Body = "<p>Hello World</p>",
                Slug = "comecando-com-docker",
                Summary = "Neste artigo vamos aprender Docker",
                Title = "Começando com Docker",
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            };

            // EF gerencia as entidades de modo a entender seus relacionamentos
            // e os insere na ordem correta no banco como uma Transaction.
            context.Posts.Add(post);
            context.SaveChanges();
        }
    }
}
