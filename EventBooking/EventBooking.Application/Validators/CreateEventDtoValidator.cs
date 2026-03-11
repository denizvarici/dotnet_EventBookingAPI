using EventBooking.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title can not be empty")
                .MaximumLength(200).WithMessage("Title can not be more than 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description can not be empty");

            RuleFor(x => x.Date)
                .GreaterThan(DateTime.Now).WithMessage("Date must be future");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location can not be empty");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity can not be below 1");

        }
    }
}
