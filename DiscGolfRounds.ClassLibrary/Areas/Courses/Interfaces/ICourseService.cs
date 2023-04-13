using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces
{
    public interface ICourseService
    {
        Task<List<Course>> ViewAllCourses();
        Task<Course> UpdateCourseName(int courseId, string courseName);
        Task<Course> CreateCourseByPar(string courseName, string variantName, List<int> pars);
        Task<CourseVariant> UpdateCourseVariantName(int courseVariantId, string courseVariantName);
        Task<Hole> UpdateHolePar(int holeId, int holePar);
        Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID);
        Task<CourseVariant> DeleteCourseVariant(int variantID);
        Task<Course> DeleteCourse(int courseID);
        Task<Course> UndoDeleteCourse(int courseID);
        Task<CourseVariant> UndoDeleteCourseVariant(int courseID);
        Task<List<CourseVariant>> ViewAllCourseVariants();

    }
}