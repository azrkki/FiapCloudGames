using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IGameService
    {
        /// <summary>
        /// Obtém todos os jogos.
        /// </summary>
        /// <returns>Uma coleção de DTOs de jogos.</returns>
        IEnumerable<GameDTO> GetAllGames();

        /// <summary>
        /// Obtém todos os jogos de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de jogos.</returns>
        Task<IEnumerable<GameDTO>> GetAllGamesAsync();

        /// <summary>
        /// Obtém um jogo pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>O DTO do jogo encontrado ou null.</returns>
        GameDTO GetGameById(int id);

        /// <summary>
        /// Obtém um jogo pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo encontrado ou null.</returns>
        Task<GameDTO> GetGameByIdAsync(int id);

        /// <summary>
        /// Obtém todos os jogos com desconto.
        /// </summary>
        /// <returns>Uma coleção de DTOs de jogos com desconto.</returns>
        IEnumerable<GameDTO> GetGamesOnSale();

        /// <summary>
        /// Obtém todos os jogos com desconto de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de DTOs de jogos com desconto.</returns>
        Task<IEnumerable<GameDTO>> GetGamesOnSaleAsync();

        /// <summary>
        /// Cria um novo jogo.
        /// </summary>
        /// <param name="gameDto">O DTO do jogo a ser criado.</param>
        /// <returns>O DTO do jogo criado.</returns>
        GameDTO CreateGame(GameCreateDTO gameDto);

        /// <summary>
        /// Cria um novo jogo de forma assíncrona.
        /// </summary>
        /// <param name="gameDto">O DTO do jogo a ser criado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo criado.</returns>
        Task<GameDTO> CreateGameAsync(GameCreateDTO gameDto);

        /// <summary>
        /// Atualiza um jogo existente.
        /// </summary>
        /// <param name="gameDto">O DTO do jogo a ser atualizado.</param>
        /// <returns>O DTO do jogo atualizado.</returns>
        GameDTO UpdateGame(GameUpdateDTO gameDto);

        /// <summary>
        /// Atualiza um jogo existente de forma assíncrona.
        /// </summary>
        /// <param name="gameDto">O DTO do jogo a ser atualizado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo atualizado.</returns>
        Task<GameDTO> UpdateGameAsync(GameUpdateDTO gameDto);

        /// <summary>
        /// Exclui um jogo pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>True se o jogo foi excluído com sucesso, caso contrário, False.</returns>
        bool DeleteGame(int id);

        /// <summary>
        /// Exclui um jogo pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se o jogo foi excluído com sucesso, caso contrário, False.</returns>
        Task<bool> DeleteGameAsync(int id);

        /// <summary>
        /// Aplica um desconto a um jogo.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <param name="discountPercentage">O percentual de desconto a ser aplicado.</param>
        /// <returns>O DTO do jogo atualizado.</returns>
        GameDTO ApplyDiscount(int id, int discountPercentage);

        /// <summary>
        /// Aplica um desconto a um jogo de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <param name="discountPercentage">O percentual de desconto a ser aplicado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo atualizado.</returns>
        Task<GameDTO> ApplyDiscountAsync(int id, int discountPercentage);

        /// <summary>
        /// Remove o desconto de um jogo.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>O DTO do jogo atualizado.</returns>
        GameDTO RemoveDiscount(int id);

        /// <summary>
        /// Remove o desconto de um jogo de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo atualizado.</returns>
        Task<GameDTO> RemoveDiscountAsync(int id);

        /// <summary>
        /// Atualiza o status de venda de um jogo.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <param name="isOnSale">O novo status de venda.</param>
        /// <returns>O DTO do jogo atualizado.</returns>
        GameDTO UpdateSaleStatus(int id, bool isOnSale);

        /// <summary>
        /// Atualiza o status de venda de um jogo de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador do jogo.</param>
        /// <param name="isOnSale">O novo status de venda.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o DTO do jogo atualizado.</returns>
        Task<GameDTO> UpdateSaleStatusAsync(int id, bool isOnSale);
    }
}