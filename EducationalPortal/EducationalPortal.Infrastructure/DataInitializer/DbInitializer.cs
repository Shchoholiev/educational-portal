using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.Identity;

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
                Link = "https://educationalportal.blob.core.windows.net/videos/Describing%20An%20Object%20With%20Different%20Data%20Types.mp4",
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
                Link = "https://educationalportal.blob.core.windows.net/books/04.%20Village%20in%20India%20Myths%20and%20Realities%20author%20Vishwa%20Anand.pdf",
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
                PublicationDate = new DateTime(2022, 2, 18),
            };
            context.Articles.Add(csharpArticle);
            context.SaveChanges();

            var csharpCyntax = new Skill { Name = "C# Syntax" };
            var oop = new Skill { Name = "OOP" };
            var linq = new Skill { Name = "LINQ" };

            var csharpSkills = new List<Skill>
            {
                csharpCyntax, oop, linq
            };

            context.Skills.AddRange(csharpSkills);
            context.SaveChanges();

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.Hash("111111");

            var user = new User
            {
                Id = "1234567890",
                Name = "Default",
                Position = "Senior .NET Developer",
                Email = "default@gmail.com",
                Avatar = "https://educationalportal.blob.core.windows.net/avatars/cute-monster-face-square-avatar-vector-stock-cute-monster-face-square-avatar-114650081.jpg",
                PasswordHash = passwordHash,
            };

            var usersSkills1 = new UsersSkills
            {
                User = user,
                Skill = csharpCyntax,
                Level = 4,
            };

            var usersSkills2 = new UsersSkills
            {
                User = user,
                Skill = oop,
                Level = 2,
            };

            var usersSkills3 = new UsersSkills
            {
                User = user,
                Skill = linq,
                Level = 8,
            };

            var usersSkills = new List<UsersSkills>
            {
                usersSkills1, usersSkills2, usersSkills3
            };

            user.UsersSkills = usersSkills;

            context.Users.Add(user);
            context.SaveChanges();

            var courseCSharp = new Course
            {
                Name = "CSharp",
                Thumbnail = "https://educationalportal.blob.core.windows.net/thumbnails/csharp.jpg",
                ShortDescription = "Learning to code C#? We have provided HD video lectures, " +
                                   "live coding sessions, and nearly 100 exercises to learn on!",
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
                Price = 80,
                Skills = csharpSkills,
                Author = user,
            };

            var coursesMaterials1 = new CoursesMaterials
            {
                Material = csharpVideo1,
                Course = courseCSharp,
                Index = 1,
            };

            var coursesMaterials2 = new CoursesMaterials
            {
                Material = csharpBook,
                Course = courseCSharp,
                Index = 2,
            };

            var coursesMaterials3 = new CoursesMaterials
            {
                Material = csharpArticle,
                Course = courseCSharp,
                Index = 3,
            };

            var csharpCoursesMaterials = new List<CoursesMaterials>
            {
                coursesMaterials1, coursesMaterials2, coursesMaterials3
            };

            courseCSharp.CoursesMaterials = csharpCoursesMaterials;

            context.Courses.Add(courseCSharp);
            context.SaveChanges();

            var usersCourses = new UsersCourses
            {
                Course = courseCSharp,
                User = user,
                LearnedMaterialsCount = 0,
                MaterialsCount = 3,
            };

            context.UsersCourses.Add(usersCourses);
            context.SaveChanges();
            
            // ----------------------------------------------------

            var html = new Skill { Name = "HTML 5" };
            var css = new Skill { Name = "CSS 3" };
            var js = new Skill { Name = "JavaScript" };
            var bootstrap5 = new Skill { Name = "Bootstrap 5" };
            var responsibleDesign = new Skill { Name = "Responsible Design" };
            var flexbox = new Skill { Name = "Flexbox" };
            var asyncJS = new Skill { Name = "Asynchronous JavaScript" };
            var bulma = new Skill { Name = "Bulma CSS Framework" };
            var nodeJS = new Skill { Name = "Node JS" };

            var webSkills = new List<Skill>
            {
                html, css, js, bootstrap5, responsibleDesign, flexbox,
                asyncJS, bulma, nodeJS
            };

            context.Skills.AddRange(webSkills);
            context.SaveChanges();

            var webVideo1 = new Video
            {
                Name = "Introduction to HTML",
                Link = "https://educationalportal.blob.core.windows.net/videos/Describing%20An%20Object%20With%20Different%20Data%20Types.mp4",
                Duration = DateTime.MinValue.AddMinutes(7.5),
                Quality = q720p,
            };
            context.Videos.Add(webVideo1);
            context.SaveChanges();

            var webVideo2 = new Video
            {
                Name = "Heading Elements",
                Link = "https://educationalportal.blob.core.windows.net/videos/Describing%20An%20Object%20With%20Different%20Data%20Types.mp4",
                Duration = DateTime.MinValue.AddMinutes(2.5),
                Quality = q1080p,
            };
            context.Videos.Add(webVideo2);
            context.SaveChanges();

            var webVideo3 = new Video
            {
                Name = "Including Styles Correctly",
                Link = "https://educationalportal.blob.core.windows.net/videos/Describing%20An%20Object%20With%20Different%20Data%20Types.mp4",
                Duration = DateTime.MinValue.AddMinutes(10.3),
                Quality = q1080p,
            };
            context.Videos.Add(webVideo3);
            context.SaveChanges();

            var haverbeke = new Author { FullName = "Marijn Haverbeke" };
            context.Authors.Add(haverbeke);
            context.SaveChanges();

            var webBook1 = new Book
            {
                Name = "Eloquent JavaScript",
                Link = "https://educationalportal.blob.core.windows.net/books/04.%20Village%20in%20India%20Myths%20and%20Realities%20author%20Vishwa%20Anand.pdf",
                PagesCount = 5,
                Extension = pdf,
                PublicationYear = 2018,
                Authors = new List<Author> { haverbeke }
            };
            context.Books.Add(webBook1);
            context.SaveChanges();

            var duckett = new Author { FullName = "Jon Duckett" };
            context.Authors.Add(duckett);
            context.SaveChanges();

            var webBook2 = new Book
            {
                Name = "HTML & CSS: Design and Build Web Sites",
                Link = "https://educationalportal.blob.core.windows.net/books/04.%20Village%20in%20India%20Myths%20and%20Realities%20author%20Vishwa%20Anand.pdf",
                PagesCount = 5,
                Extension = pdf,
                PublicationYear = 2011,
                Authors = new List<Author> { duckett }
            };
            context.Books.Add(webBook2);
            context.SaveChanges();

            var mdn = new Resource { Name = "developer.mozilla.org" };
            context.Resources.Add(mdn);
            context.SaveChanges();

            var webArticle1 = new Article
            {
                Name = "HTML Tables",
                Link = "https://developer.mozilla.org/en-US/docs/Learn/HTML/Tables",
                Resource = mdn,
                PublicationDate = new DateTime(2021, 4, 13),
            };
            context.Articles.Add(webArticle1);
            context.SaveChanges();

            var webArticle2 = new Article
            {
                Name = "What is JavaScript?",
                Link = "https://developer.mozilla.org/en-US/docs/Learn/JavaScript/First_steps/What_is_JavaScript",
                Resource = mdn,
                PublicationDate = new DateTime(2021, 7, 2),
            };
            context.Articles.Add(webArticle2);
            context.SaveChanges();

            var webCourse = new Course
            {
                Name = "The Web Developer Bootcamp 2022",
                Thumbnail = "https://educationalportal.blob.core.windows.net/thumbnails/webdev.jpg",
                ShortDescription = "The only course you need to learn web development " +
                                   "- HTML, CSS, JS, Node, and More!",
                Description = "Hi! Welcome to the brand new version of The Web Developer Bootcamp, " +
                              "Udemy's most popular web development course.  This course was just completely " +
                              "overhauled to prepare students for the 2022 job market, with over 60 hours of " +
                              "brand new content. This is the only course you need to learn web development. " +
                              "There are a lot of options for online developer training, but this course is " +
                              "without a doubt the most comprehensive and effective on the market.",
                Price = 150,
                Skills = webSkills,
                Author = user,
            };

            var webMaterials = new List<CoursesMaterials>
            {
                new CoursesMaterials { Course = webCourse, Material = webVideo1, Index = 1 },
                new CoursesMaterials { Course = webCourse, Material = webArticle1, Index = 5 },
                new CoursesMaterials { Course = webCourse, Material = webVideo2, Index = 3 },
                new CoursesMaterials { Course = webCourse, Material = webBook1, Index = 2 },
                new CoursesMaterials { Course = webCourse, Material = webVideo3, Index = 4 },
                new CoursesMaterials { Course = webCourse, Material = webArticle2, Index = 6 },
                new CoursesMaterials { Course = webCourse, Material = webBook2, Index = 7 },
            };

            webCourse.CoursesMaterials = webMaterials;
            context.Courses.Add(webCourse);
            context.SaveChanges();

            // -------------------------

            var jsSkills = new List<Skill>
            {
                js, asyncJS, nodeJS
            };

            var jsCourse = new Course
            {
                Name = "The Complete JavaScript Course 2022: From Zero to Expert!",
                Thumbnail = "https://educationalportal.blob.core.windows.net/thumbnails/js.jpg",
                ShortDescription = "The modern JavaScript course for everyone! Master JavaScript " +
                                   "with projects, challenges and theory. Many courses in one!",
                Description = "You will learn modern JavaScript from the very beginning, step-by-step. " +
                                "I will guide you through practical and fun code examples, important theory " +
                                "about how JavaScript works behind the scenes, and beautiful and complete " +
                                "projects. You will also learn how to think like a developer, how to plan " +
                                "application features, how to architect your code, how to debug code, and a " +
                                "lot of other real-world skills that you will need on your developer job. " +
                                "And unlike other courses, this one actually contains beginner, intermediate, " +
                                "advanced, and even expert topics, so you don't have to buy any other course " +
                                "in order to master JavaScript from the ground up!",
                Price = 110,
                Skills = jsSkills,
                Author = user,
            };

            var jsMaterials = new List<CoursesMaterials>
            {
                new CoursesMaterials { Course = jsCourse, Material = webBook1, Index = 2 },
                new CoursesMaterials { Course = jsCourse, Material = webArticle2, Index = 1 },
            };

            jsCourse.CoursesMaterials = jsMaterials;

            context.Courses.Add(jsCourse);
            context.SaveChanges();
        }
    }
}
