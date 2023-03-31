using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses
{
    public class CourseService : ICourseService
    {
        private readonly DiscGolfContext _dbContext;

        public CourseService(DiscGolfContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CourseVariant> CourseVariantCreatorByPar(string courseName, string variantName, List<int> pars)
        {
            Course course = new();
            CourseVariant variant = new();

            course.Name = courseName;

            if (_dbContext.Courses.FirstOrDefault(c => c.Name == courseName) != null)
            {
                course = _dbContext.Courses.First(c => c.Name == courseName);
            }
            else
            {
                await _dbContext.Courses.AddAsync(course);
            }
            variant.Name = variantName;
            variant.CourseId = course.Id;

            if (course.Variants == null)
            {
                course.Variants = new List<CourseVariant>();
            }
            else
            {
                course.Variants.Add(variant);
            }
            await _dbContext.CourseVariants.AddAsync(variant);
            course.Variants.Add(variant);

            List<Hole> holes = new();
            /*for (int i = 1; i <= pars.Count; i++)
            {
                Hole hole = new();
                hole.Course = course;
                hole.CourseVariantID = variant.Id;
                hole.CourseVariant = variant;
                hole.Number = i;
                hole.Par = pars[i-1];
                holes.Add(hole);
            };*/

            var holesList = pars.Select((value, index) => new Hole
            {
                Course = course,
                CourseID = course.Id,
                CourseVariant = variant,
                CourseVariantID = variant.Id,
                Number = index + 1,
                Par = value,

            }).ToList();

            holes.AddRange(holesList);

            variant.Holes = holes;
            await _dbContext.Holes.AddRangeAsync(holes);



            await _dbContext.SaveChangesAsync();
            return variant;
        }
        public async Task<Course> CourseNameUpdater(int courseId, string courseName)
        {
            var course = await _dbContext.Courses.FindAsync(courseId);

            course.Name = courseName;
            await _dbContext.SaveChangesAsync();
            return course;
        }
        public async Task<CourseVariant> CourseVariantNameUpdater(int courseVariantId, string courseVariantName)
        {
            var courseVariant = await _dbContext.CourseVariants.FindAsync(courseVariantId);

            courseVariant.Name = courseVariantName;
            await _dbContext.SaveChangesAsync();
            return courseVariant;
        }
        public async Task<Hole> HoleParUpdater(int holeId, int holePar)
        {
            var hole = await _dbContext.Holes.FindAsync(holeId);
            hole.Par = holePar;
            await _dbContext.SaveChangesAsync();
            return hole;
        }

        public async Task<List<Course>> AllCourseViewer()
        {
            return await _dbContext.Courses.Include(c => c.Variants).ToListAsync();
        }
        public async Task<List<Hole>> ViewAllHolesAtCourse(int courseID)
        {
            var holes = await _dbContext.Holes.Where(h => h.CourseID == courseID).ToListAsync();
            var courseVariant = await _dbContext.CourseVariants.Where(cv => cv.CourseId == courseID).ToListAsync();
            var course = await _dbContext.Courses.FirstAsync(c => c.Id == courseID);
            List<Hole> holeList = new();
            foreach (var hole in holes)
            {
                hole.Course = course;
                hole.CourseVariant = courseVariant.FirstOrDefault(cv => cv.Id == hole.CourseVariantID);
                holeList.Add(hole);
            }
            return holeList;
        }
        public async Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID)
        {
            var holes = await _dbContext.Holes.Where(h => h.CourseVariantID == courseVariantID).ToListAsync();
            var courseVariant = await _dbContext.CourseVariants.FirstAsync(cv => cv.Id == courseVariantID);
            var course = await _dbContext.Courses.FirstAsync(c => c.Id == courseVariant.CourseId);
            List<Hole> holeList = new();
            foreach (var hole in holes)
            {
                hole.Course = course;
                hole.CourseVariant = courseVariant;
                holeList.Add(hole);
            }
            return holeList;
        }
    }
}
