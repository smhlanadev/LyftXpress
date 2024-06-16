namespace LyftXpress.Services.Abstraction
{
    internal interface IElevator
    {
        void Initialise(int numberOfElevators, int numberOfFloors);
        void AddRequest(string request);
        void Move(int elevatorId);
    }
}
