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
                ScheduleIdleElevators(idleElevators, Direction.Up);
                ScheduleIdleElevators(idleElevators, Direction.Down);
            }

            ScheduleMovingElevators(Direction.Up);
            ScheduleMovingElevators(Direction.Down);
        }

        private void ScheduleIdleElevators(List<Elevator> idleElevators, Direction direction)
        {
            var requestList = new List<Request>();
            requestList = direction == Direction.Up ? _dataService.UpRequestList : _dataService.DownRequestList;

            var canSchedule = idleElevators.Count > 0 && requestList.Count > 0;
            if (canSchedule)
            {
                var firstRequest = requestList.First();
                var closestElevator = FindNearestElevator(firstRequest.CurrentFloor, idleElevators);
                var eligibleUpRequests = EligibleRequests(closestElevator.Floor, firstRequest.Direction);

                var dispatched = _dataService.Elevators.SelectMany(x => x.RequestList).Any(y => y.RequestType == RequestType.Fetch && y.DestinationFloor == firstRequest.CurrentFloor);
                if (eligibleUpRequests.Count == 0 && !dispatched)
                {
                    closestElevator.AddRequest(
                    RequestHelper.Create(RequestType.Fetch, closestElevator.Floor, firstRequest.CurrentFloor));
                    closestElevator.AddRequest(firstRequest);

                    requestList.Remove(firstRequest);
                    idleElevators.Remove(closestElevator);
                }
                else
                {
                    closestElevator.AddRequests(eligibleUpRequests);

                    foreach (var req in eligibleUpRequests)
                    {
                        requestList.Remove(req);
                    }
                    idleElevators.Remove(closestElevator);
                }
            }
        }

        private void ScheduleMovingElevators(Direction direction)
        {
            var requestList = new List<Request>();
            requestList = direction == Direction.Up ? _dataService.UpRequestList : _dataService.DownRequestList;

            var elevatorsMovingInDirection = _dataService.Elevators.Where(x => x.Direction.HasValue && x.Direction.Value == direction) ?? [];

            foreach (var elevator in elevatorsMovingInDirection)
            {
                var eligibleUpRequests = EligibleRequests(elevator.Floor, elevator.Direction!.Value);

                elevator.AddRequests(eligibleUpRequests);

                foreach (var req in eligibleUpRequests)
                {
                    requestList.Remove(req);
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
