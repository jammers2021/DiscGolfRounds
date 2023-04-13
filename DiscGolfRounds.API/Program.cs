
using DiscGolfRounds.ClassLibrary.Areas.Courses;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Players;
using DiscGolfRounds.ClassLibrary.Areas.Players.Interfaces;
using DiscGolfRounds.ClassLibrary.Areas.Rounds;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Interfaces;
using DiscGolfRounds.ClassLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DiscGolfRounds.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DiscGolfContext>(options =>
            {
                options.UseSqlite(builder.Configuration["ConnectionStrings:Sqlite"]);
                //options.UseSqlite("FileName=D:/Program Files/DiscGolfDataBase/DiscGolfEFScores.sqlite");
            });
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IRoundService, RoundService>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<DiscGolfContext>().Database.EnsureCreatedAsync();
            }

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