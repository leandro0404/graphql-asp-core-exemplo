using API.Instragram.Entities;
using API.Instragram.Repository.Context;
using Bogus;
using System.Collections.Generic;

namespace API.Instragram.Repository.FakeRepository
{
    public static class FakeRepository
    {
        private const string locale = "pt_BR";

        public static void AdicionarDadosTeste(PostDbContext context)
        {
            context.Post.AddRange(GetPostRepository());
            context.Author.AddRange(GetAuthorRepository());
            context.Comment.AddRange(GetComentRepository());
            context.SaveChanges();

        }

        private static List<Post> GetPostRepository()
        {
            var postFaker = new Faker<Post>(locale)
                .CustomInstantiator(f => new Post())
                .RuleFor(o => o.AuthorId, f => f.IndexFaker)
                .RuleFor(o => o.Title, f => f.Lorem.Sentences(1))
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(2))
                .RuleFor(o => o.Likes, f => f.Random.Number(1, 5));
            var posts = postFaker.Generate(20);

            return posts;
        }

        private static List<Author> GetAuthorRepository()
        {
            var authorFake = new Faker<Author>(locale)
                .CustomInstantiator(f => new Author())
                .RuleFor(o => o.Name, f => f.Name.FullName());
            var authors = authorFake.Generate(20);

            return authors;
        }

        private static List<Comment> GetComentRepository()
        {
            var authorFake = new Faker<Comment>(locale)
                .CustomInstantiator(f => new Comment())
                .RuleFor(o => o.PostId, f => f.Random.Number(1, 100))
                .RuleFor(o => o.Text, f => f.Lorem.Sentences(1));
            var authors = authorFake.Generate(1000);

            return authors;
        }

    }
}
