using Bogus;
using PromptVault.Api.Models;

namespace PromptVault.Api.Database.DatabaseSeeder
{
    public class Seeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Prompts.Any())
                return;

            // Seed Tags
            var tagNames = new[] { "AI", "Creative", "Code", "Marketing", "Education", "Fun", "Business", "Science" };
            var tags = tagNames.Select(name => new Tag { Name = name }).ToList();
            context.Tags.AddRange(tags);
            context.SaveChanges();

            // Seed Prompts
            var categories = new[] { "Writing", "Programming", "Analysis", "Translation", "Summarization" };

            var promptFaker = new Faker<Prompt>()
                .RuleFor(p => p.Text, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.AuthorNote, f => f.Lorem.Sentence())
                .RuleFor(p => p.IsPublic, f => f.Random.Bool())
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1));

            var prompts = promptFaker.Generate(20);
            context.Prompts.AddRange(prompts);
            context.SaveChanges();

            // Seed PromptTags (each prompt gets 1-3 random tags)
            var random = new Random(42);
            var promptTags = new List<PromptTag>();

            foreach (var prompt in prompts)
            {
                var selectedTags = tags.OrderBy(_ => random.Next()).Take(random.Next(1, 4)).ToList();
                foreach (var tag in selectedTags)
                {
                    promptTags.Add(new PromptTag { PromptId = prompt.Id, TagId = tag.Id });
                }
            }

            context.PromptTags.AddRange(promptTags);
            context.SaveChanges();

            // Seed TestResults (each prompt gets 1-3 test results)
            var models = new[] { "gpt-4o", "gpt-4o-mini", "gpt-3.5-turbo", "claude-sonnet-4-20250514" };

            var testResultFaker = new Faker<TestResult>()
                .RuleFor(t => t.Output, f => f.Lorem.Paragraphs(2))
                .RuleFor(t => t.Rating, f => f.Random.Int(1, 5))
                .RuleFor(t => t.ModelUsed, f => f.PickRandom(models))
                .RuleFor(t => t.CreatedAt, f => f.Date.Past(1));

            foreach (var prompt in prompts)
            {
                var count = random.Next(1, 4);
                var results = testResultFaker
                    .RuleFor(t => t.PromptId, _ => prompt.Id)
                    .Generate(count);

                context.Results.AddRange(results);
            }

            context.SaveChanges();
        }
    }
}
