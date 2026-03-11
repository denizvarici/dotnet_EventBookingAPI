using EventBooking.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace EventBooking.Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ValidateAsync<T>(T dto)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if(validator != null)
            {
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors);
                }
            }
        }
    }
}
