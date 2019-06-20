using API.Instragram.Entities;
using System.Collections.Generic;

namespace API.Instragram.Repository
{
    public interface IPostRepostirory
    {
        IEnumerable<Post> Get(PaginationSettings pageSettings);
        Post Created(Post post);

    }
}
