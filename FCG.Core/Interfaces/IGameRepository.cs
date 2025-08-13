using FCG.Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// Obtém todos os jogos com seus usuários.
        /// </summary>
        /// <returns>Uma coleção de jogos com seus usuários.</returns>
        IEnumerable<Game> GetAllWithUsers();

        /// <summary>
        /// Obtém todos os jogos com seus usuários de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de jogos com seus usuários.</returns>
        Task<IEnumerable<Game>> GetAllWithUsersAsync();

        /// <summary>
        /// Obtém um jogo pelo seu identificador com seus usuários.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>O jogo encontrado com seus usuários ou null.</returns>
        Game GetByIdWithUsers(int id);

        /// <summary>
        /// Obtém um jogo pelo seu identificador com seus usuários de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o jogo encontrado com seus usuários ou null.</returns>
        Task<Game> GetByIdWithUsersAsync(int id);

        /// <summary>
        /// Obtém todos os jogos com desconto.
        /// </summary>
        /// <returns>Uma coleção de jogos com desconto.</returns>
        IEnumerable<Game> GetGamesOnSale();

        /// <summary>
        /// Obtém todos os jogos com desconto de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de jogos com desconto.</returns>
        Task<IEnumerable<Game>> GetGamesOnSaleAsync();
    }
}