using LyftXpress.Models;

namespace LyftXpress.Services.Abstraction
{
    public interface IData
    {
        public List<Request> UpRequestList { get; set; }
        public List<Request> DownRequestList { get; set; }
        public List<Elevator> Elevators { get; set; }

        void Initialise(int numberOfElevators);
        void AddRequest(string request);
    }
}
