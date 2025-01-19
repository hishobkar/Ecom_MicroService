using Example01.Models;
using Example01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Example01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public HomeController(AuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        try
        {
            await _authService.RegisterAsync(username, email, password);
            return Ok("User registered successfully");
        } catch(Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var token = await _authService.AuthenticateAsync(username, password);
        if (token == null)
            return Unauthorized("Invalid username or password");

        return Ok(new { Token = token });
    }
    
    [HttpGet("Get1")]
    public IActionResult Get1()
    {
        var cs = _configuration["MongoDB:ConnectionString"];
        return Ok(cs);
    }

    [HttpGet("GetUserProfiles")]
    [Authorize]
    public IActionResult GetUserProfiles()
    {
        // Dummy user profile data
        var users = new List<UserProfile>
        {
            new UserProfile
            {
                UserId = 1,
                FullName = "Alice Johnson",
                Email = "alice.johnson@example.com",
                Address = "123 Main Street, Springfield, USA",
                PhoneNumber = "555-1234",
                OrderHistory = new List<Order>
                {
                    new Order { OrderId = 101, ProductName = "Smartphone", Amount = 699.99m, OrderDate = "2024-12-01" },
                    new Order { OrderId = 102, ProductName = "Headphones", Amount = 199.99m, OrderDate = "2024-11-15" }
                }
            },
            new UserProfile
            {
                UserId = 2,
                FullName = "Bob Smith",
                Email = "bob.smith@example.com",
                Address = "456 Elm Street, Metropolis, USA",
                PhoneNumber = "555-5678",
                OrderHistory = new List<Order>
                {
                    new Order { OrderId = 103, ProductName = "Laptop", Amount = 1199.99m, OrderDate = "2024-12-03" }
                }
            },
            new UserProfile
            {
                UserId = 3,
                FullName = "Charlie Davis",
                Email = "charlie.davis@example.com",
                Address = "789 Oak Street, Gotham, USA",
                PhoneNumber = "555-9012",
                OrderHistory = new List<Order>
                {
                    new Order { OrderId = 104, ProductName = "Gaming Console", Amount = 499.99m, OrderDate = "2024-11-20" },
                    new Order { OrderId = 105, ProductName = "Wireless Controller", Amount = 59.99m, OrderDate = "2024-11-22" }
                }
            },
            new UserProfile
            {
                UserId = 4,
                FullName = "Diana Moore",
                Email = "diana.moore@example.com",
                Address = "101 Maple Avenue, Star City, USA",
                PhoneNumber = "555-3456",
                OrderHistory = new List<Order>
                {
                    new Order { OrderId = 106, ProductName = "Smartwatch", Amount = 299.99m, OrderDate = "2024-12-05" }
                }
            }
        };

        return Ok(users);
    }
}
