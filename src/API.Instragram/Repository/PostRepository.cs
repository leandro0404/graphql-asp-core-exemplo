using API.Instragram.Entities;
using API.Instragram.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API.Instragram.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext _context;
        public PostRepository(PostDbContext context)
        {
            _context = context;
        }

        public Post Created(Post post)
        {
            _context.Post.Add(post);
            _context.SaveChanges();
            return post;
        }

        public IEnumerable<Post> Get()
        {
            return _context.Post.Include(x => x.Author).AsQueryable();
        }

        public IEnumerable<Comment> GetComments(int postId)
        {
            return _context.Comment.Where(x => x.PostId == postId).AsQueryable();
        }
    }
}
