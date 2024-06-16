using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Implementation;
using Moq;

namespace LyftEpress.Tests.Services
{
    [TestFixture]
    internal class ElevatorServiceTests
    {
        ElevatorService _elevatorService;

        Mock<IData> _mockData;
        Mock<IScheduler> _mockScheduler;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockData = new Mock<IData>();
            _mockScheduler = new Mock<IScheduler>();

            _mockData.Setup(d => d.Initialise(It.IsAny<int>(), It.IsAny<int>()));
            _mockScheduler.Setup(s => s.Schedule());

            _elevatorService = new ElevatorService(_mockData.Object, _mockScheduler.Object);
        }

        [Test]
        public void ElevatorService_Initialise_Is_Successful()
        {
            // Arrange

            // Act

            _elevatorService.Initialise(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            _mockData.Verify(d => d.Initialise(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void ElevatorService_AddRequest_Is_Successful()
        {
            // Arrange

            // Act

            _elevatorService.AddRequest(It.IsAny<string>());

            // Assert

            _mockData.Verify(d => d.AddRequest(It.IsAny<string>()), Times.Once());
        }
    }
}
