using FCG.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IUserGameLibraryRepository
    {
        /// <summary>
        /// Obtém todos os registros da entidade.
        /// </summary>
        /// <returns>Uma coleção de entidades.</returns>
        IEnumerable<UserGameLibrary> GetAll();

        /// <summary>
        /// Obtém todos os registros da entidade de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de entidades.</returns>
        Task<IEnumerable<UserGameLibrary>> GetAllAsync();

        /// <summary>
        /// Obtém um registro da entidade pelo seu identificador.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>A entidade encontrada ou null se não for encontrada.</returns>
        UserGameLibrary GetById(int userId, int gameId);

        /// <summary>
        /// Obtém um registro da entidade pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade encontrada ou null se não for encontrada.</returns>
        Task<UserGameLibrary> GetByIdAsync(int userId, int gameId);

        /// <summary>
        /// Encontra registros da entidade que satisfazem uma condição.
        /// </summary>
        /// <param name="predicate">A condição a ser satisfeita.</param>
        /// <returns>Uma coleção de entidades que satisfazem a condição.</returns>
        IEnumerable<UserGameLibrary> Find(Expression<Func<UserGameLibrary, bool>> predicate);

        /// <summary>
        /// Encontra registros da entidade que satisfazem uma condição de forma assíncrona.
        /// </summary>
        /// <param name="predicate">A condição a ser satisfeita.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de entidades que satisfazem a condição.</returns>
        Task<IEnumerable<UserGameLibrary>> FindAsync(Expression<Func<UserGameLibrary, bool>> predicate);

        /// <summary>
        /// Adiciona um registro da entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada.</param>
        void Add(UserGameLibrary entity);

        /// <summary>
        /// Adiciona um registro da entidade de forma assíncrona.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddAsync(UserGameLibrary entity);

        /// <summary>
        /// Adiciona vários registros da entidade.
        /// </summary>
        /// <param name="entities">As entidades a serem adicionadas.</param>
        void AddRange(IEnumerable<UserGameLibrary> entities);

        /// <summary>
        /// Adiciona vários registros da entidade de forma assíncrona.
        /// </summary>
        /// <param name="entities">As entidades a serem adicionadas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddRangeAsync(IEnumerable<UserGameLibrary> entities);

        /// <summary>
        /// Atualiza um registro da entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada.</param>
        void Update(UserGameLibrary entity);

        /// <summary>
        /// Remove um registro da entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser removida.</param>
        void Remove(UserGameLibrary entity);

        /// <summary>
        /// Remove vários registros da entidade.
        /// </summary>
        /// <param name="entities">As entidades a serem removidas.</param>
        void RemoveRange(IEnumerable<UserGameLibrary> entities);

        /// <summary>
        /// Verifica se existe algum registro da entidade que satisfaz uma condição.
        /// </summary>
        /// <param name="predicate">A condição a ser satisfeita.</param>
        /// <returns>True se existir algum registro que satisfaça a condição, caso contrário, false.</returns>
        bool Any(Expression<Func<UserGameLibrary, bool>> predicate);

        /// <summary>
        /// Verifica se existe algum registro da entidade que satisfaz uma condição de forma assíncrona.
        /// </summary>
        /// <param name="predicate">A condição a ser satisfeita.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é true se existir algum registro que satisfaça a condição, caso contrário, false.</returns>
        Task<bool> AnyAsync(Expression<Func<UserGameLibrary, bool>> predicate);

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de usuários com usuários e jogos.
        /// </summary>
        /// <returns>Uma coleção de registros de biblioteca de jogos de usuários com usuários e jogos.</returns>
        IEnumerable<UserGameLibrary> GetAllWithUsersAndGames();

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de usuários com usuários e jogos de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de registros de biblioteca de jogos de usuários com usuários e jogos.</returns>
        Task<IEnumerable<UserGameLibrary>> GetAllWithUsersAndGamesAsync();

        /// <summary>
        /// Obtém um registro de biblioteca de jogos de usuário pelo identificador do usuário e do jogo.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>O registro de biblioteca de jogos de usuário encontrado ou null.</returns>
        UserGameLibrary GetByUserIdAndGameId(int userId, int gameId);

        /// <summary>
        /// Obtém um registro de biblioteca de jogos de usuário pelo identificador do usuário e do jogo de forma assíncrona.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o registro de biblioteca de jogos de usuário encontrado ou null.</returns>
        Task<UserGameLibrary> GetByUserIdAndGameIdAsync(int userId, int gameId);

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de um usuário específico.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <returns>Uma coleção de registros de biblioteca de jogos do usuário.</returns>
        IEnumerable<UserGameLibrary> GetByUserId(int userId);

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de um usuário específico de forma assíncrona.
        /// </summary>
        /// <param name="userId">O identificador do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de registros de biblioteca de jogos do usuário.</returns>
        Task<IEnumerable<UserGameLibrary>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de um jogo específico.
        /// </summary>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma coleção de registros de biblioteca de jogos do jogo.</returns>
        IEnumerable<UserGameLibrary> GetByGameId(int gameId);

        /// <summary>
        /// Obtém todos os registros de biblioteca de jogos de um jogo específico de forma assíncrona.
        /// </summary>
        /// <param name="gameId">O identificador do jogo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de registros de biblioteca de jogos do jogo.</returns>
        Task<IEnumerable<UserGameLibrary>> GetByGameIdAsync(int gameId);
    }
}