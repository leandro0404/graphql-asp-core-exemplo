using API.Instragram.Entities;
using System.Collections.Generic;
using System.Linq;

namespace API.Instragram.Repository
{
    public interface IPostRepository
    {
        IQueryable<Post> Get();
        Post Created(Post post);
        IEnumerable<Comment> GetComments(int postId);

    }
}
