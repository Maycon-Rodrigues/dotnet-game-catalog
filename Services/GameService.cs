using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entities;
using GameCatalog.Exceptions;
using GameCatalog.InputModels;
using GameCatalog.Repositories;
using GameCatalog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> GetAll(int page, int quantity)
        {
            var games = await _gameRepository.GetAll(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Producer = game.Producer,
                Price = game.Price
            }).ToList();
        }

        public async Task<GameViewModel> GetById(int id)
        {
            var game = await _gameRepository.GetById(id);

            if (game == null)
                return null;
            
            return new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> GetByNameAndProducer(string title, string producer)
        {
            var game = await _gameRepository.GetByNameAndProducer(title, producer);

            if (game == null)
                throw new GameIsNotRegistered();

            return new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> Create(GameInputModel entity)
        {
            var gameValid = await _gameRepository.GetByNameAndProducer(entity.Title.ToLower(), entity.Producer.ToLower());
            if (gameValid != null)
                throw new GameAlreadyRegisteredExeption();
            
            var game = new Game
            {
                Title = entity.Title,
                Producer = entity.Producer,
                Price = entity.Price
            };

            await _gameRepository.Create(game);

            return new GameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task Update(int id, GameInputModel entity)
        {
            var game = await _gameRepository.GetById(id);

            if (game == null)
                throw new GameIsNotRegistered();

            game.Title = entity.Title;
            game.Producer = entity.Producer;
            game.Price = entity.Price;

            await _gameRepository.Update(game);
        }

        public async Task Remove(int id)
        {
            var game = await _gameRepository.GetById(id);

            if (game == null)
                throw new GameIsNotRegistered();

            await _gameRepository.Remove(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}