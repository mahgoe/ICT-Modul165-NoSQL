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
using JetstreamSkiserviceAPI.Models;
namespace JetstreamSkiserviceTests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly EmployeesController _controller;

        public EmployeeControllerTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _controller = new EmployeesController(_mockTokenService.Object, new NullLogger<EmployeesController>());
        }

        [Fact]
        public async Task Login_Success_ReturnsOkWithToken()
        {
            // Arrange
            var authDto = new AuthDto { Username = "testUser", Password = "testPassword" };
            _mockTokenService.Setup(service => service.GetEmployees())
            .ReturnsAsync(new List<EmployeeDto>
            {
            new EmployeeDto
                {
                EmployeeId = "1",
                Username = "testUser",
                Password = "testPassword",
                Attempts = 0
                }
            });

            _mockTokenService.Setup(service => service.CreateToken(It.IsAny<string>()))
                .Returns("testToken");

            // Act
            var result = await _controller.Login(authDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(okResult.Value);
            var dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            Assert.NotNull(dictionary);
            Assert.True(dictionary.ContainsKey("token"));
            var token = dictionary["token"];
            Assert.Equal("testToken", token);
        }

        [Fact]
        public void Unban_Success_ReturnsOk()
        {
            // Arrange
            var employeeId = "1";
            _mockTokenService.Setup(service => service.Unban(employeeId))
                .Verifiable();

            // Act
            var result = _controller.Unban(employeeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockTokenService.Verify(service => service.Unban(employeeId), Times.Once);
        }

    }
}
