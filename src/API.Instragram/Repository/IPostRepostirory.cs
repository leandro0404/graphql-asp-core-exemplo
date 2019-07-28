using API.Instragram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace API.Instragram.Repository
{
    public interface IPostRepository
    {
        IQueryable<Post> Get();
        Post Created(Post post);
        IEnumerable<Comment> GetComments(int postId);
        Task<ILookup<int, Comment>> GetCommentsByIdAsync(IEnumerable<int> postIds, PaginationSettings pageSettings);

    }
}
