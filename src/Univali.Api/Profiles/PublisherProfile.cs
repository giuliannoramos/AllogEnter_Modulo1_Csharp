using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Publishers.Commands.CreatePublisher;
using Univali.Api.Features.Publishers.Commands.UpdatePublisher;
using Univali.Api.Features.Publishers.Queries.GetPublisherById;
using Univali.Api.Features.Publishers.Queries.GetPublisherByIdWithCourses;
using Univali.Api.Models;

namespace Univali.Api.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<CreatePublisherCommand, Publisher>();

            CreateMap<Publisher, CreatePublisherDto>();

            CreateMap<UpdatePublisherCommand, Publisher>();

            CreateMap<Publisher, GetPublisherByIdDto>();

            CreateMap<Publisher, GetPublisherByIdWithCoursesDto>();

            CreateMap<Course, CourseDto>().ReverseMap();

        }
    }
}