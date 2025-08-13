using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            _logger.LogInformation("GetAllRoles called");
            try
            {
                var roles = _roleRepository.GetAll();
                var result = roles.Select(MapRoleToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} roles", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all roles");
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("GetAllRolesAsync called");
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                var result = roles.Select(MapRoleToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} roles asynchronously", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all roles asynchronously");
                throw;
            }
        }

        public RoleDTO GetRoleById(int id)
        {
            _logger.LogInformation("GetRoleById called with id: {RoleId}", id);
            try
            {
                var role = _roleRepository.GetById(id);
                if (role != null)
                {
                    _logger.LogInformation("Successfully retrieved role: {RoleName} (ID: {RoleId})", role.Name, id);
                    return MapRoleToDto(role);
                }
                else
                {
                    _logger.LogWarning("Role not found with id: {RoleId}", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role with id: {RoleId}", id);
                throw;
            }
        }

        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return role != null ? MapRoleToDto(role) : null;
        }

        public RoleDTO GetRoleByName(string name)
        {
            var role = _roleRepository.GetByName(name);
            return role != null ? MapRoleToDto(role) : null;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string name)
        {
            var role = await _roleRepository.GetByNameAsync(name);
            return role != null ? MapRoleToDto(role) : null;
        }

        public RoleDTO CreateRole(RoleCreateDTO roleDto)
        {
            if (roleDto == null)
                throw new ArgumentNullException(nameof(roleDto));

            // Check if name already exists
            if (_roleRepository.Any(r => r.Name == roleDto.Name))
                throw new InvalidOperationException($"Role with name '{roleDto.Name}' already exists");

            var role = new Role { Name = roleDto.Name };

            _roleRepository.Add(role);
            _unitOfWork.SaveChanges();

            return MapRoleToDto(role);
        }

        public async Task<RoleDTO> CreateRoleAsync(RoleCreateDTO roleDto)
        {
            if (roleDto == null)
                throw new ArgumentNullException(nameof(roleDto));

            // Check if name already exists
            if (await _roleRepository.AnyAsync(r => r.Name == roleDto.Name))
                throw new InvalidOperationException($"Role with name '{roleDto.Name}' already exists");

            var role = new Role { Name = roleDto.Name };

            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return MapRoleToDto(role);
        }

        public RoleDTO UpdateRole(RoleUpdateDTO roleDto)
        {
            if (roleDto == null)
                throw new ArgumentNullException(nameof(roleDto));

            var role = _roleRepository.GetById(roleDto.Id);
            if (role == null)
                throw new InvalidOperationException($"Role ID {roleDto.Id} not found");

            // Check if name already exists (if changing)
            if (roleDto.Name != role.Name && _roleRepository.Any(r => r.Name == roleDto.Name))
                throw new InvalidOperationException($"Role with name '{roleDto.Name}' already exists");

            role.Name = roleDto.Name;

            _roleRepository.Update(role);
            _unitOfWork.SaveChanges();

            return MapRoleToDto(role);
        }

        public async Task<RoleDTO> UpdateRoleAsync(RoleUpdateDTO roleDto)
        {
            if (roleDto == null)
                throw new ArgumentNullException(nameof(roleDto));

            var role = await _roleRepository.GetByIdAsync(roleDto.Id);
            if (role == null)
                throw new InvalidOperationException($"Role ID {roleDto.Id} not found");

            // Check if name already exists (if changing)
            if (roleDto.Name != role.Name && await _roleRepository.AnyAsync(r => r.Name == roleDto.Name))
                throw new InvalidOperationException($"Role with name '{roleDto.Name}' already exists");

            role.Name = roleDto.Name;

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            return MapRoleToDto(role);
        }

        public bool DeleteRole(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null)
                return false;

            // Check if there is user with this Role
            if (_roleRepository.GetByIdWithUsers(id)?.Users?.Any() == true)
                throw new InvalidOperationException("This is not possible to exclude a function that is used for users");

            _roleRepository.Remove(role);
            _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return false;

            // Check if there is user with this Role
            var roleWithUsers = await _roleRepository.GetByIdWithUsersAsync(id);
            if (roleWithUsers?.Users?.Any() == true)
                throw new InvalidOperationException("This is not possible to exclude a function that is used for users");

            _roleRepository.Remove(role);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Auxiliar method to map Role to RoleDTO
        private RoleDTO MapRoleToDto(Role role)
        {
            if (role == null)
                return null;

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}