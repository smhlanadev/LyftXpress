using System.ComponentModel;

namespace LyftXpress.Models
{
    public class Elevator
    {
        private List<Request> _requestList;

        public int Id { get; init; }
        public bool IsMoving { get; set; }
        public bool IsOpen { get; set; }
        public int Floor { get; set; }
        public Direction? Direction { get; set; }
        public int NumberOfPassengers { get; set; }
        public int NumberOfFloors { get; set; }
        public List<Request> RequestList { 
            get => _requestList;
            private set 
            { 
                _requestList = value;
                Move();
            }
        }

        public Elevator(int id, int numberOfFloors)
        {
            Id = id;
            _requestList = RequestList = [];
            Floor = 0;
            NumberOfFloors = numberOfFloors;
        }

        // Could have used the List Add() method here, but I created a custom add method so I can call the Move() method
        public void AddRequest(Request request)
        {
            _requestList.Add(request);
            Move();
        }

        // Could have used the List AddRange() method here, but I created a custom add method so I can call the Move() method
        public void AddRequests(List<Request> requests)
        {
            _requestList.AddRange(requests);
            Move();
        }

        public void Move()
        {
            // No need to continue if there are no requests to process or the elevator is already moving
            if (_requestList.Count == 0 || IsMoving) return;

            IsMoving = true;

            // Run this in the background so that it does not halt the UI
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerCompleted += (s, e) =>
            {
                backgroundWorker.Dispose();
                Console.WriteLine($"Elevator {Id} requests completed.");
            };
            backgroundWorker.DoWork += (s, e) => MoveElevator();
            backgroundWorker.RunWorkerAsync();
        }

        private void MoveElevator()
        {
            while (_requestList.Count > 0 && Floor >= 0 && Floor <= NumberOfFloors)
            {
                // If the elevator is fetching, the direction is of the fetch request
                var fetchRequest = _requestList.FirstOrDefault(x => x.RequestType == RequestType.Fetch);
                Direction = fetchRequest is not null ? fetchRequest.Direction : _requestList[0].Direction;
                var elevatorMoved = false;

                Console.WriteLine($"Elevator {Id}, Floor {Floor}, Direction {Direction}");

                Thread.Sleep(1000); // Adding sleep operations to slow down the movement
                if (Direction.Value == Models.Direction.Up && Floor < NumberOfFloors)
                {
                    Floor++;
                    elevatorMoved = true;
                }
                else if (Direction.Value == Models.Direction.Down && Floor > 0)
                {
                    Floor--;
                    elevatorMoved = true;
                }

                if (elevatorMoved) FloorAction();
            }
            IsMoving = false;
        }

        private void FloorAction()
        {
            // If there are any requests from this floor or to this floor, open the elevator, remove requests to this floor
            if (_requestList.Any(x => x.CurrentFloor == Floor || x.DestinationFloor == Floor))
            {
                IsOpen = true;
                Console.WriteLine($"Elevator {Id}, Floor {Floor}, Direction {Direction} Door IsOpen {IsOpen}");
                Thread.Sleep(1000);
                var hasFulfiled = _requestList.Any(x => x.DestinationFloor == Floor);
                if (hasFulfiled) _requestList.RemoveAll(x => x.DestinationFloor == Floor);
                IsOpen = false;
                Console.WriteLine($"Elevator {Id}, Floor {Floor}, Direction {Direction} Door IsOpen {IsOpen}");
            }
        }
    }
}
