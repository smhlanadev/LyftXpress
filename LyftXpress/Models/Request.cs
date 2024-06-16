namespace LyftXpress.Models
{
    public class Request
    {
        public Direction Direction { get; set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public RequestType? RequestType { get; set; }
    }
}
