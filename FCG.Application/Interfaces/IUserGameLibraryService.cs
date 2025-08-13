using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IUserGameLibraryService
    {
        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos dos usuários.
        /// </summary>
        /// <returns>Uma coleção de DTOs de biblioteca de jogos dos usuários.</returns>
        IEnumerable<UserGameLibraryDTO> GetAllUserGameLibraries();

        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos dos usuários de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de biblioteca de jogos dos usuários.</returns>
        Task<IEnumerable<UserGameLibraryDTO>> GetAllUserGameLibrariesAsync();

        /// <summary>
        /// Obtém um registro da biblioteca de jogos do usuário pelo ID do usuário e ID do jogo.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>O DTO da biblioteca de jogos do usuário encontrado ou null.</returns>
        UserGameLibraryDTO GetUserGameLibraryByUserIdAndGameId(int userId, int gameId);

        /// <summary>
        /// Obtém um registro da biblioteca de jogos do usuário pelo ID do usuário e ID do jogo de forma assíncrona.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da biblioteca de jogos do usuário encontrado ou null.</returns>
        Task<UserGameLibraryDTO> GetUserGameLibraryByUserIdAndGameIdAsync(int userId, int gameId);

        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos de um usuário pelo ID do usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <returns>Uma coleção de DTOs de biblioteca de jogos do usuário.</returns>
        IEnumerable<UserGameLibraryDTO> GetUserGameLibrariesByUserId(int userId);

        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos de um usuário pelo ID do usuário de forma assíncrona.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de biblioteca de jogos do usuário.</returns>
        Task<IEnumerable<UserGameLibraryDTO>> GetUserGameLibrariesByUserIdAsync(int userId);

        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos de um jogo pelo ID do jogo.
        /// </summary>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>Uma coleção de DTOs de biblioteca de jogos do jogo.</returns>
        IEnumerable<UserGameLibraryDTO> GetUserGameLibrariesByGameId(int gameId);

        /// <summary>
        /// Obtém todos os registros da biblioteca de jogos de um jogo pelo ID do jogo de forma assíncrona.
        /// </summary>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de biblioteca de jogos do jogo.</returns>
        Task<IEnumerable<UserGameLibraryDTO>> GetUserGameLibrariesByGameIdAsync(int gameId);

        /// <summary>
        /// Adiciona um jogo à biblioteca de um usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>O DTO da biblioteca de jogos do usuário criado.</returns>
        UserGameLibraryDTO AddGameToUserLibrary(int userId, int gameId);

        /// <summary>
        /// Adiciona um jogo à biblioteca de um usuário de forma assíncrona.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO da biblioteca de jogos do usuário criado.</returns>
        Task<UserGameLibraryDTO> AddGameToUserLibraryAsync(int userId, int gameId);

        /// <summary>
        /// Remove um jogo da biblioteca de um usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>True se a remoção foi bem-sucedida, False caso contrário.</returns>
        bool RemoveGameFromUserLibrary(int userId, int gameId);

        /// <summary>
        /// Remove um jogo da biblioteca de um usuário de forma assíncrona.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        /// <param name="gameId">O ID do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém True se a remoção foi bem-sucedida, False caso contrário.</returns>
        Task<bool> RemoveGameFromUserLibraryAsync(int userId, int gameId);
    }
}