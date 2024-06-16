using LyftXpress.Services.Implementation;
using LyftXpress.Models;

namespace LyftEpress.Tests.Services
{
    [TestFixture]
    internal class SchedulerTests
    {
        Scheduler _scheduler;
        DataService _dataService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataService = new();
            _scheduler = new(_dataService);
        }

        [Test]
        public void Scheduler_Schedule_NoRequests_Should_NotSchedule()
        {
            // Arrange

            _dataService.Elevators.Clear();
            _dataService.Elevators.Add(new Elevator(4));
            _dataService.Elevators.Add(new Elevator(4));
            _dataService.UpRequestList.Clear();
            _dataService.DownRequestList.Clear();
            
            // Act

            _scheduler.Schedule();

            // Assert

            Assert.That(_dataService.Elevators[0].RequestList, Is.Empty);
            Assert.That(_dataService.Elevators[1].RequestList, Is.Empty);
        }

        [Test]
        public void Scheduler_Schedule_WithRequests_Should_Schedule_Case1()
        {
            // Arrange

            _dataService.Elevators.Clear();
            _dataService.Elevators.Add(new Elevator(4));
            _dataService.Elevators.Add(new Elevator(4));

            _dataService.UpRequestList = [
                new() { CurrentFloor = 0, DestinationFloor = 1, Direction = Direction.Up },
                new() { CurrentFloor = 0, DestinationFloor = 2, Direction = Direction.Up },
                new() { CurrentFloor = 0, DestinationFloor = 3, Direction = Direction.Up },
            ];

            _dataService.DownRequestList = [
                new() { CurrentFloor = 3, DestinationFloor = 0, Direction = Direction.Down },
                new() { CurrentFloor = 2, DestinationFloor = 0, Direction = Direction.Down },
                new() { CurrentFloor = 1, DestinationFloor = 0, Direction = Direction.Down },
            ];

            // Act

            _scheduler.Schedule();

            // Assert

            Assert.That(_dataService.Elevators[0].RequestList.Count, Is.EqualTo(3));
            Assert.That(_dataService.Elevators[1].RequestList.Count, Is.EqualTo(1));
        }

        [Test]
        public void Scheduler_Schedule_WithRequests_Should_Schedule_Case2()
        {
            // Arrange

            _dataService.Elevators.Clear();
            _dataService.Elevators.Add(new Elevator(4) { Floor = 1 });
            _dataService.Elevators.Add(new Elevator(4) { Floor = 1 });

            _dataService.UpRequestList = [
                new() { CurrentFloor = 0, DestinationFloor = 3, Direction = Direction.Up },
                new() { CurrentFloor = 1, DestinationFloor = 3, Direction = Direction.Up },
                new() { CurrentFloor = 2, DestinationFloor = 3, Direction = Direction.Up },
            ];

            _dataService.DownRequestList = [
                new() { CurrentFloor = 3, DestinationFloor = 0, Direction = Direction.Down },
                new() { CurrentFloor = 2, DestinationFloor = 0, Direction = Direction.Down },
                new() { CurrentFloor = 1, DestinationFloor = 0, Direction = Direction.Down },
            ];

            // Act

            _scheduler.Schedule();

            // Assert

            Assert.That(_dataService.Elevators[0].RequestList.Count, Is.EqualTo(2));
            Assert.That(_dataService.Elevators[1].RequestList.Count, Is.EqualTo(1));
        }
    }
}
