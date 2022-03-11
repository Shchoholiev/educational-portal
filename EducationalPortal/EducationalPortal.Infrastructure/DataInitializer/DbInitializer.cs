using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.DataInitializer
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var q144p = new Quality { Name = "144p" };
            var q240p = new Quality { Name = "240p" };
            var q360p = new Quality { Name = "360p" };
            var q480p = new Quality { Name = "480p" };
            var q720p = new Quality { Name = "720p" };
            var q1080p = new Quality { Name = "1080p" };
            var q1440p = new Quality { Name = "1440p" };
            var q2160p = new Quality { Name = "2160p" };

            var qualities = new List<Quality>
            {
                q144p, q240p, q360p, q480p, q720p, q1080p, q1440p, q2160p
            };

            foreach (var quality in qualities)
            {
                context.Qualities.Add(quality);
            }
            context.SaveChanges();

            var csharpVideo1 = new Video
            {
                Name = "Describing An Object With Different Data Types",
                Link = "https://firebasestorage.googleapis.com/v0/b/educational-portal-584a2.appspot.com/o/Describing%20An%20Object%20With%20Different%20Data%20Types.mp4?alt=media&token=dddb5f82-3514-40dd-a798-76b4f565083e",
                Duration = DateTime.MinValue.AddMinutes(6.1),
                Quality = q360p,
            };
            context.Videos.Add(csharpVideo1);
            context.SaveChanges();

            var pdf = new Extension { Name = "pdf" };
            context.Extensions.Add(pdf);
            context.SaveChanges();

            var troelsen = new Author { FullName = "Andrew Troelsen" };
            context.Authors.Add(troelsen);
            context.SaveChanges();

            var csharpBook = new Book
            {
                Name = "Pro C# 9",
                Link = "https://firebasestorage.googleapis.com/v0/b/educational-portal-584a2.appspot.com/o/04.%20Village%20in%20India%20Myths%20and%20Realities%20author%20Vishwa%20Anand.pdf?alt=media&token=7fbe48da-df92-40c0-ac8c-666dfca7d2ad",
                PagesCount = 5,
                Extension = pdf,
                PublicationYear = 2020,
                Authors = new List<Author> { troelsen } 
            };
            context.Books.Add(csharpBook);
            context.SaveChanges();

            var resource1 = new Resource { Name = "microsoft.com" };
            context.Resources.Add(resource1);
            context.SaveChanges();

            var csharpArticle = new Article
            {
                Name = "LINQ",
                Link = "https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/",
                Resource = resource1,
            };
            context.Articles.Add(csharpArticle);
            context.SaveChanges();

            var csharpSkills = new List<Skill>
            {
                new Skill { Name = "C# Syntax" },
                new Skill { Name = "OOP" },
                new Skill { Name = "LINQ" },
            };

            foreach (var skill in csharpSkills)
            {
                context.Skills.Add(skill);
            }
            context.SaveChanges();

            var courseCSharp = new Course
            {
                Name = "CSharp",
                Thumbnail = "https://firebasestorage.googleapis.com/v0/b/educational-portal-584a2.appspot.com/o/csharp.jpg?alt=media&token=6c21dc45-aead-4083-b439-177201aa0938",
                Description = "My name is Tom Owsiak. I'm the author of \"Beginning C# Hands-On-The Core Language\" " +
                              "from Packt Publishing. Let's take a look at every this course has to offer. I have an " +
                              "updated version of a  similar course with Visual Studio 2017. Please search \"Learn C# " +
                              "with Visual Studio 2017 and Console Programs\" Update 8/24/2020: Added several new " +
                              "lessons on the bottom on concepts related to try/catch/finally and using blocks with " +
                              "C# 8.0. This comprehensive C# course is designed for beginner programmers, as you won't " +
                              "be rushing through code. It focuses on the C# language rather than the graphical aspects " +
                              "of Windows programming. The focus at first is on simple Console applications. This course " +
                              "works with Visual Studio 2013, or Visual Studio 2015. You'll learn in a detailed and " +
                              "deliberate way so you can set a foundation and move from basic to more advanced concepts easily.",
                Price = 100,
                Skills = csharpSkills,
            };
            context.Courses.Add(courseCSharp);
            context.Entry(courseCSharp).State = EntityState.Added;
            context.SaveChanges();

            var coursesMaterials1 = new CoursesMaterials
            {
                Material = csharpVideo1,
                Course = courseCSharp,
                Index = 1,
            };

            var coursesMaterials2 = new CoursesMaterials
            {
                Material = csharpVideo1,
                Course = courseCSharp,
                Index = 2,
            };

            var csharpCoursesMaterials = new List<CoursesMaterials>
            {
                coursesMaterials1, coursesMaterials2,
            };

            courseCSharp.Id = 1;
            courseCSharp.CoursesMaterials = csharpCoursesMaterials;
            //context.Courses.Add(courseCSharp);
            //context.SaveChanges();

            //context.Courses.Update(courseCSharp);
            //context.SaveChanges();
        }
    }
}
