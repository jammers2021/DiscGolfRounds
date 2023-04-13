using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces
{
    public interface ICourseService
    {
        Task<List<Course>> AllCourseViewer();
        Task<Course> CourseNameUpdater(int courseId, string courseName);
        Task<Course> CourseVariantCreatorByPar(string courseName, string variantName, List<int> pars);
        Task<CourseVariant> CourseVariantNameUpdater(int courseVariantId, string courseVariantName);
        Task<Hole> HoleParUpdater(int holeId, int holePar);
        Task<List<Hole>> ViewAllHolesAtCourse(int courseID);
        Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID);
        Task<CourseVariant> DeleteCourseVariant(int variantID);
        Task<Course> DeleteCourse(int courseID);
        Task<Course> UndoCourseDeleter(int courseID);
        Task<CourseVariant> UndoCourseVariantDeleter(int courseID);

    }
}