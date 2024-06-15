using LyftXpress.Models;

namespace LyftXpress.Services.Helpers
{
    public static class RequestHelper
    {
        public static Request Create(string request)
        {
            var requestProps = request.Split(' ');

            var isValidCurrentFloor = int.TryParse(requestProps[0], out int currentFloor);
            var isValidDestinationFloor = int.TryParse(requestProps[1], out int destinationFloor);

            if (!(isValidCurrentFloor && isValidDestinationFloor)) throw new ArgumentException(null, nameof(request));

            return new Request
            {
                CurrentFloor = currentFloor,
                DestinationFloor = destinationFloor,
                Direction = currentFloor < destinationFloor ? Direction.Up : Direction.Down
            }; 
        }
    }
}
