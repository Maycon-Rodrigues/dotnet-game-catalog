using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCatalog.Entities;

namespace GameCatalog.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> GetAll(int page, int quantity);
        Task<Game> GetById(int id);
        Task<Game> GetByNameAndProducer(string title, string producer);
        Task Create(Game entity);
        Task Update(Game entity);
        Task Remove(int id);
    }
}