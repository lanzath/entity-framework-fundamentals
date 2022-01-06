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
            using var context = new BlogDataContext();

            var user = context.Users.FirstOrDefault();
            var category = context.Categories.FirstOrDefault();
            var post = new Post
            {
                Author = user,
                Body = "Meu artigo",
                Category = new Category
                {
                    Name = "Teste",
                    Slug = "teste"
                },
                CreateDate = DateTime.Now,
                Slug = "meu-artigo",
                Summary = "Neste artigo vamos conferir...",
                Title = "Meu artigo"
            };

            context.Posts.Add(post);
            context.SaveChanges();
        }
    }
}
