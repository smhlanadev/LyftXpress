using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LyftXpress
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Register services for DI
            var serviceProvider = new ServiceCollection()
            .AddScoped<IData, DataService>()
            .AddScoped<IElevator, ElevatorService>()
            .AddScoped<IScheduler, Scheduler>()
            .BuildServiceProvider();

            
            Instructions();
            Console.WriteLine("Number of floors: ");
            string? input = Console.ReadLine();
            if (input == "exit") return;
            var numberOfFloors = int.Parse(input!);
            Console.WriteLine("Number of elevators: ");
            input = Console.ReadLine();
            if (input == "exit") return;
            var numberOfElevators = int.Parse(input!);

            var elevatorService = serviceProvider.GetService<IElevator>();
            elevatorService!.Initialise(numberOfElevators, numberOfFloors);

            var exit = false;
            Console.WriteLine("Enter command, or 'exit' to exit");

            while (!exit)
            {
                // Take input only when key is pressed to allow normal execution of the program
                var command = string.Empty;
                if (Console.KeyAvailable)
                {
                    command = Console.ReadLine();
                    if (command == "exit") break;
                }

                if (string.IsNullOrEmpty(command)) continue;
                elevatorService!.AddRequest(command!);
            }
        }

        static void Instructions()
        {
            Console.WriteLine("----Welcome to LyftXpress----");
            Console.WriteLine("To start, first enter the number of elevators and the number of floors.");
            Console.WriteLine("Then, enter a string command in the format 'current_floor destination_floor' to request an elevator.");
            Console.WriteLine("current_floor and destination_floor are integer values: 0 >= value >= number_of_floors");
            Console.WriteLine("To exit type 'exit'");
        }
    }
}
    