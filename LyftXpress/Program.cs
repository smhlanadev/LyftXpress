using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LyftXpress.Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var serviceProvider = new ServiceCollection()
            .AddScoped<IData, DataService>()
            .AddScoped<IElevator, ElevatorService>()
            .AddScoped<IScheduler, Scheduler>()
            .BuildServiceProvider();

            var elevatorService = serviceProvider.GetService<IElevator>();
            elevatorService!.Initialise(1);
            elevatorService!.AddRequest("2 4");
        }
    }
}
    