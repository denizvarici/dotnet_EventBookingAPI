using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<Guid> CreateEventAsync(CreateEventDto createEventDto)
        {
            await _validationService.ValidateAsync(createEventDto);

            var @event = _mapper.Map<Event>(createEventDto);

            @event.RemainingCapacity = @event.Capacity;

            await _unitOfWork.Repository<Event>().AddAsync(@event);
            await _unitOfWork.SaveChangesAsync();
            return @event.Id;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var @events = await _unitOfWork.Repository<Event>().GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(@events);
        }

        public async Task<EventDto> GetEventByIdAsync(Guid id)
        {
            var @event = await _unitOfWork.Repository<Event>().GetByIdAsync(id);
            return _mapper.Map<EventDto>(@event);
        }
    }
}
