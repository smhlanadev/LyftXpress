using LyftXpress.Models;
using LyftXpress.Services.Abstraction;

namespace LyftXpress.Services.Implementation
{
    internal class ElevatorService : IElevator
    {
        private List<string> UpRequestList { get; set; } = [];
        private List<string> DownRequestList { get; set; } = [];
        private List<Elevator> Elevators { get; set; } = [];

        public void AddRequest(string request)
        {
            throw new NotImplementedException();
        }

        public void Initialise(int numberOfElevators)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfElevators);
            
            Elevators = [];
            for (int i = 0; i < numberOfElevators; i++)
            {
                Elevators.Add(new Elevator());
            }
        }

        public void Move(int elevatorId)
        {
            throw new NotImplementedException();
        }
    }
}
