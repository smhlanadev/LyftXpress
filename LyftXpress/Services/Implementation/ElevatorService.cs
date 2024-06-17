using LyftXpress.Services.Abstraction;

namespace LyftXpress.Services.Implementation
{
    public class ElevatorService(IData dataService, IScheduler scheduler) : IElevator
    {
        private readonly IData _dataService = dataService;
        private readonly IScheduler _scheduler = scheduler;

        public void Initialise(int numberOfElevators, int numberOfFloors)
        {
            _dataService.Initialise(numberOfElevators, numberOfFloors);
        }

        public void AddRequest(string command)
        {
            _dataService.AddRequest(command);
            _scheduler.Schedule();
        }
    }
}
