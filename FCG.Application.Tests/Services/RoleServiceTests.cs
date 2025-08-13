using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using FCG.Application.Services;
using FCG.Core.Interfaces;
using FCG.Core.Entity;
using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Tests.Services
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<IRoleRepository> _mockRoleRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ILogger<RoleService>> _mockLogger;
        private RoleService _roleService;
        private Role _testRole;
        private Role _adminRole;

        [SetUp]
        public void Setup()
        {
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<RoleService>>();

            _roleService = new RoleService(
                _mockRoleRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);

            // Setup test data
            _testRole = new Role("User");
            _adminRole = new Role("Admin");
        }

        [Test]
        public void GetAllRoles_ReturnsRoleDTOs()
        {
            // Arrange
            var roles = new List<Role> { _testRole, _adminRole };
            _mockRoleRepository.Setup(r => r.GetAll())
                .Returns(roles);

            // Act
            var result = _roleService.GetAllRoles();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            var roleDto = result.First();
            Assert.AreEqual(_testRole.Name, roleDto.Name);
        }

        [Test]
        public async Task GetAllRolesAsync_ReturnsRoleDTOs()
        {
            // Arrange
            var roles = new List<Role> { _testRole, _adminRole };
            _mockRoleRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(roles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            var roleDto = result.First();
            Assert.AreEqual(_testRole.Name, roleDto.Name);
        }

        [Test]
        public void GetRoleById_ExistingRole_ReturnsRoleDTO()
        {
            // Arrange
            var roleId = 1;
            _mockRoleRepository.Setup(r => r.GetById(roleId))
                .Returns(_testRole);

            // Act
            var result = _roleService.GetRoleById(roleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testRole.Name, result.Name);
        }

        [Test]
        public void GetRoleById_NonExistingRole_ReturnsNull()
        {
            // Arrange
            var roleId = 999;
            _mockRoleRepository.Setup(r => r.GetByIdWithUsers(roleId))
                .Returns((Role)null);

            // Act
            var result = _roleService.GetRoleById(roleId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetRoleByIdAsync_ExistingRole_ReturnsRoleDTO()
        {
            // Arrange
            var roleId = 1;
            _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId))
                .ReturnsAsync(_testRole);

            // Act
            var result = await _roleService.GetRoleByIdAsync(roleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testRole.Name, result.Name);
        }

        [Test]
        public void GetRoleByName_ExistingRole_ReturnsRoleDTO()
        {
            // Arrange
            var roleName = "User";
            _mockRoleRepository.Setup(r => r.GetByName(roleName))
                .Returns(_testRole);

            // Act
            var result = _roleService.GetRoleByName(roleName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testRole.Name, result.Name);
        }

        [Test]
        public void GetRoleByName_NonExistingRole_ReturnsNull()
        {
            // Arrange
            var roleName = "NonExistentRole";
            _mockRoleRepository.Setup(r => r.GetByName(roleName))
                .Returns((Role)null);

            // Act
            var result = _roleService.GetRoleByName(roleName);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetRoleByNameAsync_ExistingRole_ReturnsRoleDTO()
        {
            // Arrange
            var roleName = "User";
            _mockRoleRepository.Setup(r => r.GetByNameAsync(roleName))
                .ReturnsAsync(_testRole);

            // Act
            var result = await _roleService.GetRoleByNameAsync(roleName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testRole.Name, result.Name);
        }

        [Test]
        public void CreateRole_ValidData_ReturnsRoleDTO()
        {
            // Arrange
            var roleCreateDto = new RoleCreateDTO
            {
                Name = "Moderator"
            };

            _mockRoleRepository.Setup(r => r.GetByName(roleCreateDto.Name))
                .Returns((Role)null);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _roleService.CreateRole(roleCreateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roleCreateDto.Name, result.Name);
            _mockRoleRepository.Verify(r => r.Add(It.IsAny<Role>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void CreateRole_ExistingName_ThrowsException()
        {
            // Arrange
            var roleCreateDto = new RoleCreateDTO
            {
                Name = "User"
            };

            _mockRoleRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(true);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _roleService.CreateRole(roleCreateDto));
            Assert.AreEqual("Role with name 'User' already exists", ex.Message);
        }

        [Test]
        public async Task CreateRoleAsync_ValidData_ReturnsRoleDTO()
        {
            // Arrange
            var roleCreateDto = new RoleCreateDTO
            {
                Name = "Moderator"
            };

            _mockRoleRepository.Setup(r => r.GetByNameAsync(roleCreateDto.Name))
                .ReturnsAsync((Role)null);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _roleService.CreateRoleAsync(roleCreateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roleCreateDto.Name, result.Name);
            _mockRoleRepository.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateRole_ValidData_ReturnsUpdatedRoleDTO()
        {
            // Arrange
            var roleUpdateDto = new RoleUpdateDTO
            {
                Id = 1,
                Name = "Updated Role"
            };

            _mockRoleRepository.Setup(r => r.GetById(1))
                .Returns(_testRole);
            _mockRoleRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(false);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _roleService.UpdateRole(roleUpdateDto);

            // Assert
            Assert.IsNotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateRole_NonExistingRole_ThrowsException()
        {
            // Arrange
            var roleUpdateDto = new RoleUpdateDTO
            {
                Id = 999,
                Name = "Updated Role"
            };

            _mockRoleRepository.Setup(r => r.GetByIdWithUsers(999))
                .Returns((Role)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _roleService.UpdateRole(roleUpdateDto));
            Assert.AreEqual("Role ID 999 not found", ex.Message);
        }

        [Test]
        public void UpdateRole_DuplicateName_ThrowsException()
        {
            // Arrange
            var roleUpdateDto = new RoleUpdateDTO
            {
                Id = 1,
                Name = "Admin"
            };

            _mockRoleRepository.Setup(r => r.GetById(1))
                .Returns(_testRole);
            _mockRoleRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(true);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _roleService.UpdateRole(roleUpdateDto));
            Assert.AreEqual("Role with name 'Admin' already exists", ex.Message);
        }

        [Test]
        public void DeleteRole_ExistingRole_ReturnsTrue()
        {
            // Arrange
            var roleId = 1;
            _mockRoleRepository.Setup(r => r.GetById(roleId))
                .Returns(_testRole);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _roleService.DeleteRole(roleId);

            // Assert
            Assert.IsTrue(result);
            _mockRoleRepository.Verify(r => r.Remove(_testRole), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteRole_NonExistingRole_ReturnsFalse()
        {
            // Arrange
            var roleId = 999;
            _mockRoleRepository.Setup(r => r.GetById(roleId))
                .Returns((Role)null);

            // Act
            var result = _roleService.DeleteRole(roleId);

            // Assert
            Assert.IsFalse(result);
            _mockRoleRepository.Verify(r => r.Remove(It.IsAny<Role>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Test]
        public async Task DeleteRoleAsync_ExistingRole_ReturnsTrue()
        {
            // Arrange
            var roleId = 1;
            _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId))
                .ReturnsAsync(_testRole);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _roleService.DeleteRoleAsync(roleId);

            // Assert
            Assert.IsTrue(result);
            _mockRoleRepository.Verify(r => r.Remove(_testRole), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}