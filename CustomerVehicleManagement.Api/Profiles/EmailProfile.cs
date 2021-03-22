﻿using AutoMapper;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Email, EmailCreateDto>()
                .ReverseMap();
            CreateMap<Email, EmailReadDto>()
                .ReverseMap();
            CreateMap<Email, EmailUpdateDto>()
                .ReverseMap();
            CreateMap<EmailReadDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
            CreateMap<EmailCreateDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
            CreateMap<EmailUpdateDto, Email>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
        }
    }
}
