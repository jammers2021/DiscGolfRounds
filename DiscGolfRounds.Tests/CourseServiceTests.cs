using DiscGolfRounds.ClassLibrary.Areas.Courses;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace DiscGolfRounds.Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        /*
        public readonly ICourseService _courseService;
        public CourseServiceTests(ICourseService courseService)
        {
            _courseService= courseService;
        }
        */
        [TestMethod]
        public async Task CreateCourse_WhenNameDoesNotExistInDatabase_AddsCourseToDatabase()
        {

            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                .UseInMemoryDatabase(databaseName: "golfDatabase")
                .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "MyTestCourse";
            await courseService.CreateCourse(courseName);
            var courseCheck = await context.Courses.FirstOrDefaultAsync(c => c.Name == courseName);
            Assert.IsNotNull(courseCheck);

        }

        [TestMethod]
        public async Task CreateCourse_WhenNameAlreadyExists_CourseNotAddedToDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "MyTestCourse";
            await courseService.CreateCourse(courseName);
            await courseService.CreateCourse(courseName);
            var courseCheck = await context.Courses.SingleOrDefaultAsync(c => c.Name == courseName);
            Assert.IsNotNull(courseCheck);

        }
        [TestMethod]
        public async Task CreateCourseVariant_WhenValidCourseProvided_CourseVariantAddedToDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                .UseInMemoryDatabase(databaseName: "golfDatabase")
                .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Testing 1,2,3",
                Deleted = false,
                Variants = null
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            course = await context.Courses.FirstOrDefaultAsync(c => c.Name == "Testing 1,2,3");
            string courseVariantName = "MyTestCourseVariant";
            await courseService.CreateCourseVariant(courseVariantName, course);
            var courseVariantCheck = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == courseVariantName);
            Assert.IsNotNull(courseVariantCheck);
        }
        [TestMethod]
        public async Task CreateCourseVariant_WhenInvalidCourseProvided_CourseVariantNotAddedToDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                .UseInMemoryDatabase(databaseName: "golfDatabase")
                .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Testing 1,2,3",
                Deleted = false,
                Variants = null
            };
            string courseVariantName = "MyTestCourseVariant";
            await courseService.CreateCourseVariant(courseVariantName, course);
            var courseVariantCheck = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == courseVariantName);
            Assert.IsNull(courseVariantCheck);
        }
        [TestMethod]
        public async Task CreateCourseVariant_WhenCourseVariantNameAlreadyExists_CourseVariantNotAlsoAddedToDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                .UseInMemoryDatabase(databaseName: "golfDatabase")
                .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Testing 1,2,3,4",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "TestCourseVariant",
                Course = course,
                Deleted = false,
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            await context.SaveChangesAsync();
            course = await context.Courses.FirstOrDefaultAsync(c => c.Name == "Testing 1,2,3,4");
            string courseVariantName = "TestCourseVariant";
            await courseService.CreateCourseVariant(courseVariantName, course);
            var courseVariantCheck = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == courseVariantName);
            Assert.IsNotNull(courseVariantCheck);
            //Exception not thrown for multiple of the same courseVariantName
        }
        [TestMethod]
        public async Task CreateHolesInVariant_WhenNoListOfHolesAlreadyPresent_ListOfHolesAdded()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Holes Present Tester",
                Deleted = false,
            };
            CourseVariant courseVariant = new()
            {
                Name = "Holes Present Tester",
                Deleted = false,
                Course = course,
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(courseVariant);
            await context.SaveChangesAsync();
            List<int> pars = new() { 3, 3, 3, 4, 5 };
            courseService.CreateHolesInVariant(pars, courseVariant);
            var holesCheck = await context.Holes.CountAsync(h => h.CourseVariant.Name == courseVariant.Name);
            Assert.IsNotNull(holesCheck);
            Assert.IsTrue(holesCheck == pars.Count);
        }
        [TestMethod]
        public async Task CreateHolesInVariant_WhenListOfHolesAlreadyPresent_ListOfHolesNotAdded()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            Hole hole1 = new()
            {
                Par = 3,
                Number = 1,
                Deleted = false,
            };
            Hole hole2 = new()
            {
                Par = 4,
                Number = 2,
                Deleted = false,
            };
            List<Hole> holes = new()
            {
                hole1,
                hole2
            };
            CourseService courseService = new(context); Course course = new()
            {
                Name = "Holes Not Present Tester",
                Deleted = false,
            };
            CourseVariant courseVariant = new()
            {
                Name = "Holes Not Present Tester",
                Deleted = false,
                Course = course,
                Holes = holes,
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(courseVariant);
            context.Holes.AddRange(holes);
            await context.SaveChangesAsync();
            courseVariant = await context.CourseVariants.SingleOrDefaultAsync(cv => cv.Name == "Holes Not Present Tester");
            List<int> pars = new() { 3, 3, 3, 4, 5 };
            await courseService.CreateHolesInVariant(pars, courseVariant);
            var holesCheck = await context.Holes.CountAsync(h => h.CourseVariant.Name == courseVariant.Name);
            Assert.IsNotNull(holesCheck);
            Assert.IsFalse(holesCheck == pars.Count);
            Assert.IsFalse(holesCheck == pars.Count + holes.Count);
            Assert.IsTrue(holesCheck == holes.Count);
        }
        [TestMethod]
        public async Task UpdateCourseName_WhenNameIsAlreadyInDatabase_NameDoesNotUpdate()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "this Course";
            var courseCreation = await courseService.CreateCourse(courseName);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Name == courseName);
            int courseId = course.Id;
            string newCourseName = "MyNewTestCourse";
            var firstCheck = await courseService.UpdateCourseName(courseId, newCourseName);
            Assert.IsNotNull(firstCheck);
            var nullCheck = await courseService.UpdateCourseName(courseId, newCourseName);
            Assert.IsNull(nullCheck);
            var noCourse = await context.Courses.SingleOrDefaultAsync(c => c.Name == "this Course");
            Assert.IsTrue(noCourse == null);
        }
        [TestMethod]
        public async Task UpdateCourseName_WhenNameIsNotInDatabaseAndGivnValidID_UpdatesName()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "this other Course";
            var courseCreation = await courseService.CreateCourse(courseName);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Name == courseName);

            int courseId = course.Id;
            string newCourseName = "MyNewestTestCourse";
            await courseService.UpdateCourseName(courseId, newCourseName);
            var newCourse = await context.Courses.FirstOrDefaultAsync(c => c.Name.Equals(newCourseName));
            var noCourse = await context.Courses.FirstOrDefaultAsync(c => c.Name == "this other Course");
            Assert.IsTrue(noCourse == null);
            Assert.IsNotNull(newCourse);
        }
        [TestMethod]
        public async Task UpdateCourseName_WhenNameIsNotInDatabaseButGivenInvalidID_ExceptionThrown()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "this other Course";
            var courseCreation = await courseService.CreateCourse(courseName);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Name == courseName);

            int courseId = course.Id;
            string newCourseName = "NewestTestCourse";


            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await courseService.UpdateCourseName(37, newCourseName));
        }
        [TestMethod]
        public async Task UpdateHolePar_WhenHoleIdIsValid_UpdateHolePar()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                           .UseInMemoryDatabase(databaseName: "golfDatabase")
                           .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Death Star",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "The first destroyed one",
                Deleted = false,
                Course = course,
            };

            Hole hole = new()
            {
                Par = 4,
                CourseVariant = variant,
                Deleted = false,
                Number = 1,
                Name = "Impossible Missile Shot",
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            context.Holes.Add(hole);
            await context.SaveChangesAsync();
            var holeTest = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole.Name);
            int updatedPar = 5;
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await courseService.UpdateHolePar(97, updatedPar));
        }

        [TestMethod]
        public async Task UpdateHolePar_WhenHoleIdIsInvalid_ExceptionThrown()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                           .UseInMemoryDatabase(databaseName: "golfDatabase")
                           .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Death Star",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "The first destroyed one",
                Deleted = false,
                Course = course,
            };

            Hole hole = new()
            {
                Par = 4,
                CourseVariant = variant,
                Deleted = false,
                Number = 1,
                Name = "Impossible Missile Shot",
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            context.Holes.Add(hole);
            await context.SaveChangesAsync();
            var holeTest = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole.Name);
            int updatedPar = 5;
            await courseService.UpdateHolePar(holeTest.Id, updatedPar);
            var holeVerified = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole.Name);
            Assert.AreEqual(updatedPar, holeVerified.Par);
        }

        [TestMethod]
        public async Task UpdateCourseVariantName_WhenGivenValidID_UpdatesName()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);


            string courseName = "Course Testing 1,2,3";
            var course = await courseService.CreateCourse(courseName);
            string courseVariantName = "this Course Variant";
            var courseVariantCreation = await courseService.CreateCourseVariant(courseVariantName, course);
            var courseVariant = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == courseVariantName);
            int courseVariantId = courseVariant.Id;
            string newCourseVariantName = "MyNewerTestCourseVariant";
            var checkThis = await courseService.UpdateCourseVariantName(courseVariantId, newCourseVariantName);
            var noCourseVariant = await context.CourseVariants.SingleOrDefaultAsync(cv => cv.Name == "this Course Variant");
            var newCourseVariant = await context.CourseVariants.SingleOrDefaultAsync(cv => cv.Name == "MyNewerTestCourseVariant");
            Assert.IsNull(noCourseVariant);
            Assert.IsNotNull(newCourseVariant);
        }

        [TestMethod]
        public async Task DeleteCourse_WhenIdIsValid_ChangeDeletedToTrue()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            Course course = new()
            {
                Name = "USS Enterprise",
                Deleted = false,
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            var courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);
            await courseService.DeleteCourse(courseSelected.Id);
            courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);

            Assert.IsTrue(courseSelected.Deleted);
        }
        [TestMethod]
        public async Task DeleteCourse_WhenIdIsInvalid_NullReturned()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            var nullCheck = await courseService.DeleteCourse(42);
            Assert.IsNull(nullCheck);
        }
        [TestMethod]
        public async Task UndoDeleteCourse_WhenIdIsValid_ChangeDeletedToFalse()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            Course course = new()
            {
                Name = "Star Destroyer",
                Deleted = true,
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            var courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == "Star Destroyer");
            await courseService.UndoDeleteCourse(courseSelected.Id);
            courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == "Star Destroyer");
            Assert.IsFalse(courseSelected.Deleted);

        }
        [TestMethod]
        public async Task UndoDeleteCourse_WhenIdIsInvalid_ThrowsNullReferenceException()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                            .UseInMemoryDatabase(databaseName: "golfDatabase")
                            .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);


            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await courseService.UndoDeleteCourse(99));

        }
        [TestMethod]
        public async Task DeleteCourse_WhenIdIsValidAndVariantsExist_DeletesCourseVariants()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            Course course = new()
            {
                Name = "Imperial Fleet",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "Tie Interceptor",
                Deleted = false,
                Course = course,
            };
            CourseVariant variant2 = new()
            {
                Name = "Tie Fighter",
                Deleted = false,
                Course = course
            };

            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            context.CourseVariants.Add(variant2);
            await context.SaveChangesAsync();
            var courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);
            await courseService.DeleteCourse(courseSelected.Id);
            var courseVariantSelected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == "Tie Interceptor");
            var secondVariantSelected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == "Tie Fighter");

            Assert.IsTrue(courseVariantSelected.Deleted);
            Assert.IsTrue(secondVariantSelected.Deleted);
        }
        public async Task DeleteCourse_WhenIdIsValidAndVariantsExist_DeletesHoles()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            Course course = new()
            {
                Name = "Firefly",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "Serenity",
                Deleted = false,
                Course = course,
            };
            Hole hole1 = new()
            {
                Par = 3,
                Deleted = false,
                Number = 1,
                CourseVariant = variant,
                Name = "Mal",
            };
            Hole hole2 = new()
            {
                Par = 4,
                Deleted = false,
                Number = 2,
                CourseVariant = variant,
                Name = "Inara"
            };
            List<Hole> holes = new()
            {
                hole1,
                hole2,
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            context.Holes.AddRange(holes);
            await context.SaveChangesAsync();
            var courseSelection = await context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);
            courseService.DeleteCourse(courseSelection.Id);
            var hole1Selected = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole1.Name);
            var hole2Selected = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole2.Name);
            Assert.IsTrue(hole1Selected.Deleted);
            Assert.IsTrue(hole2Selected.Deleted);
        }
        [TestMethod]
        public async Task DeleteCourseVariant_WhenIdIsInvalid_ReturnNull()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            var nullCheck = await courseService.DeleteCourseVariant(77);
            Assert.IsNull(nullCheck);
        }
        [TestMethod]
        public async Task DeleteCourseVariant_WhenIdIsValid_DeleteSelectedCourseVariantLeavingCourseAndOtherVariant()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            Course course = new()
            {
                Name = "D&D",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "Cleric",
                Deleted = false,
                Course = course,
            };
            CourseVariant variant1 = new()
            {
                Name = "Barbarian",
                Deleted = false,
                Course = course,
            };
            context.CourseVariants.Add(variant);
            context.CourseVariants.Add(variant1);
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            var variant1Selected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant1.Name);
            var variantSelected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant.Name);
            var courseVariant = await courseService.DeleteCourseVariant(variant1.Id);
            variant1Selected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant1.Name);
            variantSelected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant.Name);
            var courseSelected = await context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);

            Assert.IsTrue(variant1Selected.Deleted);
            Assert.IsFalse(variantSelected.Deleted);
            Assert.IsFalse(courseSelected.Deleted);
        }
        [TestMethod]
        public async Task DeleteCourseVariant_WhenIdIsValid_DeletesHolesFromVariantButLeavesOthers()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);

            Course course = new()
            {
                Name = "D&D Classes",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name = "Fighter",
                Deleted = false,
                Course = course,
            };
            Hole hole = new()
            {
                Number = 1,
                Par = 3,
                CourseVariant = variant,
                Name = "Eldritch Knight",
                Deleted = false,
            };
            Hole hole1 = new()
            {
                Number = 2,
                Par = 4,
                CourseVariant = variant,
                Name = "Battle Master",
                Deleted = false,
            };
            List<Hole> holes = new()
            {
                hole,
                hole1,
            };
            CourseVariant variant1 = new()
            {
                Name = "Monk",
                Deleted = false,
                Course = course,
            };
            Hole hole2 = new()
            {
                Number = 1,
                Par = 3,
                CourseVariant = variant1,
                Name = "Way of Open Hand",
                Deleted = false,
            };
            Hole hole3 = new()
            {
                Number = 2,
                Par = 4,
                CourseVariant = variant1,
                Name = "Way of Mercy",
                Deleted = false,
            };
            List<Hole> holesDeleted = new()
            {
                hole2,
                hole3,
            };
            context.CourseVariants.Add(variant);
            context.CourseVariants.Add(variant1);
            context.Holes.AddRange(holes);
            context.Holes.AddRange(holesDeleted);
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            var variant1Selected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant1.Name);
            var variantSelected = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variant.Name);
            var courseVariant = await courseService.DeleteCourseVariant(variant1.Id);
            var holeSelected = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole.Name);
            var hole1Selected = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole1.Name);
            var hole2Selected = await context.Holes.FirstOrDefaultAsync(h=> h.Name == hole2.Name);
            var hole3Selected = await context.Holes.FirstOrDefaultAsync(h => h.Name == hole3.Name);


            Assert.IsTrue(hole2Selected.Deleted);
            Assert.IsTrue(hole3Selected.Deleted);
            Assert.IsFalse(holeSelected.Deleted);
            Assert.IsFalse(hole1Selected.Deleted);
        }
        // Possible Tests to Add Undo Course Delete (And Course Variant Delete) leaves cascade deleted

        [TestMethod]
        public async Task CreateCourseByPar_SavesInformationFromCourseCourseVariantAndHoles()
        {

            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                .UseInMemoryDatabase(databaseName: "golfDatabase")
                .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            CourseService courseService = new(context);
            string courseName = "Course Named";
            string variantName = "Variant Named";
            List<int> pars = new() { 3, 3, 3, 4, 5 };
            await courseService.CreateCourseByPar(courseName, variantName, pars);
            var courseCheck = await context.Courses.FirstOrDefaultAsync(c => c.Name == courseName);
            var variantCheck = await context.CourseVariants.FirstOrDefaultAsync(cv => cv.Name == variantName);
            var parCheck = await context.Holes.CountAsync(h => h.CourseVariant.Name == variantName);
            Assert.IsNotNull(courseCheck);
            Assert.IsNotNull(variantCheck);
            Assert.IsTrue(parCheck == 5);
        }
    }
}