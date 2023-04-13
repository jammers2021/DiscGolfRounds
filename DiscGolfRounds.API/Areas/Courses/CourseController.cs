using DiscGolfRounds.API.Areas.Courses.Requests;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiscGolfRounds.ClassLibrary.DataAccess;

namespace DiscGolfRounds.API.Areas.Courses
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseCreator)
        {
            _courseService = courseCreator;
        }

        [HttpPost(nameof(CreateNewCourse))]
        public async Task<Course> CreateNewCourse(NewCourseRequest request)
        {
            CourseVariant courseVariant = await _courseService.CourseVariantCreatorByPar(request. courseName, request.variantName, request.holePars);
            var existCheck = _context.Courses.FirstOrDefault(c => c.Name == courseVariant.Course.Name);
            if (existCheck != null)
                return null;
            Course course = new();
            course = _context.Courses.FirstOrDefault(c => c.Name == courseVariant.Course.Name);

            return course;
        }
        
        [HttpGet (nameof(ViewAllCourses))]
        public async Task<List<Course>> ViewAllCourses()
        {
            var courses = await _courseService.AllCourseViewer();
            return courses;
        }
        
        [HttpPost(nameof(UpdateCourseName))]
        public async Task<Course> UpdateCourseName(int courseId, string courseName)
        {
            return await _courseService.CourseNameUpdater(courseId, courseName);
        }
        [HttpPost(nameof(CourseVariantNameUpdater))]
        public async Task<CourseVariant> CourseVariantNameUpdater(int courseVariantId, string courseVariantName)
        {
            return await _courseService.CourseVariantNameUpdater(courseVariantId, courseVariantName);
        }
        [HttpPost(nameof(HoleParUpdater))]
        public async Task<Hole> HoleParUpdater(int holeID, int holePar)
        {
            return await _courseService.HoleParUpdater(holeID, holePar);
        }
        [HttpPost(nameof(CourseDeleter))]
        public async Task<Course> CourseDeleter(int courseID)
        {
            return await _courseService.DeleteCourse(courseID);
        }
        [HttpGet(nameof(UndoCourseDeleter))]
        public async Task<Course> UndoCourseDeleter(int courseID)
        {
            return await _courseService.UndoCourseDeleter(courseID);
        }
        [HttpGet(nameof(UndoCourseVariantDeleter))]
        public async Task<CourseVariant> UndoCourseVariantDeleter(int courseID)
        {
            return await _courseService.UndoCourseVariantDeleter(courseID);
        }

    }
}
