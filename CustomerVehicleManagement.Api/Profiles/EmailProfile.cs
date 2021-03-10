using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Email, EmailCreateDto>();
            CreateMap<EmailCreateDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore());
            CreateMap<Email, EmailReadDto>();
            CreateMap<EmailReadDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore());
            CreateMap<Email, EmailUpdateDto>();
            CreateMap<EmailUpdateDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore());
        }
    }
}
