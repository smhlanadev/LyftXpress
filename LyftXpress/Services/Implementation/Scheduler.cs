using LyftXpress.Models;
using LyftXpress.Services.Abstraction;

namespace LyftXpress.Services.Implementation
{
    public class Scheduler(IData dataService) : IScheduler
    {
        private readonly IData _dataService = dataService;

        public void Schedule()
        {
            var idleElevators = _dataService.Elevators.Where(x => !x.IsMoving);

            // While there are still idle elevators and requests that have not been processed
            while (idleElevators.Any() && (_dataService.UpRequestList.Count > 0 || _dataService.DownRequestList.Count > 0))
            {
                var canSchedule = idleElevators.Any() && _dataService.UpRequestList.Count > 0;
                if (canSchedule)
                {
                    var firstRequest = _dataService.UpRequestList.First();
                    var closestElevator = FindNearestElevator(firstRequest.CurrentFloor, idleElevators.ToList());
                    var eligibleUpRequests = EligibleRequests(closestElevator.Floor, firstRequest.Direction);
                    closestElevator.RequestList.AddRange(eligibleUpRequests);

                    foreach (var req in eligibleUpRequests)
                    {
                        _dataService.UpRequestList.Remove(req);
                    }
                }
                canSchedule = idleElevators.Any() && _dataService.DownRequestList.Count > 0;
                if (canSchedule)
                {
                    var firstRequest = _dataService.UpRequestList.First();
                    var closestElevator = FindNearestElevator(firstRequest.CurrentFloor, idleElevators.ToList());
                    var eligibleUpRequests = EligibleRequests(closestElevator.Floor, firstRequest.Direction);
                    closestElevator.RequestList.AddRange(eligibleUpRequests);

                    foreach (var req in eligibleUpRequests)
                    {
                        _dataService.UpRequestList.Remove(req);
                    }
                }
            }

            var direction = Direction.Up;
            var elevatorsMovingInDirection = _dataService.Elevators.Where(x => x.Direction.HasValue && x.Direction.Value == direction) ?? [];

            foreach (var elevator in elevatorsMovingInDirection)
            {
                var eligibleUpRequests = EligibleRequests(elevator.Floor, elevator.Direction!.Value);

                elevator.RequestList.AddRange(eligibleUpRequests);

                foreach (var req in eligibleUpRequests)
                {
                    _dataService.UpRequestList.Remove(req);
                }
            }

            direction = Direction.Up;
            elevatorsMovingInDirection = _dataService.Elevators.Where(x => x.Direction.HasValue && x.Direction.Value == direction) ?? [];

            foreach (var elevator in elevatorsMovingInDirection)
            {
                var eligibleDownRequests = EligibleRequests(elevator.Floor, elevator.Direction!.Value);

                elevator.RequestList.AddRange(eligibleDownRequests);

                foreach (var req in eligibleDownRequests)
                {
                    _dataService.DownRequestList.Remove(req);
                }
            }
        }

        private List<Request> EligibleRequests(int elevatorFloor, Direction direction)
        {
            var requests = direction == Direction.Up ? _dataService.UpRequestList.Where(x => x.CurrentFloor >= elevatorFloor) :
                _dataService.DownRequestList.Where(x => x.CurrentFloor <= elevatorFloor);
            
            return requests.ToList();
        }

        private Elevator FindNearestElevator(int floor, List<Elevator> elevators)
        {
            var closest = elevators.First();

            foreach(var elevator in elevators)
            {
                if (Math.Abs(floor - elevator.Floor) < Math.Abs(floor - closest.Floor)) closest = elevator;
            }

            return closest;
        }
    }
}
