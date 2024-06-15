namespace LyftXpress.Models
{
    internal class Elevator
    {
        public Guid Id { get; init; }
        public List<string> RequestList { get; set; } = [];
        public bool IsMoving { get; set; }
        public char? Direction { get; set; }
        public int NumberOfPassengers { get; set; }
        // Raise Event

        public Elevator() => Id = Guid.NewGuid();
    }
}
