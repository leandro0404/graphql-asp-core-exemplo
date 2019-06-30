using API.Instragram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Instragram.Filter
{
    public class PostFilter
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public int? Likes { get; set; }
        public AuthorFilter Author { get; set; }

    }
}
