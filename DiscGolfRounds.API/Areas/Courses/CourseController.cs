using DiscGolfRounds.API.Areas.Courses.Requests;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DiscGolfRounds.ClassLibrary.DataAccess.DTOs;
using DiscGolfRounds.ClassLibrary.DataAccess;

namespace DiscGolfRounds.API.Areas.Courses
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {

        private readonly DiscGolfContext _context;
        private readonly ICourseService _courseService;

        private readonly IMapper _mapper;
        public CourseController(ICourseService courseCreator, IMapper mapper)

        {
            _context = context;
            _courseService = courseCreator;
            _mapper = mapper;
        }

        [HttpPost(nameof(CreateNewCourse))]
        public async Task<CourseDTO> CreateNewCourse(NewCourseRequest request)
        {

           Course course = await _courseService.CreateCourseByPar(request. courseName, request.variantName, request.holePars);


            return _mapper.Map<CourseDTO>(course);
        }
        //System.Text.Json.JsonException: A possible object cycle was detected.
        //This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
        
        [HttpGet (nameof(ViewAllCourses))]
        public async Task<List<CourseDTO>> ViewAllCourses()
        {
            var courses = await _courseService.ViewAllCourses();
            var courseDTOs = courses.Select(c=> _mapper.Map<CourseDTO>(c)).ToList();
            return courseDTOs;
        }
        [HttpGet(nameof(ViewAllCourseVariants))]
        public async Task<List<CourseVariantDTO>> ViewAllCourseVariants()
        {
            var variants = await _courseService.ViewAllCourseVariants();
            return variants.Select(cv => _mapper.Map<CourseVariantDTO>(cv)).ToList();
        }

        [HttpPost(nameof(UpdateCourseName))]
        public async Task<CourseDTO> UpdateCourseName(int courseId, string courseName)
        {
            var course = await _courseService.UpdateCourseName(courseId, courseName);
            return _mapper.Map<CourseDTO>(course);
        }
        [HttpPost(nameof(CourseVariantNameUpdater))]
        public async Task<CourseVariantDTO> CourseVariantNameUpdater(int courseVariantId, string courseVariantName)
        {
            var variant = await _courseService.UpdateCourseVariantName(courseVariantId, courseVariantName);
            return _mapper.Map<CourseVariantDTO>(variant);
        }
        [HttpPost(nameof(HoleParUpdater))]
        public async Task<HoleDTO> HoleParUpdater(int holeID, int holePar)
        {
            var hole = await _courseService.UpdateHolePar(holeID, holePar);
            return _mapper.Map<HoleDTO>(hole);
        }
        [HttpPost(nameof(CourseDeleter))]
        public async Task<CourseDTO> CourseDeleter(int courseID)
        {
            var course = await _courseService.DeleteCourse(courseID);
            return _mapper.Map<CourseDTO>(course);
        }
        [HttpGet(nameof(UndoCourseDeleter))]
        public async Task<CourseDTO> UndoCourseDeleter(int courseID)
        {
            var course = await _courseService.UndoDeleteCourse(courseID);
            return _mapper.Map<CourseDTO>(course);
        }
        [HttpGet(nameof(UndoCourseVariantDeleter))]
        public async Task<CourseVariantDTO> UndoCourseVariantDeleter(int courseID)
        {
            var variant = await _courseService.UndoDeleteCourseVariant(courseID);
            return _mapper.Map<CourseVariantDTO>(variant);
        }

    }
}
