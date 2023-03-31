
using DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.DataAccess;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces;
using Microsoft.EntityFrameworkCore;
using DiscGolfRounds.ClassLibrary.Areas.Courses;
using DiscGolfRounds.ClassLibrary.Areas.Players;
using DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Rounds;

namespace DiscGolfRounds.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DiscGolfContext>(options =>
            {
                options.UseSqlite("Filename=./DiscGolfEFScores.sqlite");

            });

            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IRoundService, RoundService>();
            //Consider one service depending on numbers
            //Remove constructors for models--Options class exception, but not relevant



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}