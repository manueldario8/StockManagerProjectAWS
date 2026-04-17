namespace StockManager.API.Middlewares
{
    public class ApiResponses
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public string? CorrelationId { get; set; }
    }
}
