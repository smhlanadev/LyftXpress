using LyftXpress.Models;
using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Helpers;

namespace LyftXpress.Services.Implementation
{
    public class Scheduler(IData dataService) : IScheduler
    {
        private readonly IData _dataService = dataService;

        public void Schedule()
        {
            var idleElevators = _dataService.Elevators.Where(x => !x.IsMoving).ToList();

            // While there are still idle elevators and requests that have not been processed
            while (idleElevators.Count > 0 && (_dataService.UpRequestList.Count > 0 || _dataService.DownRequestList.Count > 0))
            {
                var canSchedule = idleElevators.Count > 0 && _dataService.UpRequestList.Count > 0;
                if (canSchedule)
                {
                    var firstRequest = _dataService.UpRequestList.First();
                    var closestElevator = FindNearestElevator(firstRequest.CurrentFloor, idleElevators);
                    var eligibleUpRequests = EligibleRequests(closestElevator.Floor, firstRequest.Direction);

                    var dispatched = _dataService.Elevators.SelectMany(x => x.RequestList).Any(y => y.RequestType == RequestType.Fetch && y.DestinationFloor == firstRequest.CurrentFloor);
                    if (eligibleUpRequests.Count == 0 && !dispatched)
                    {
                        closestElevator.AddRequest(
                        RequestHelper.Create(RequestType.Fetch, closestElevator.Floor, firstRequest.CurrentFloor));
                        closestElevator.AddRequest(firstRequest);

                        _dataService.UpRequestList.Remove(firstRequest);
                        idleElevators.Remove(closestElevator);
                    }
                    else
                    {
                        closestElevator.AddRequests(eligibleUpRequests);

                        foreach (var req in eligibleUpRequests)
                        {
                            _dataService.UpRequestList.Remove(req);
                        }
                        idleElevators.Remove(closestElevator);
                    }
                }
                canSchedule = idleElevators.Count > 0 && _dataService.DownRequestList.Count > 0;
                if (canSchedule)
                {
                    var firstRequest = _dataService.DownRequestList.First();
                    var closestElevator = FindNearestElevator(firstRequest.CurrentFloor, idleElevators);
                    var eligibleDownRequests = EligibleRequests(closestElevator.Floor, firstRequest.Direction);

                    var dispatched = _dataService.Elevators.SelectMany(x => x.RequestList).Any(y => y.RequestType == RequestType.Fetch && y.DestinationFloor == firstRequest.CurrentFloor);
                    if (eligibleDownRequests.Count == 0 && !dispatched)
                    {
                        closestElevator.AddRequest(
                        RequestHelper.Create(RequestType.Fetch, closestElevator.Floor, firstRequest.CurrentFloor));
                        closestElevator.AddRequest(firstRequest);

                        _dataService.DownRequestList.Remove(firstRequest);
                        idleElevators.Remove(closestElevator);
                    }
                    else
                    {
                        closestElevator.AddRequests(eligibleDownRequests);

                        foreach (var req in eligibleDownRequests)
                        {
                            _dataService.UpRequestList.Remove(req);
                        }
                        idleElevators.Remove(closestElevator);
                    }
                }
            }

            var direction = Direction.Up;
            var elevatorsMovingInDirection = _dataService.Elevators.Where(x => x.Direction.HasValue && x.Direction.Value == direction) ?? [];

            foreach (var elevator in elevatorsMovingInDirection)
            {
                var eligibleUpRequests = EligibleRequests(elevator.Floor, elevator.Direction!.Value);

                elevator.AddRequests(eligibleUpRequests);

                foreach (var req in eligibleUpRequests)
                {
                    _dataService.UpRequestList.Remove(req);
                }
            }

            direction = Direction.Down;
            elevatorsMovingInDirection = _dataService.Elevators.Where(x => x.Direction.HasValue && x.Direction.Value == direction) ?? [];

            foreach (var elevator in elevatorsMovingInDirection)
            {
                var eligibleDownRequests = EligibleRequests(elevator.Floor, elevator.Direction!.Value);

                elevator.AddRequests(eligibleDownRequests);

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
