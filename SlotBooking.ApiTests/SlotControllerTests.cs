using FluentAssertions;
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
        public async Task GetAvailability_ReturnsOkResult_WhenStartDateIsValid()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(1);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var weekDto = new AvailabilityFromDateDto(startDate);

            var expectedAvailabilityDto = new GetWeeklyAvailabilityDto(
                Monday: new List<string> { "09:00" },
                Tuesday: new List<string>(),
                Wednesday: new List<string>(),
                Thursday: new List<string>(),
                Friday: new List<string>(),
                Saturday: new List<string>(),
                Sunday: new List<string>()
            );

            // Act 
            _mediatorMock.Setup(m => m.Send(It.IsAny<AvailabilityFromDateDto>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedAvailabilityDto);

            var result = await _controller.GetAvailability(startDate);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var returnedModel = okResult.Value as SlotAvailabilityModel;
            returnedModel.Should().NotBeNull();
            returnedModel.Monday.Count().Should().Be(1);
            returnedModel.Monday.First().Should().Be("09:00");
            returnedModel.Tuesday.Should().BeEmpty();
            returnedModel.Wednesday.Should().BeEmpty();
            returnedModel.Thursday.Should().BeEmpty();
            returnedModel.Friday.Should().BeEmpty();
            returnedModel.Saturday.Should().BeEmpty();
            returnedModel.Sunday.Should().BeEmpty();
        }


        [Test]
        public async Task GetAvailability_ReturnsBadRequest_WhenStartDateIsNotValid()
        {
            // Arrange
            var date = DateTime.UtcNow.AddDays(-1);
            _controller.ModelState.AddModelError("week", "Invalid date");

            // Act
            var result = await _controller.GetAvailability(date);

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
                Comments = "fake comment",
                Slot = DateTime.UtcNow,
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
                Comments = "This is a comment",
                Slot = DateTime.UtcNow
            };

            _controller.ModelState.AddModelError("slot", expectedErrorMessage);

            // Act
            var result = await _controller.TakeSlot(takeSlotModel);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}