using FCG.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Obtém todos os registros da entidade.
        /// </summary>
        /// <returns>Uma coleção de entidades.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Obtém todos os registros da entidade de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtém uma entidade pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador da entidade.</param>
        /// <returns>A entidade encontrada ou null.</returns>
        T GetById(int id);

        /// <summary>
        /// Obtém uma entidade pelo seu identificador de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador da entidade.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade encontrada ou null.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Encontra entidades que correspondem a uma expressão de filtro.
        /// </summary>
        /// <param name="predicate">A expressão de filtro.</param>
        /// <returns>Uma coleção de entidades que correspondem ao filtro.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Encontra entidades que correspondem a uma expressão de filtro de forma assíncrona.
        /// </summary>
        /// <param name="predicate">A expressão de filtro.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de entidades que correspondem ao filtro.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adiciona uma nova entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada.</param>
        void Add(T entity);

        /// <summary>
        /// Adiciona uma nova entidade de forma assíncrona.
        /// </summary>
        /// <param name="entity">A entidade a ser adicionada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adiciona uma coleção de entidades.
        /// </summary>
        /// <param name="entities">As entidades a serem adicionadas.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Adiciona uma coleção de entidades de forma assíncrona.
        /// </summary>
        /// <param name="entities">As entidades a serem adicionadas.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada.</param>
        void Update(T entity);

        /// <summary>
        /// Remove uma entidade.
        /// </summary>
        /// <param name="entity">A entidade a ser removida.</param>
        void Remove(T entity);

        /// <summary>
        /// Remove uma coleção de entidades.
        /// </summary>
        /// <param name="entities">As entidades a serem removidas.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Verifica se existe alguma entidade que corresponde a uma expressão de filtro.
        /// </summary>
        /// <param name="predicate">A expressão de filtro.</param>
        /// <returns>True se existir alguma entidade que corresponda ao filtro, caso contrário, False.</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verifica se existe alguma entidade que corresponde a uma expressão de filtro de forma assíncrona.
        /// </summary>
        /// <param name="predicate">A expressão de filtro.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa é True se existir alguma entidade que corresponda ao filtro, caso contrário, False.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}