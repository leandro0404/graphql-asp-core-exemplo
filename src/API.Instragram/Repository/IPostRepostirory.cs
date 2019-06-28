using API.Instragram.Entities;
using System.Collections.Generic;

namespace API.Instragram.Repository
{
    public interface IPostRepository
    {
        IEnumerable<Post> Get();
        Post Created(Post post);
        IEnumerable<Comment> GetComments(int postId);

    }
}
