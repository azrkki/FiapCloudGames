using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Application.Interfaces;
using FCG.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using FCG.Application.Validators;
using FluentValidation;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Public user registration endpoint with password validation
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCreateDTO userCreateDto)
        {
            _logger.LogInformation("User registration attempt for email: {Email}", userCreateDto?.Email ?? "Unknown");
            
            try
            {
                if (userCreateDto == null)
                {
                    _logger.LogWarning("User registration failed: Request body is null");
                    return BadRequest(new { message = "User data is required" });
                }

                var validator = new UserCreateDTOValidator();
                var validationResult = validator.Validate(userCreateDto);
                
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("User registration failed: Validation errors for email: {Email}", userCreateDto.Email);
                    
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );

                    return BadRequest(new
                    {
                        messagge = "Some errors happened"
                    });
                }

                // Password validation passed - proceed with user creation
                var result = _userService.CreateUser(userCreateDto);
                
                _logger.LogInformation("User registration successful for email: {Email}", userCreateDto.Email);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("User registration failed: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user registration for email: {Email}", userCreateDto?.Email);
                return StatusCode(500, new { message = "An error occurred during user registration" });
            }
        }

        [HttpGet]
        [AuthorizeAdmin]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        [AuthorizeAdmin]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user == null)
                    return NotFound();
                    
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Creates a new user (Admin only)
        /// </summary>
        [HttpPost]
        [AuthorizeAdmin]
        public IActionResult Post([FromBody] UserCreateDTO userCreateDto)
        {
            _logger.LogInformation("User creation attempt for email: {Email}", userCreateDto?.Email ?? "Unknown");
            
            try
            {
                if (userCreateDto == null)
                {
                    _logger.LogWarning("User creation failed: Request body is null");
                    return BadRequest(new { message = "User data is required" });
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("User creation failed: Validation errors for email: {Email}", userCreateDto.Email);
                    return BadRequest(ModelState);
                }

                // FluentValidation will automatically validate the DTO here
                // Password must meet security requirements: min 8 chars, uppercase, lowercase, numbers, special chars
                
                var result = _userService.CreateUser(userCreateDto);
                
                _logger.LogInformation("User creation successful for email: {Email}", userCreateDto.Email);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("User creation failed: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user creation for email: {Email}", userCreateDto?.Email);
                return StatusCode(500, new { message = "An error occurred during user creation" });
            }
        }

        [HttpPut]
        [AuthorizeAdmin]
        public IActionResult Put([FromBody] UserUpdateDTO input)
        {
            try
            {
                var userUpdateDto = new UserUpdateDTO
                {
                    Id = input.Id,
                    Name = input.Name,
                    Email = input.Email,
                    RoleId = input.RoleId
                };
                
                var result = _userService.UpdateUser(userUpdateDto);
                if (result == null)
                    return NotFound();
                    
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [AuthorizeAdmin]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var result = _userService.DeleteUser(id);
                if (!result)
                    return NotFound();
                    
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
