using AutoMapper;
using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using DiscGolfRounds.ClassLibrary.Areas.Rounds.Models;
using DiscGolfRounds.ClassLibrary.DataAccess.DTOs;

namespace DiscGolfRounds.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Course, CourseDTO>();
            CreateMap<CourseDTO, Course>();
            CreateMap<CourseVariantDTO, CourseVariant>();
            CreateMap<CourseVariant, CourseVariantDTO>();
            CreateMap<Hole, HoleDTO>();
            CreateMap<HoleDTO, Hole>();
            CreateMap<Player, PlayerDTO>();
            CreateMap<PlayerDTO, Player>();
            CreateMap<RoundDTO, Round>();
            CreateMap<Round, RoundDTO>();
            CreateMap<ScoreDTO, Score>();
            CreateMap<Score, ScoreDTO>();
        }
    }
}
