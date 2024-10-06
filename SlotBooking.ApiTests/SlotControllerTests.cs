using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlotBooking.Application.Slot.Commands;
using SlotBooking.Application.Slot.Queries;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Infrastructure.HttpClients;
using SlotBookingAPI.Controllers;
using SlotBookingAPI.Model.BookingSlot;

namespace SlotBooking.ApiTests
{
    [TestFixture]
    public class SlotControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private SlotController _controller;
        private Mock<IApiClient> _apiClientMock;
        private Mock<IAvailabilityService> _availabilityServiceMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SlotController(_mediatorMock.Object);

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            _apiClientMock = new Mock<IApiClient>();
            _availabilityServiceMock = new Mock<IAvailabilityService>();
        }

        [Test]
        public async Task GetAvailability_ReturnsOkResult_WhenWeekIsValid()
        {
            // Arrange
            var week = "20241007";
            var weekDto = new WeekDto(week);

            var expectedAvailabilityDto = new GetWeeklyAvailabilityDto(
                Monday: new List<AvailableSlotDto> { new AvailableSlotDto(DateTime.Parse("2024-10-07T09:00"), DateTime.Parse("2024-10-07T09:30")) },
                Tuesday: new List<AvailableSlotDto>(),
                Wednesday: new List<AvailableSlotDto>(),
                Thursday: new List<AvailableSlotDto>(),
                Friday: new List<AvailableSlotDto>(),
                Saturday: new List<AvailableSlotDto>(),
                Sunday: new List<AvailableSlotDto>()
            );

            // Act 
            _mediatorMock.Setup(m => m.Send(It.IsAny<WeekDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedAvailabilityDto);

            var result = await _controller.GetAvailability(week);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var returnedModel = okResult.Value as SlotAvailabilityModel;
            Assert.That(returnedModel, Is.Not.Null);
            Assert.That(returnedModel.Monday.Count(), Is.EqualTo(1));
            Assert.That(returnedModel.Monday.First().Start, Is.EqualTo(DateTime.Parse("2024-10-07T09:00")));
            Assert.That(returnedModel.Monday.First().End, Is.EqualTo(DateTime.Parse("2024-10-07T09:30")));
            Assert.That(returnedModel.Tuesday.Count(), Is.EqualTo(0));
            Assert.That(returnedModel.Wednesday.Count(), Is.EqualTo(0));
            Assert.That(returnedModel.Thursday.Count(), Is.EqualTo(0));
            Assert.That(returnedModel.Friday.Count(), Is.EqualTo(0));
            Assert.That(returnedModel.Saturday.Count(), Is.EqualTo(0));
            Assert.That(returnedModel.Sunday.Count(), Is.EqualTo(0));
        }


        [Test]
        [TestCase("invalidFormat", "Week must be in 'yyyyMMdd' format.")]
        [TestCase("20241008", "The week must represent a Monday.")]
        public async Task GetAvailability_ReturnsBadRequest_WhenWeekIsNotValid(string week, string expectedErrorMessage)
        {
            // Arrange
            _controller.ModelState.AddModelError("week", expectedErrorMessage);

            // Act
            var result = await _controller.GetAvailability(week);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }


        [Test]
        public async Task TakeSlot_ReturnsOkResult_WhenRequestIsValid()
        {
            // Arrange
            var takeSlotModel = new TakeSlotModel
            {
                Patient = new PatientInformationModel { 
                    Email = "fake@email.com",
                    Name = "name",
                    SecondName = "secondname",
                    Phone = "555 44 44 55"
                },
                FacilityId = new Guid(),
                Comments = "fake comment",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<TakeSlotDto>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.TakeSlot(takeSlotModel);

            // Assert
            var okResult = result as OkResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        [TestCase(null, "Doe", "fake@email.com", "555 444 4455", "First name is required.", false)]
        [TestCase("John", null, "fake@email.com", "555 444 4455", "Second name is required.", false)]
        [TestCase("John", "Doe", "invalid-email", "555 444 4455", "Invalid email format.", false)]
        [TestCase("John", "Doe", "fake@email.com", "invalid-phone", "Invalid phone number.", false)]
        public async Task TakeSlotModel_ReturnsValidationError_WhenRequestIsNotValid(string? name, string? secondName, string email, string phone, string expectedErrorMessage, bool validateDate)
        {
            // Arrange
            var takeSlotModel = new TakeSlotModel
            {
                Patient = new PatientInformationModel
                {
                    Email = email,
                    Name = name,
                    SecondName = secondName,
                    Phone = phone
                },
                FacilityId = Guid.NewGuid(),
                Comments = "This is a comment",
                End =  validateDate ? DateTime.Now : DateTime.Now.AddDays(1),
                Start = validateDate ? DateTime.Now.AddDays(1) : DateTime.Now,
            };

            _controller.ModelState.AddModelError("slot", expectedErrorMessage);

            // Act
            var result = await _controller.TakeSlot(takeSlotModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}