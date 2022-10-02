using BenchmarkDotNet.Attributes;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Infrastructure.EF;
using EducationalPortal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMarking
{
    [MemoryDiagnoser(false)]
    public class Benchmarking
    {
        public ApplicationContext _db;

        public CoursesRepository _coursesRepository;

        private readonly Mapper _mapper = new();

        [GlobalSetup]
        public void Setup()
        {
            _db = new();
            _coursesRepository = new(_db);
        }

        [Benchmark]
        public async Task GetFullCourseAsyncNew()
        {
            var random = new Random();
            var id = random.Next(1, 4);

            var course = await this._coursesRepository.GetFullCourseAsync(id, "1234567890", CancellationToken.None);
            if (course != null)
            {
                var dto = this._mapper.Map(course);
            }
        }

        [Benchmark]
        public async Task GetFullCourseAsyncOld()
        {
            var random = new Random();
            var id = random.Next(1, 4);

            var course = await this._db.Courses
               .Include(c => c.Author)
               .Include(c => c.CoursesMaterials)
                  .ThenInclude(cm => cm.Material)
               .Include(c => c.CoursesSkills)
                  .ThenInclude(cs => cs.Skill)
               .FirstOrDefaultAsync(c => c.Id == id, CancellationToken.None);

            if (course != null)
            {
                var materials = new List<MaterialsBase>();
                foreach (var cm in course.CoursesMaterials.OrderBy(cm => cm.Index))
                {
                    var book = await this._db.Books
                        .Include(b => b.Authors)
                        .Include(b => b.Extension)
                        .FirstOrDefaultAsync(b => b.Id == cm.MaterialId, CancellationToken.None);

                    var video = await this._db.Videos
                        .Include(v => v.Quality)
                        .FirstOrDefaultAsync(v => v.Id == cm.MaterialId, CancellationToken.None);

                    var article = await this._db.Articles
                        .Include(a => a.Resource)
                        .FirstOrDefaultAsync(a => a.Id == cm.MaterialId, CancellationToken.None);

                    course.CoursesMaterials[cm.Index - 1].Material = book ?? video ?? article ?? new MaterialsBase();
                }

                var user = await this._db.Users
                           .Include(u => u.Materials)
                           .FirstOrDefaultAsync(u => u.Id == "1234567890", CancellationToken.None);

                var courseViewModel = this._mapper.Map(course, user?.Materials);
            }
            

        }
    }
}
