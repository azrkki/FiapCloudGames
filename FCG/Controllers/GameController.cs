using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Application.Interfaces;
using FCG.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService) => _gameService = gameService;

        [HttpGet]
        [AuthorizeCommon]
        public IActionResult GetAll()
        {
            try
            {
                var games = _gameService.GetAllGames();
                return Ok(games);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        [AuthorizeCommon]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var game = _gameService.GetGameById(id);
                if (game == null)
                    return NotFound();
                    
                return Ok(game);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [AuthorizeAdmin]
        public IActionResult Post([FromBody] GameCreateDTO input)
        {
            try
            {
                var result = _gameService.CreateGame(input);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [AuthorizeAdmin]
        public IActionResult Put([FromBody] GameUpdateDTO input)
        {
            try
            {
                var gameUpdateDto = new GameUpdateDTO
                {
                    Id = input.Id,
                    Name = input.Name,
                    Description = input.Description,
                    Price = input.Price,
                    Discount = input.Discount,
                    IsOnSale = input.IsOnSale
                };
                
                var result = _gameService.UpdateGame(gameUpdateDto);
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
                var result = _gameService.DeleteGame(id);
                if (!result)
                    return NotFound();
                    
                return Ok();
            } 
            catch (Exception e)
            { 
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id:int}/discount")]
        [AuthorizeAdmin]
        public IActionResult ApplyDiscount([FromRoute] int id, [FromBody] ApplyDiscountDTO discountDto)
        {
            try
            {
                var result = _gameService.ApplyDiscount(id, discountDto.Discount);
                if (result == null)
                    return NotFound();
                    
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id:int}/sale-status")]
        [AuthorizeAdmin]
        public IActionResult UpdateSaleStatus([FromRoute] int id, [FromBody] UpdateSaleStatusDTO saleStatusDto)
        {
            try
            {
                var result = _gameService.UpdateSaleStatus(id, saleStatusDto.IsOnSale);
                if (result == null)
                    return NotFound();
                    
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
