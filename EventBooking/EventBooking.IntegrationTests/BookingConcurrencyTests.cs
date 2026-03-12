using EventBooking.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace EventBooking.IntegrationTests
{
    public class BookingConcurrencyTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public BookingConcurrencyTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateBooking_ConcurrentRequests_ShouldHandleRaceCondition()
        {
            // GEREKSİNİMLER:
            // 1. DB'de kapasitesi 1 olan bir etkinlik olmalı.
            // 2. İki farklı User Token'ı olmalı (Auth bypass edilebilir veya seed edilebilir).

            var eventId = Guid.Parse("..."); // Seed ettiğin bir event ID
            var dto = new CreateBookingDto { EventId = eventId };

            // İki isteği AYNI ANDA hazırlıyoruz
            var task1 = _client.PostAsJsonAsync("/api/Bookings", dto);
            var task2 = _client.PostAsJsonAsync("/api/Bookings", dto);

            // İkisini de aynı anda fırlatıyoruz
            var responses = await Task.WhenAll(task1, task2);

            // ANALİZ:
            // Biri 201 (Created) dönmeli, diğeri ise bizim BusinessException sayesinde 400 veya 409 dönmeli.
            var successCount = responses.Count(r => r.StatusCode == HttpStatusCode.Created);
            var conflictCount = responses.Count(r => r.StatusCode == HttpStatusCode.BadRequest || r.StatusCode == HttpStatusCode.Conflict);

            Assert.Equal(1, successCount); // Sadece biri bilet alabilmiş olmalı!
            Assert.Equal(1, conflictCount); // Diğeri hata almış olmalı!
        }
    }
}
