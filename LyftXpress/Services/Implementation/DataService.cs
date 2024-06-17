using LyftXpress.Models;
using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Helpers;

namespace LyftXpress.Services.Implementation
{
    /// <summary>
    /// This section handles the data related operations.
    /// It creates new elevator instances, receives and stores requests from the commands.
    /// </summary>
    public class DataService : IData
    {
        public List<Request> UpRequestList { get; set; } = [];
        public List<Request> DownRequestList { get; set; } = [];
        public List<Elevator> Elevators { get; set; } = [];

        public void Initialise(int numberOfElevators, int numberOfFloors)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfElevators);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfFloors);

            Elevators = [];
            for (int i = 0; i < numberOfElevators; i++)
            {
                Elevators.Add(new Elevator(i+1, numberOfFloors));
            }
        }

        public void AddRequest(string command)
        {
            var request = RequestHelper.Create(command);
            if (request.Direction == Direction.Up) UpRequestList.Add(request);
            else DownRequestList.Add(request);
        }
    }
}
