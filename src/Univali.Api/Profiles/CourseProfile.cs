using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Publishers.Commands.CreateCourse;
using Univali.Api.Features.Publishers.Commands.UpdateCourse;
using Univali.Api.Features.Publishers.Queries.GetCourseById;
using Univali.Api.Features.Publishers.Queries.GetCourseByIdWithAuthors;
using Univali.Api.Models;

namespace Univali.Api.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseCommand, Course>();

            CreateMap<Course, CreateCourseDto>();

            CreateMap<Course, GetCourseByIdDto>();

            CreateMap<UpdateCourseCommand, Course>();

            CreateMap<Course, GetCourseByIdWithAuthorsDto>();
            
            CreateMap<Author, AuthorDto>().ReverseMap();
        }
    }
}