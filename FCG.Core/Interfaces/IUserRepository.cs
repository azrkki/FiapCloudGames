using FCG.Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Obtém um usuário pelo seu email.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>O usuário encontrado ou null.</returns>
        User GetByEmail(string email);

        /// <summary>
        /// Obtém um usuário pelo seu email de forma assíncrona.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o usuário encontrado ou null.</returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Obtém todos os usuários com suas funções (roles) e biblioteca de jogos.
        /// </summary>
        /// <returns>Uma coleção de usuários com suas funções e biblioteca de jogos.</returns>
        IEnumerable<User> GetAllWithRolesAndGames();

        /// <summary>
        /// Obtém todos os usuários com suas funções (roles) e biblioteca de jogos de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de usuários com suas funções e biblioteca de jogos.</returns>
        Task<IEnumerable<User>> GetAllWithRolesAndGamesAsync();

        /// <summary>
        /// Obtém um usuário pelo seu identificador com sua função (role) e biblioteca de jogos.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>O usuário encontrado com sua função e biblioteca de jogos ou null.</returns>
        User GetByIdWithRoleAndGames(int id);

        /// <summary>
        /// Obtém um usuário pelo seu identificador com sua função (role) e biblioteca de jogos de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o usuário encontrado com sua função e biblioteca de jogos ou null.</returns>
        Task<User> GetByIdWithRoleAndGamesAsync(int id);
    }
}