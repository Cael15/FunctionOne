using Microsoft.Extensions.Hosting;

namespace FunctionOne
{
    public class Program    
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults().Build();

            host.Run();


        }
    }
}
