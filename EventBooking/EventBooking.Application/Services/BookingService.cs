using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Exceptions;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EventBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<Guid> CreateBookingAsync(CreateBookingDto createBookingDto,Guid userId)
        {
            //validation
            await _validationService.ValidateAsync(createBookingDto);
            //event checking
            var @event = await _unitOfWork.Repository<Event>().GetByIdAsync(createBookingDto.EventId);
            if (@event == null) throw new KeyNotFoundException("event not found at booking phase");

            //check if already booked
            var alreadyBooked = await _unitOfWork.Repository<Booking>()
                                                 .Where(b => b.EventId.Equals(@event.Id) && b.UserId.Equals(userId))
                                                 .AnyAsync();
            if (alreadyBooked)
            {
                throw new BusinessException("You already have an active reservation for this event.");
            }
            //event capacity control
            if(@event.RemainingCapacity <= 0)
            {
                throw new BusinessException("No remaining capacity left for this event.");
            }

            @event.RemainingCapacity--;
            _unitOfWork.Repository<Event>().Update(@event);

            //make booking
            var booking = _mapper.Map<Booking>(createBookingDto);
            booking.BookingDate = DateTime.Now;
            booking.UserId = userId;

            await _unitOfWork.Repository<Booking>().AddAsync(booking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BusinessException("Başka bir kullanıcı sizden önce bilet aldığı için işlem başarısız oldu. Lütfen tekrar deneyin.");
            }

            return booking.Id;
        }
        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId)
        {
            var bookings = await _unitOfWork.Repository<Booking>()
                                            .Where(b => b.UserId.Equals(userId))
                                            .Include(b => b.Event)
                                            .ToListAsync();

            var bookingDtoList = _mapper.Map<IEnumerable<BookingDto>>(@bookings);

            return bookingDtoList;
        }
        public async Task CancelBookingAsync(Guid bookingId, Guid userId)
        {
            var booking = await _unitOfWork.Repository<Booking>()
                                     .Where(b => b.Id.Equals(bookingId))
                                     .Include(b => b.Event)
                                     .FirstOrDefaultAsync();

            if (booking == null) throw new KeyNotFoundException("Reservation couldn't be found");

            if (booking.UserId != userId) throw new AuthException("You are not authorized to cancel this reservation !");

            if (booking.IsCancelled) throw new BusinessException("This reservation is already cancelled.");

            var @event = booking.Event;
            @event.RemainingCapacity++;
            _unitOfWork.Repository<Event>().Update(@event);

            booking.IsCancelled = true;
            _unitOfWork.Repository<Booking>().Update(booking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                throw new BusinessException("System is too busy for now. Please try again later to cancel your reservation");
            }
        }
    }
}
