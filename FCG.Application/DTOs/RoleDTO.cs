using System.Collections.Generic;

namespace FCG.Application.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }

    public class RoleCreateDTO
    {
        public string Name { get; set; }
    }

    public class RoleUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}