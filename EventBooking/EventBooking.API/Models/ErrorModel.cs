namespace EventBooking.API.Models
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
