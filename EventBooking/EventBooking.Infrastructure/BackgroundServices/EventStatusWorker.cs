using EventBooking.Application.Interfaces;
using EventBooking.Domain.Entities;
using EventBooking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.BackgroundServices
{
    public class EventStatusWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EventStatusWorker> _logger;

        public EventStatusWorker(IServiceScopeFactory scopeFactory, ILogger<EventStatusWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Event end dates started to checking.....");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await using (var scope = _scopeFactory.CreateAsyncScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        var expiredEvents = await unitOfWork.Repository<Event>()
                                                      .Where(e => e.Date < DateTime.Now && e.IsActive)
                                                      .ToListAsync();
                        if (expiredEvents.Any())
                        {
                            foreach (var @event in expiredEvents)
                            {
                                @event.IsActive = false;
                                unitOfWork.Repository<Event>().Update(@event);
                                _logger.LogInformation($"This event is now passive : {@event.Title}");
                            }

                            await unitOfWork.SaveChangesAsync();

                            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                            await cacheService.RemoveAsync("all_events");
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Event status updating exception");
                }

                await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
            }
        }
    }
}
