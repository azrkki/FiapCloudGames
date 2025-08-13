using FCG.Api.Attributes;
using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [AuthorizeAdmin]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) => _roleService = roleService;

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var roles = _roleService.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var role = _roleService.GetRoleById(id);
                if (role == null)
                    return NotFound();
                    
                return Ok(role);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] RoleCreateDTO input)
        {
            try
            {
                var roleCreateDto = new RoleCreateDTO
                {
                    Name = input.Name
                };
                
                var result = _roleService.CreateRole(roleCreateDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] RoleUpdateDTO input)
        {
            try
            {
                var roleUpdateDto = new RoleUpdateDTO
                {
                    Id = input.Id,
                    Name = input.Name
                };
                
                var result = _roleService.UpdateRole(roleUpdateDto);
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
        public IActionResult Delete([FromRoute] int id)
        {
            try 
            {
                var result = _roleService.DeleteRole(id);
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
