namespace LyftXpress.Models
{
    public class Elevator
    {
        private List<Request> _requestList;

        public Guid Id { get; init; }
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

        public Elevator(int numberOfFloors)
        {
            Id = Guid.NewGuid();
            _requestList = RequestList = [];
            Floor = 0;
            NumberOfFloors = numberOfFloors;
        }

        public void AddRequest(Request request)
        {
            _requestList.Add(request);
            Move();
        }

        public void AddRequests(List<Request> requests)
        {
            _requestList.AddRange(requests);
            Move();
        }

        public void Move()
        {
            if (_requestList.Count == 0 || IsMoving) return;

            var actionThread = new Thread(MoveElevator);
            actionThread.Start();
        }

        private void MoveElevator()
        {
            Direction = _requestList[0].Direction;
            IsMoving = true;

            while (_requestList.Count > 0 && Floor > 0 && Floor < NumberOfFloors)
            {
                Thread.Sleep(1000);
                if (Direction.Value == Models.Direction.Up) Floor++;
                else Floor--;
            }
            IsMoving = false;
        }

        private void FloorAction()
        {
            // If there are any requests from this floor or to this floor, open the elevator, remove requests to this floor
            if (_requestList.Any(x => x.CurrentFloor == Floor || x.DestinationFloor == Floor))
            {
                IsOpen = true;
                Thread.Sleep(1000);
                _requestList.RemoveAll(x => x.DestinationFloor == Floor);
                IsOpen = false;
            }
        }
    }
}
