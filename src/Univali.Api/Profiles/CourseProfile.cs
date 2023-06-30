using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Publishers.Commands.CreateCourse;
using Univali.Api.Features.Publishers.Queries.GetCourseById;
using Univali.Api.Models;

namespace Univali.Api.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseCommand, Course>()
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(source => source.Authors));

            CreateMap<Course, CreateCourseDto>()
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(source => source.Authors));

            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();

            CreateMap<Course, GetCourseByIdDto>();
        }
    }
}