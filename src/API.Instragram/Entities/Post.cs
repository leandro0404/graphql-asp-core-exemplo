using System;
using System.Collections.Generic;

namespace API.Instragram.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int Likes { get; set; }
        public ICollection<Comment> Comments{ get; set; }
        public Author Author { get; set; }
    }
}
