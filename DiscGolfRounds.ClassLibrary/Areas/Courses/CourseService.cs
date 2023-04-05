using DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Course> CourseVariantCreatorByPar(string courseName, string variantName, List<int> pars)
        {
            Course course;

            var existingCourse = _dbContext.Courses.FirstOrDefault(c => c.Name == courseName);
            if (existingCourse != null)
            {
                course = existingCourse;
            }
            else
            {
                course = new Course
                {
                    Name = courseName,
                    Deleted = false,
                };
                await _dbContext.Courses.AddAsync(course);
            }

            var variant = new CourseVariant
            {
                Name = variantName,
                Course = course,
                Deleted = false
            };

            await _dbContext.CourseVariants.AddAsync(variant);
            

            //variant.Holes
            /*
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
            */

            List<Hole> holes = new();
            for (int i = 1; i <= pars.Count; i++)
            {
                Hole hole = new();
                hole.CourseVariant = variant;
                hole.Number = i;
                hole.Par = pars[i-1];
                holes.Add(hole);
                hole.Deleted= false;
            };
            /*
            var holesList = pars.Select((value, index) => new Hole
            {
                Course = course,
                CourseID = course.Id,
                CourseVariantID = variant.Id,
                Number = index + 1,
                Par = value,
                Deleted= false,
                CourseVariant = variant,

            }).ToList();
            */
            

            //holes.AddRange(holesList);
            //variant.Holes = holesList;

            //variant.Holes = holes;
            //variant.Course = course;
            //variant.Deleted = false;
            await _dbContext.Holes.AddRangeAsync(holes);

            await _dbContext.SaveChangesAsync();

            //var newCourse = await _dbContext.Courses.FirstAsync(c=> c.Name == courseName);
            //variant.CourseId = newCourse.Id;
            //foreach (var hole in holes)
            //{
            //    hole.CourseID = newCourse.Id;
            //}

            //_dbContext.UpdateRange(holes);
            //_dbContext.Update(variant);
            //await _dbContext.SaveChangesAsync();
            return course;
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
            var courses = await _dbContext.Courses
                .Include(c => c.Variants)
                .Where(c=> c.Deleted != true).ToListAsync();

            //var courseVariants = await _dbContext.CourseVariants.Where(cv=>cv.Deleted != true).ToListAsync();
            //for (int i = 0; i < courses.Count; i++)
            //{
            //    var course = courses[i];
            //    course.VariantIds = new();
            //    foreach (var variant in courseVariants)
            //    {
            //        if (variant.CourseId == course.Id)
            //            course.VariantIds.Add(variant.Id);
            //    }
            //}
            return courses; 
        }
        public async Task<List<Hole>> ViewAllHolesAtCourse(int courseID)
        {
            //var holes = await _dbContext.Holes.Where(h => h.CourseID == courseID).ToListAsync();
            //var courseVariant = await _dbContext.CourseVariants.Where(cv => cv.CourseId == courseID).ToListAsync();
            //var course = await _dbContext.Courses.FirstAsync(c => c.Id == courseID);
            //List<Hole> holeList = new();
            //foreach (var hole in holes)
            //{
            //    hole.CourseVariant = courseVariant.FirstOrDefault(cv => cv.Id == hole.CourseVariantID);
            //    holeList.Add(hole);
            //}
            //return holeList;
            throw new NotImplementedException();
        }
        public async Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID)
        {
            //var holes = await _dbContext.Holes.Where(h => h.CourseVariantID == courseVariantID).ToListAsync();
            //var courseVariant = await _dbContext.CourseVariants.FirstAsync(cv => cv.Id == courseVariantID);
            //var course = await _dbContext.Courses.FirstAsync(c => c.Id == courseVariant.CourseId);
            List<Hole> holeList = new();
            //foreach (var hole in holes)
            //{
            //    hole.Course = course;
            //    hole.CourseVariant = courseVariant;
            //    holeList.Add(hole);
            //}
            return holeList;
        }
        //Methods below need to be tested
        public async Task<Course> DeleteCourse(int courseID)
        {
            var course = await _dbContext.Courses.FindAsync(courseID);
           // if (course == null) 
           //     return course;
           // course.Deleted = true;
           //var variants = await _dbContext.CourseVariants.Where(cv=> cv.CourseId == courseID).ToListAsync();
           // foreach (var variant in variants)
           // {
           //     variant.Deleted = true;
           // }
           // var holes = await _dbContext.Holes.Where(h=> h.CourseID == courseID).ToListAsync(); 
           // foreach (var hole in holes)
           // {
           //     hole.Deleted = true;
           // }
           // var rounds = await _dbContext.Rounds.Where(r=> r.CourseId== courseID).ToListAsync();
           // List<int> roundIds = new();
           // foreach (var round in rounds)
           // {
           //     round.Deleted = true;
           //     roundIds.Add(round.Id);
           // }
           // var scores = await _dbContext.Scores.Where(s=> roundIds.Contains(s.RoundID)).ToListAsync();
           // foreach (var score in scores)
           // {
           //     score.Deleted = true;
           // }
           // await _dbContext.SaveChangesAsync();
            return course;
        }
        public async Task<CourseVariant> DeleteCourseVariant(int variantID)
        {
            var variant = await _dbContext.CourseVariants.FindAsync(variantID);
            if (variant == null)
                return variant;
            variant.Deleted = true;
            
            var holes = await _dbContext.Holes.Where(h => h.CourseVariantID == variantID).ToListAsync();
            foreach (var hole in holes)
            {
                hole.Deleted = true;
            }
            var rounds = await _dbContext.Rounds.Where(r => r.CourseVariantID == variantID).ToListAsync();
            List<int> roundIds = new();
            foreach (var round in rounds)
            {
                round.Deleted = true;
                roundIds.Add(round.Id);
            }
            var scores = await _dbContext.Scores.Where(s => roundIds.Contains(s.RoundID)).ToListAsync();
            foreach (var score in scores)
            {
                score.Deleted = true;
            }
            await _dbContext.SaveChangesAsync();
            return variant;
        }
        public async Task<Course> UndoCourseDeleter(int courseID)
        {
            var course = await _dbContext.Courses.FindAsync(courseID);
            course.Deleted = false;
            _dbContext.Update(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }
        public async Task<CourseVariant> UndoCourseVariantDeleter(int courseID)
        {
            var variant = await _dbContext.CourseVariants.FindAsync(courseID);
            variant.Deleted = false;
            _dbContext.Update(variant);
            await _dbContext.SaveChangesAsync();
            return variant;
        }

    }
}
