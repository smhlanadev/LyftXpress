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
            _dataService.Elevators.Add(new Elevator());
            _dataService.Elevators.Add(new Elevator());
            _dataService.UpRequestList.Clear();
            _dataService.DownRequestList.Clear();
            
            // Act

            _scheduler.Schedule();

            // Assert

            Assert.That(_dataService.Elevators[0].RequestList, Is.Empty);
            Assert.That(_dataService.Elevators[1].RequestList, Is.Empty);
        }
    }
}
