using DiscGolfRounds.ClassLibrary.Areas.Courses;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.Tests
{
    [TestClass]
    public class RoundServiceTests
    {
        [TestMethod]
        public async Task CreateRoundFromExistingCourseVariant_VariantIdIsInvalid_ReturnsNullReferenceException()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            RoundService roundService = new(context);
            DateTime dateTime= DateTime.Now;
            List<int> scores = new()
            {
                3,2,3,4,4,
            };
            Player player= new()
            {
                PDGANumber= 1,
                HasPDGANumber= true,
                Deleted= false,
                FirstName = "Nathan",
                LastName = "Fillion",
            };
            context.Players.Add(player);
            await context.SaveChangesAsync();
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async()=>await roundService.CreateRoundFromExistingCourseVariant(35, 1, dateTime, scores));
        }
        public async Task CreateRoundFromExistingCourseVariant_PlayerIdIsInvalid_ReturnsNullReferenceException()
        {
            var dbOptions = new DbContextOptionsBuilder<DiscGolfContext>()
                             .UseInMemoryDatabase(databaseName: "golfDatabase")
                             .Options;
            var context = new DiscGolfContext(dbOptions);
            await context.Database.EnsureCreatedAsync();
            RoundService roundService = new(context);
            DateTime dateTime = DateTime.Now;
            List<int> scores = new()
            {
                3,2,3,4,4,
            };
            Course course = new()
            {
                Name = "Marvel",
                Deleted = false,
            };
            CourseVariant variant = new()
            {
                Name= "616",
                Course = course,
                Deleted= false,
            };
            context.Courses.Add(course);
            context.CourseVariants.Add(variant);
            await context.SaveChangesAsync();
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await roundService.CreateRoundFromExistingCourseVariant(1, 77, dateTime, scores));
        }
    }
}

