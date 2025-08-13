using System.Collections.Generic;

namespace FCG.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public char RemoveFlag { get; set; }
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();
    }

    public class UserCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class UserAuthResponseDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
    }

    public class UserPasswordUpdateDTO
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}