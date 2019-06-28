using System;

namespace API.Instragram.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public Author Author { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
