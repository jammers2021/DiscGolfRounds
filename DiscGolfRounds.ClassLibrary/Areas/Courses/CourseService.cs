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
        public async Task<Course> CreateCourse(string courseName)
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
                _dbContext.SaveChanges();
                course = _dbContext.Courses.First(c=> c.Name == courseName);
            }
            return course;
        }
        public async Task<CourseVariant> CreateCourseVariant(string variantName, Course course)
        {
            CourseVariant variant;
            var courseCheck = await _dbContext.Courses.FindAsync(course.Id);
            if (courseCheck == null)
            {
                return null;
            }
            var variantCheck =  _dbContext.CourseVariants.FirstOrDefault(cv=> cv.Name == variantName && cv.CourseId == course.Id);
            if(variantCheck != null)
            {
                variant = variantCheck;
            }
            else
            {
                variant = new CourseVariant
                {
                    Name = variantName,
                    Course = course,
                    Deleted = false
                };
                await _dbContext.CourseVariants.AddAsync(variant);
                _dbContext.SaveChanges();
            }
            return variant;
        }
        public async Task<List<Hole>> CreateHolesInVariant(List<int> pars, CourseVariant variant)
        {
            var holesExist = _dbContext.Holes.Any(h => h.CourseVariantID == variant.Id);
            if (holesExist)
            {
                var holes = await _dbContext.Holes.Where(h => h.CourseVariantID == variant.Id).ToListAsync();
                return holes;
            }
            else
            {
                List<Hole> holes = new();
                for (int i = 1; i <= pars.Count; i++)
                {
                    Hole hole = new()
                    {
                        CourseVariant = variant,
                        Number = i,
                        Par = pars[i - 1]
                    };
                    holes.Add(hole);
                    hole.Deleted = false;
                };
                _dbContext.Holes.AddRange(holes);
                await _dbContext.SaveChangesAsync();
                return holes;
            }
            
           /*
            if(holesExist)
            {
                var holes = await _dbContext.Holes.Where(h=> h.CourseVariantID ==variant.Id).ToListAsync();
                return holes;
            }
            else
            {
                var holes = pars.Select((value, index) => new Hole
                {
                    CourseVariantID = variant.Id,
                    Number = index + 1,
                    Par = value,
                    Deleted = false,
                    CourseVariant = variant,

                }).ToList();
                await _dbContext.Holes.AddRangeAsync(holes);
                return holes;
            }
           */
            
        }

        public async Task<Course> CreateCourseByPar(string courseName, string variantName, List<int> pars)
        {
            Course course = await CreateCourse(courseName);

            CourseVariant variant = await CreateCourseVariant(variantName, course);

            List<Hole> holes = await CreateHolesInVariant(pars, variant);
            

            await _dbContext.SaveChangesAsync();

            return course;
        }
        public async Task<Course> UpdateCourseName(int courseId, string courseName)
        {
            var course = await _dbContext.Courses.FindAsync(courseId);
            var nameCheck = await _dbContext.Courses.FirstOrDefaultAsync(c=> c.Name == courseName);
            if (nameCheck != null)
            {
                return null;
            }
            course.Name = courseName;
            await _dbContext.SaveChangesAsync();
            return course;
        }
        public async Task<CourseVariant> UpdateCourseVariantName(int courseVariantId, string courseVariantName)
        {
            var courseVariant = await _dbContext.CourseVariants.FindAsync(courseVariantId);

            courseVariant.Name = courseVariantName;
            _dbContext.CourseVariants.Update(courseVariant);
            await _dbContext.SaveChangesAsync();
            return courseVariant;
        }
        public async Task<Hole> UpdateHolePar(int holeId, int holePar)
        {
            var hole = await _dbContext.Holes.FindAsync(holeId);
            hole.Par = holePar;
            await _dbContext.SaveChangesAsync();
            return hole;
        }

        public async Task<List<Course>> ViewAllCourses()
        {
            var courses = await _dbContext.Courses
                .Where(c=> c.Deleted != true).ToListAsync();

            return courses; 
        }
        public async Task<List<CourseVariant>> ViewAllCourseVariants()
        {
            var variants = await _dbContext.CourseVariants
                .Include(cv=> cv.Course)
                .Where(c => c.Deleted != true).ToListAsync();
            return variants;
        }
        public async Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID)
        {
            var holes = await _dbContext.Holes.Where(h => h.CourseVariantID == courseVariantID).ToListAsync();
            var courseVariant = await _dbContext.CourseVariants.FirstAsync(cv => cv.Id == courseVariantID);
            List<Hole> holeList = new();
            foreach (var hole in holes)
            {
                hole.CourseVariant = courseVariant;
                holeList.Add(hole);
            };
            return holeList;
        }
        
        public async Task<Course> DeleteCourse(int courseID)
        {
            var course = await _dbContext.Courses.FindAsync(courseID);
            if (course == null) 
                return course;
            course.Deleted = true;
            var variants = await _dbContext.CourseVariants.Where(cv=> cv.CourseId == courseID).ToListAsync();
            foreach (var variant in variants)
            {
                variant.Deleted = true;
            }
            var holes = await _dbContext.Holes.Where(h => h.CourseVariant.CourseId == courseID).ToListAsync();
            foreach (var hole in holes)
            {
                hole.Deleted = true;
            }
            var rounds = await _dbContext.Rounds.Where(r => r.CourseId == courseID).ToListAsync();
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
        public async Task<Course> UndoDeleteCourse(int courseID)
        {
            var course = await _dbContext.Courses.FindAsync(courseID);
            course.Deleted = false;
            _dbContext.Update(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }
        public async Task<CourseVariant> UndoDeleteCourseVariant(int courseID)
        {
            var variant = await _dbContext.CourseVariants.FindAsync(courseID);
            variant.Deleted = false;
            _dbContext.Update(variant);
            await _dbContext.SaveChangesAsync();
            return variant;
        }

    }
}
