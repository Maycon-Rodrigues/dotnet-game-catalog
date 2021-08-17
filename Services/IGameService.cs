using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCatalog.InputModels;
using GameCatalog.ViewModels;
using GameCatalog.ViewModels;

namespace GameCatalog.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> GetAll(int page, int quantity);
        Task<GameViewModel> GetById(int id);
        Task<GameViewModel> GetByNameAndProducer(string title, string producer);
        Task<GameViewModel> Create(GameInputModel entity);
        Task Update(int id, GameInputModel entity);
        Task Remove(int id);
    }
}