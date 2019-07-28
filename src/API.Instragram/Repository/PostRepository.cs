using API.Instragram.Entities;
using API.Instragram.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IQueryable<Post> Get()
        {
            var pageSettings = new PaginationSettings();
            return _context.Post.Include(x => x.Author);
        }

        public IEnumerable<Comment> GetComments(int postId)
        {
            return _context.Comment.Where(x => x.PostId == postId);
        }

        public async Task<ILookup<int, Comment>> GetCommentsByIdAsync(IEnumerable<int> postIds, PaginationSettings pageSettings)
        {
            pageSettings.PageSize = pageSettings.PageSize * postIds.Count();

            //Descobrir como faz o select abaixo no linq  {  OVER(PARTITION }

            //exec sp_executesql N'SELECT [t].*
            //FROM(
            //SELECT TOP(@__p_1)[x].[Id], [x].[AuthorId], [x].[Created], [x].[PostId], [x].[Text]
            //FROM[Comment] AS[x]
            //WHERE[x].[PostId] IN(1, 2)
            //ORDER BY ROW_NUMBER() OVER(PARTITION BY[x].[PostId] ORDER BY[x].[PostId])
            //) AS[t]
            //ORDER BY[t].[Id]
            //OFFSET @__p_2 ROWS',N'@__p_1 int,@__p_2 int',@__p_1=6,@__p_2=0



            var comments = (_context.Comment.Where(x => postIds.Contains(x.PostId))).Page(pageSettings);

            return comments.ToLookup(x => x.PostId);
        }
    }
}
