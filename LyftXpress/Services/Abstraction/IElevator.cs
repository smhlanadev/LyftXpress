namespace LyftXpress.Services.Abstraction
{
    internal interface IElevator
    {
        void Initialise(int numberOfElevators);
        void AddRequest(string request);
        // private AssignRequest(int elevatorId, string request);
        void Move(int elevatorId);
    }
}
