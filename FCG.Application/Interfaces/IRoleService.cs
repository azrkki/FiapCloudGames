using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IRoleService
    {
        /// <summary>
        /// Obtém todas as funções (roles).
        /// </summary>
        /// <returns>Uma coleção de DTOs de funções.</returns>
        IEnumerable<RoleDTO> GetAllRoles();

        /// <summary>
        /// Obtém todas as funções (roles) de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de funções.</returns>
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();

        /// <summary>
        /// Obtém uma função (role) pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>O DTO da função encontrada ou null.</returns>
        RoleDTO GetRoleById(int id);

        /// <summary>
        /// Obtém uma função (role) pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da função encontrada ou null.</returns>
        Task<RoleDTO> GetRoleByIdAsync(int id);

        /// <summary>
        /// Obtém uma função (role) pelo seu nome.
        /// </summary>
        /// <param name="name">O nome da função.</param>
        /// <returns>O DTO da função encontrada ou null.</returns>
        RoleDTO GetRoleByName(string name);

        /// <summary>
        /// Obtém uma função (role) pelo seu nome de forma assíncrona.
        /// </summary>
        /// <param name="name">O nome da função.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da função encontrada ou null.</returns>
        Task<RoleDTO> GetRoleByNameAsync(string name);

        /// <summary>
        /// Cria uma nova função (role).
        /// </summary>
        /// <param name="roleDto">O DTO da função a ser criada.</param>
        /// <returns>O DTO da função criada.</returns>
        RoleDTO CreateRole(RoleCreateDTO roleDto);

        /// <summary>
        /// Cria uma nova função (role) de forma assíncrona.
        /// </summary>
        /// <param name="roleDto">O DTO da função a ser criada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da função criada.</returns>
        Task<RoleDTO> CreateRoleAsync(RoleCreateDTO roleDto);

        /// <summary>
        /// Atualiza uma função (role) existente.
        /// </summary>
        /// <param name="roleDto">O DTO da função a ser atualizada.</param>
        /// <returns>O DTO da função atualizada.</returns>
        RoleDTO UpdateRole(RoleUpdateDTO roleDto);

        /// <summary>
        /// Atualiza uma função (role) existente de forma assíncrona.
        /// </summary>
        /// <param name="roleDto">O DTO da função a ser atualizada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da função atualizada.</returns>
        Task<RoleDTO> UpdateRoleAsync(RoleUpdateDTO roleDto);

        /// <summary>
        /// Exclui uma função (role) pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>True se a função foi excluída com sucesso, caso contrário, False.</returns>
        bool DeleteRole(int id);

        /// <summary>
        /// Exclui uma função (role) pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se a função foi excluída com sucesso, caso contrário, False.</returns>
        Task<bool> DeleteRoleAsync(int id);
    }
}