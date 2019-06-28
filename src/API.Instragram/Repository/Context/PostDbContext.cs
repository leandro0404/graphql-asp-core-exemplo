using API.Instragram.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Instragram.Repository.Context
{
    public class PostDbContext : DbContext
    {

        public PostDbContext()
        {

        }

        public PostDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Post> Post { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Comment> Comment { get; set; }


    }
}
