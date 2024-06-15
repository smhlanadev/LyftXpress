using LyftXpress.Services.Implementation;

namespace LyftEpress.Tests.Services
{
    [TestFixture]
    internal class ElevatorServiceTest
    {
        ElevatorService _elevatorService;

        [SetUp]
        public void SetUp()
        {
            _elevatorService = new ElevatorService();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ElevatorService_Initialise_Throws_ArgumentOutOfRangeException(int numberOfElevators)
        {
            // Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => _elevatorService.Initialise(numberOfElevators), nameof(numberOfElevators));
        }

        [TestCase(1)]
        [TestCase(10)]
        public void ElevatorService_Initialise_Creates_Elevators(int numberOfElevators)
        {
            // Act

            _elevatorService.Initialise(numberOfElevators);

            // Assert

            Assert.That(_elevatorService.Elevators, Has.Count.EqualTo(numberOfElevators));
        }
    }
}
