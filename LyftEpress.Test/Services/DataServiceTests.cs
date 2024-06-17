using LyftXpress.Services.Abstraction;
using LyftXpress.Services.Implementation;
using LyftXpress.Models;
using Moq;

namespace LyftEpress.Tests.Services
{
    [TestFixture]
    internal class DataServiceTests
    {
        DataService _dataService;

        Mock<IData> _mockData;
        Mock<IScheduler> _mockScheduler;


        [OneTimeSetUp]
        public void SetUp()
        {
            _mockData = new Mock<IData>();
            _mockScheduler = new Mock<IScheduler>();

            _mockData.Setup(d => d.Initialise(It.IsAny<int>(), It.IsAny<int>()));
            _mockScheduler.Setup(s => s.Schedule());

            _dataService = new DataService();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void DataService_Initialise_Invalid_NumberOfElevators_Throws_ArgumentOutOfRangeException(int numberOfElevators)
        {
            // Arrange

            // Act

            // Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => _dataService.Initialise(numberOfElevators, 4), nameof(numberOfElevators));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void DataService_Initialise_Invalid_NumberOfFloors_Throws_ArgumentOutOfRangeException(int numberOfFloors)
        {
            // Arrange

            // Act

            // Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => _dataService.Initialise(4, numberOfFloors), nameof(numberOfFloors));
        }

        [TestCase(1, 4)]
        [TestCase(10, 4)]
        public void DataService_Initialise_Creates_Elevators(int numberOfElevators, int floors)
        {
            // Act

            _dataService.Initialise(numberOfElevators, floors);

            // Assert

            Assert.That(_dataService.Elevators, Has.Count.EqualTo(numberOfElevators));

            for (int i = 0; i < _dataService.Elevators.Count; i++)
            {
                Assert.That(_dataService.Elevators[i].Id, Is.EqualTo(i+1));
                Assert.That(_dataService.Elevators[i].RequestList, Is.Empty);
                Assert.That(_dataService.Elevators[i].Floor, Is.EqualTo(0));
            }
        }

        [TestCase("0 4", Direction.Up)]
        [TestCase("2 4", Direction.Up)]
        [TestCase("4 0", Direction.Down)]
        [TestCase("4 2", Direction.Down)]
        public void DataService_AddRequest_Creates_Requests(string command, Direction expectedDirection)
        {
            // Arrange

            _dataService.UpRequestList.Clear();
            _dataService.DownRequestList.Clear();

            // Act

            _dataService.AddRequest(command);

            // Assert

            if (expectedDirection == Direction.Up)
            {
                Assert.That(_dataService.UpRequestList, Has.Count.EqualTo(1));
                Assert.That(_dataService.DownRequestList, Has.Count.EqualTo(0));
                Assert.That(_dataService.UpRequestList[0].Direction, Is.EqualTo(expectedDirection));
            }
            else
            {
                Assert.That(_dataService.UpRequestList, Has.Count.EqualTo(0));
                Assert.That(_dataService.DownRequestList, Has.Count.EqualTo(1));
                Assert.That(_dataService.DownRequestList[0].Direction, Is.EqualTo(expectedDirection));
            }
        }
    }
}
