using JetstreamSkiserviceAPI.Controllers;
using JetstreamSkiserviceAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetstreamSkiserviceAPI.Controllers;
using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

namespace JetstreamSkiserviceTests
{
    public class RegistrationsControllerTests
    {
        private readonly Mock<IRegistrationService> _mockRegistrationService;
        private readonly RegistrationsController _controller;

        public RegistrationsControllerTests()
        {
            _mockRegistrationService = new Mock<IRegistrationService>();
            _controller = new RegistrationsController(_mockRegistrationService.Object, new NullLogger<RegistrationsController>());
        }

        [Fact]
        public async Task CreateRegistration_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newRegistrationDto = new CreateRegistrationDto {
                FirstName = "Karen",
                LastName = "Muster",
                Email = "tailor@mustermann.com",
                Phone = "0791234569",
                Status = "InArbeit",
                Priority = "Standard",
                Service = "Grosser Service",
                Price = 110,
                Comment = "Testkommentar"
            };
            var createdRegistrationDto = new RegistrationDto { Id = "1",
                FirstName = "Karen",
                LastName = "Muster",
                Email = "tailor@mustermann.com",
                Phone = "0791234569",
                Status = "InArbeit",
                Priority = "Standard",
                Service = "Grosser Service",
                Price = 110,
                Comment = "Testkommentar"
            };

            _mockRegistrationService.Setup(service => service.AddRegistration(It.IsAny<CreateRegistrationDto>()))
                .ReturnsAsync(createdRegistrationDto);

            // Act
            var result = await _controller.CreateRegistration(newRegistrationDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetRegistration", createdAtActionResult.ActionName);
            Assert.Equal("1", ((RegistrationDto)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateRegistration_Success_ReturnsOkResult()
        {
            // Arrange
            var validId = "existingId";
            var updateDto = new CreateRegistrationDto
            {
                FirstName = "Karen",
                LastName = "Muster",
                Email = "tailor@mustermann.com",
                Phone = "0791234569",
                Status = "InArbeit",
                Priority = "Standard",
                Service = "Grosser Service",
                Price = 110,
                Comment = "Testkommentar"
            };
            _mockRegistrationService.Setup(service => service.UpdateRegistration(validId, updateDto))
                .Returns(Task.CompletedTask); // Simulate a successful update

            // Act
            var result = await _controller.UpdateRegistration(validId, updateDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateRegistration_NotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = "nonExistingId";
            var updateDto = new CreateRegistrationDto
            {
                FirstName = "Karen",
                LastName = "Muster",
                Email = "tailor@mustermann.com",
                Phone = "0791234569",
                Status = "InArbeit",
                Priority = "Standard",
                Service = "Grosser Service",
                Price = 110,
                Comment = "Testkommentar"
            };
            _mockRegistrationService.Setup(service => service.UpdateRegistration(nonExistingId, updateDto))
                .Throws(new KeyNotFoundException()); // Simulate not finding the registration

            // Act
            var result = await _controller.UpdateRegistration(nonExistingId, updateDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRegistration_Success_ReturnsNoContentResult()
        {
            // Arrange
            var existingId = "existingId";
            _mockRegistrationService.Setup(service => service.DeleteRegistration(existingId))
                .Returns(Task.CompletedTask); // Simulate a successful deletion

            // Act
            var result = await _controller.DeleteRegistration(existingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRegistration_NotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistingId = "nonExistingId";
            _mockRegistrationService.Setup(service => service.DeleteRegistration(nonExistingId))
                .Throws(new KeyNotFoundException()); // Simulate not finding the registration

            // Act
            var result = await _controller.DeleteRegistration(nonExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
