using AutoMapper;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Guid> CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            await _validationService.ValidateAsync(createBookingDto);
            var booking = _mapper.Map<Booking>(createBookingDto);
            booking.BookingDate = DateTime.Now;

            await _unitOfWork.Repository<Booking>().AddAsync(booking);

            await _unitOfWork.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId)
        {
            var @bookings = await _unitOfWork.Repository<Booking>().Where(b => b.UserId.Equals(userId)).ToListAsync();

            var bookingDtoList = _mapper.Map<IEnumerable<BookingDto>>(@bookings);

            return bookingDtoList;
        }
    }
}
