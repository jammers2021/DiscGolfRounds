﻿using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces
{
    public interface ICourseService
    {
        Task<List<Course>> AllCourseViewer();
        Task<Course> CourseNameUpdater(int courseId, string courseName);
        Task<CourseVariant> CourseVariantCreatorByPar(string courseName, string variantName, List<int> pars);
        Task<CourseVariant> CourseVariantNameUpdater(int courseVariantId, string courseVariantName);
        Task<Hole> HoleParUpdater(int holeId, int holePar);
        Task<List<Hole>> ViewAllHolesAtCourse(int courseID);
        Task<List<Hole>> ViewAllHolesAtCourseVariant(int courseVariantID);
    }
}