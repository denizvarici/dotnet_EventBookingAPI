using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Domain.Entities;

namespace EventBooking.Application.Mappings
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>().ReverseMap();
        }
    }
}
