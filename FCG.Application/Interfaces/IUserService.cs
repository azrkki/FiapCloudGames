using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <returns>Uma coleção de DTOs de usuários.</returns>
        IEnumerable<UserDTO> GetAllUsers();

        /// <summary>
        /// Obtém todos os usuários de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de usuários.</returns>
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Obtém um usuário pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>O DTO do usuário encontrado ou null.</returns>
        UserDTO GetUserById(int id);

        /// <summary>
        /// Obtém um usuário pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do usuário encontrado ou null.</returns>
        Task<UserDTO> GetUserByIdAsync(int id);

        /// <summary>
        /// Obtém um usuário pelo seu email.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>O DTO do usuário encontrado ou null.</returns>
        UserDTO GetUserByEmail(string email);

        /// <summary>
        /// Obtém um usuário pelo seu email de forma assíncrona.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do usuário encontrado ou null.</returns>
        Task<UserDTO> GetUserByEmailAsync(string email);

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="userDto">O DTO do usuário a ser criado.</param>
        /// <returns>O DTO do usuário criado.</returns>
        UserDTO CreateUser(UserCreateDTO userDto);

        /// <summary>
        /// Cria um novo usuário de forma assíncrona.
        /// </summary>
        /// <param name="userDto">O DTO do usuário a ser criado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do usuário criado.</returns>
        Task<UserDTO> CreateUserAsync(UserCreateDTO userDto);

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="userDto">O DTO do usuário a ser atualizado.</param>
        /// <returns>O DTO do usuário atualizado.</returns>
        UserDTO UpdateUser(UserUpdateDTO userDto);

        /// <summary>
        /// Atualiza um usuário existente de forma assíncrona.
        /// </summary>
        /// <param name="userDto">O DTO do usuário a ser atualizado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do usuário atualizado.</returns>
        Task<UserDTO> UpdateUserAsync(UserUpdateDTO userDto);

        /// <summary>
        /// Exclui um usuário pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>True se o usuário foi excluído com sucesso, caso contrário, False.</returns>
        bool DeleteUser(int id);

        /// <summary>
        /// Exclui um usuário pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se o usuário foi excluído com sucesso, caso contrário, False.</returns>
        Task<bool> DeleteUserAsync(int id);

        /// <summary>
        /// Adiciona um jogo à biblioteca do usuário.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>True se o jogo foi adicionado com sucesso, caso contrário, False.</returns>
        bool AddGameToUserLibrary(int userId, int gameId);

        /// <summary>
        /// Adiciona um jogo à biblioteca do usuário de forma assíncrona.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se o jogo foi adicionado com sucesso, caso contrário, False.</returns>
        Task<bool> AddGameToUserLibraryAsync(int userId, int gameId);

        /// <summary>
        /// Remove um jogo da biblioteca do usuário.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>True se o jogo foi removido com sucesso, caso contrário, False.</returns>
        bool RemoveGameFromUserLibrary(int userId, int gameId);

        /// <summary>
        /// Remove um jogo da biblioteca do usuário de forma assíncrona.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se o jogo foi removido com sucesso, caso contrário, False.</returns>
        Task<bool> RemoveGameFromUserLibraryAsync(int userId, int gameId);
    }
}