namespace AspNetCoreUseRedis
{
    public interface IDemoService
    {
        Task PrintMessage(string message);
    }

    public class DemoService : IDemoService
    {
        public async Task PrintMessage(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }
    }
}
