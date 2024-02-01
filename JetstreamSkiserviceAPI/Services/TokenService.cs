using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JetstreamSkiserviceAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMongoCollection<Employee> _employeesCollection;
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;


        public TokenService(IMongoDatabase database, IConfiguration configuration)
        {
            _employeesCollection = database.GetCollection<Employee>("Employees");
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        }

        public string CreateToken(string username)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId, username)
                };

                var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating token: {ex.Message}", ex);
            }
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var employees = await _employeesCollection.Find(_ => true).ToListAsync();
            // Konvertierung von Employee zu EmployeeDto
            return employees.Select(emp => new EmployeeDto
            {
                EmployeeId = emp.Id,
                Username = emp.Username,
                Password = emp.Password, // Beachten Sie die Sicherheitsrisiken bei der Rückgabe von Passwörtern
                Attempts = emp.Attempts
            }).ToList();
        }

        public async Task Attempts(string employeeId)
        {
            var update = Builders<Employee>.Update.Inc(e => e.Attempts, 1);
            await _employeesCollection.UpdateOneAsync(e => e.Id == employeeId, update);
        }

        public async Task Unban(string employeeId)
        {
            var update = Builders<Employee>.Update.Set(e => e.Attempts, 0);
            await _employeesCollection.UpdateOneAsync(e => e.Id == employeeId, update);
        }
    }
    }