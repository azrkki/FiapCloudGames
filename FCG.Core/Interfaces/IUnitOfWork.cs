using System;
using System.Threading.Tasks;

namespace FCG.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Salva todas as alterações feitas no contexto no banco de dados.
        /// </summary>
        /// <returns>O número de entidades gravadas no banco de dados</returns>
        int SaveChanges();

        /// <summary>
        /// Salva todas as alterações feitas no contexto no banco de dados de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de salvamento assíncrono. O resultado da tarefa contém o número de entidades gravadas no banco de dados.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia uma nova transação.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Confirma a transação atual.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Reverte a transação atual.
        /// </summary>
        void RollbackTransaction();
    }
}