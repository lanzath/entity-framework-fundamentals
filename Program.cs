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
            using (var context = new BlogDataContext())
            {
                // CREATE
                // context.Model.Add;
                // context.SaveChanges();

                // READ
                // context.Model.AsNoTracking().ToList();
                // context.Model.AsNoTracking().FirstOrDefault(x => x.Id == id);

                // UPDATE
                // var data = context.Model.AsNoTracking().FirstOrDefault(x => x.Id == id);
                // context.Model.Update(data);
                // context.SaveChanges();

                // DELETE
                // var data = context.Model.AsNoTracking().FirstOrDefault(x => x.Id == id);
                // context.Model.Delete(data);
                // context.SaveChanges();
            }
        }
    }
}
