using FCG.Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Obtém todas as funções (roles) com seus usuários.
        /// </summary>
        /// <returns>Uma coleção de funções com seus usuários.</returns>
        IEnumerable<Role> GetAllWithUsers();

        /// <summary>
        /// Obtém todas as funções (roles) com seus usuários de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção de funções com seus usuários.</returns>
        Task<IEnumerable<Role>> GetAllWithUsersAsync();

        /// <summary>
        /// Obtém uma função (role) pelo seu identificador com seus usuários.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>A função encontrada com seus usuários ou null.</returns>
        Role GetByIdWithUsers(int id);

        /// <summary>
        /// Obtém uma função (role) pelo seu identificador com seus usuários de forma assíncrona.
        /// </summary>
        /// <param name="id">O identificador da função.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a função encontrada com seus usuários ou null.</returns>
        Task<Role> GetByIdWithUsersAsync(int id);

        /// <summary>
        /// Obtém uma função (role) pelo seu nome.
        /// </summary>
        /// <param name="name">O nome da função.</param>
        /// <returns>A função encontrada ou null.</returns>
        Role GetByName(string name);

        /// <summary>
        /// Obtém uma função (role) pelo seu nome de forma assíncrona.
        /// </summary>
        /// <param name="name">O nome da função.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a função encontrada ou null.</returns>
        Task<Role> GetByNameAsync(string name);
    }
}