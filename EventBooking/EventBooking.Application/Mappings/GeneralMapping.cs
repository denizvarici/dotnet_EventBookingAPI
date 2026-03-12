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
            //Event
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>().ReverseMap();

            //Booking
            CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event.Title));
            CreateMap<CreateBookingDto, Booking>();
        }
    }
}
