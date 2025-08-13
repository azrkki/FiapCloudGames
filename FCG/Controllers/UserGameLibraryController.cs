using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Application.Interfaces;
using FCG.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserGameLibraryController : ControllerBase
    {
        private readonly IUserGameLibraryService _userGameLibraryService;
        private readonly ILogger<UserGameLibraryController> _logger;

        public UserGameLibraryController(IUserGameLibraryService userGameLibraryService, ILogger<UserGameLibraryController> logger)
        {
            _userGameLibraryService = userGameLibraryService;
            _logger = logger;
        }

        [HttpGet]
        [AuthorizeAdmin]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GetAll user game libraries requested");
            
            try
            {
                var libraries = _userGameLibraryService.GetAllUserGameLibraries();
                _logger.LogInformation("Successfully retrieved {Count} user game libraries", libraries.Count());
                return Ok(libraries);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving all user game libraries");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("user/{userId:int}")]
        [AuthorizeCommon]
        public IActionResult GetUserGames([FromRoute] int userId)
        {
            _logger.LogInformation("GetUserGames requested for userId: {UserId}", userId);
            
            try
            {
                // Get current user's role and ID from claims
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                
                _logger.LogDebug("Current user ID: {CurrentUserId}, Role: {Role}, Requested userId: {RequestedUserId}", currentUserId, userRole, userId);

                // If user is Common, they can only view their own library
                if (userRole == "Common" && userId != currentUserId)
                {
                    _logger.LogWarning("Common user {CurrentUserId} attempted to access library of user {RequestedUserId}", currentUserId, userId);
                    return Forbid("Common users can only view their own game library");
                }

                var userGames = _userGameLibraryService.GetUserGameLibrariesByUserId(userId);
                _logger.LogInformation("Successfully retrieved {Count} games for user {UserId}", userGames.Count(), userId);
                return Ok(userGames);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving games for user {UserId}", userId);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId:int}/{gameId:int}")]
        [AuthorizeCommon]
        public IActionResult GetByUserIdAndGameId([FromRoute] int userId, int gameId)
        {
            _logger.LogInformation("GetByUserIdAndGameId requested for userId: {UserId}, gameId: {GameId}", userId, gameId);
            
            try
            {
                // Get current user's role and ID from claims
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

                // If user is Common, they can only view their own library
                if (userRole == "Common" && userId != currentUserId)
                {
                    _logger.LogWarning("Common user {CurrentUserId} attempted to access library entry of user {RequestedUserId} for game {GameId}", currentUserId, userId, gameId);
                    return Forbid("Common users can only view their own game library");
                }

                var library = _userGameLibraryService.GetUserGameLibraryByUserIdAndGameId(userId, gameId);
                if (library == null)
                {
                    _logger.LogInformation("Library entry not found for userId: {UserId}, gameId: {GameId}", userId, gameId);
                    return NotFound();
                }
                    
                _logger.LogInformation("Successfully retrieved library entry for userId: {UserId}, gameId: {GameId}", userId, gameId);
                return Ok(library);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving library entry for userId: {UserId}, gameId: {GameId}", userId, gameId);
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [AuthorizeCommon]
        public IActionResult Post([FromBody] UserGameLibraryCreateDTO input)
        {
            try
            {
                // Get current user's role and ID from claims
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

                // If user is Common, they can only add games for themselves
                if (userRole == "Common" && input.UserId != currentUserId)
                {
                    return Forbid("Common users can only add games to their own library");
                }

                var result = _userGameLibraryService.AddGameToUserLibrary(input.UserId, input.GameId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [AuthorizeAdmin]
        public IActionResult Put([FromBody] UserGameLibraryUpdateDTO input)
        {
            try
            {
                // Remove game from current library
                var removed = _userGameLibraryService.RemoveGameFromUserLibrary(input.UserId, input.GameId);
                if (!removed)
                    return NotFound();

                // Add game from current library
                var result = _userGameLibraryService.AddGameToUserLibrary(input.UserId, input.UpdateToGameId);
                return Ok(result);
            }
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{userId:int}/{gameId:int}")]
        [AuthorizeCommon]
        public IActionResult Delete([FromRoute] int userId, int gameId)
        {
            try 
            {
                // Get current user's role and ID from claims
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

                // If user is Common, they can only delete games from their own library
                if (userRole == "Common" && userId != currentUserId)
                {
                    return Forbid("Common users can only delete games from their own library");
                }

                var result = _userGameLibraryService.RemoveGameFromUserLibrary(userId, gameId);
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
