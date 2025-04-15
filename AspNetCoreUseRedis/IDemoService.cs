namespace AspNetCoreUseRedis
{
    public interface IDemoService
    {
        Task PrintMessage(string message);
    }

    public class DemoService : IDemoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PrintMessage(string message)
        {
            // httpContext is null if the service is used outside of a request context
            var httpContext = _httpContextAccessor.HttpContext;
            await Console.Out.WriteLineAsync(message);
        }
    }
}
