using API.Instragram.Entities;
using System.Collections.Generic;
using System.Linq;

namespace API.Instragram.Repository
{
    public class PostRepository : IPostRepostirory
    {

        public PostRepository()
        {
            _posts = FakeRepository();
        }
        private static List<Post> _posts { get; set; }

        private List<Post> FakeRepository()
        {
            List<Post> posts = new List<Post>();
            for (int i = 0; i < 100; i++)
            {
                posts.Add(new Post
                {
                    Id = i,
                    Description = "Teste " + i,
                    Comment = new List<Comment> {
                          new Comment { Id = i, Text = "blalblalbal"}

                      },
                    Author = new Author { Id = i, Name = "Nome " + i }
                });
            }
            return posts;
        }

        public Post Created(Post post)
        {
            post.Id = _posts.Count + 1;
            _posts.Add(post);
            return post;
        }

        public IEnumerable<Post> Get(PaginationSettings pageSettings)
        {
            return _posts.AsQueryable().Page(pageSettings);
        }
    }
}
