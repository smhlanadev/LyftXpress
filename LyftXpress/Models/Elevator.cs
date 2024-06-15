namespace LyftXpress.Models
{
    public class Elevator
    {
        public Guid Id { get; init; }
        public List<Request> RequestList { get; set; } = [];
        public bool IsMoving { get; set; }
        public int Floor { get; set; }
        public Direction? Direction { get; set; }
        public int NumberOfPassengers { get; set; }
        // Raise Event

        public Elevator()
        {
            Id = Guid.NewGuid();
            Floor = 0;
        }
    }
}
