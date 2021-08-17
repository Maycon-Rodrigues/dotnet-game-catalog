using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Exceptions;
using GameCatalog.InputModels;
using GameCatalog.Services;
using GameCatalog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Search all games paged
        /// </summary>
        /// <remarks>
        /// Unable to return games without pagination
        /// </remarks>
        /// <param name="page">Indicates which page is being consulted. Minimum 1</param>
        /// <param name="quantity">Indicates the number of retests per page. Minimum 1 and Maximum 50</param>
        /// <response code="200">Return a list of games</response>
        /// <response code="204">If have not games</response>  
        [HttpGet]
        public async Task<ActionResult<GameViewModel>> GetAll(
            [FromQuery, Range(1, int.MaxValue)] int page = 1,
            [FromQuery, Range(1, 50)] int quantity = 5)
        {
            var games = await _gameService.GetAll(page, quantity);

            if (!games.Any())
                return NoContent();

            var pages = (games.Count / quantity).ToString("0.0");

            return Ok(new
            {
                totalPages = pages,
                count = games.Count,
                data = games
            });
        }
        
        /// <summary>
        /// Search game by Id
        /// </summary>
        /// <param name="gameId">Game Id to be fetched</param>
        /// <response code="200">Return fetched game</response>
        /// <response code="204">If have not game</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameViewModel>> GetById([FromRoute] int id)
        {
            var game = await _gameService.GetById(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }

        /// <summary>
        /// Update game on catalog
        /// </summary>
        /// /// <param name="gameId">Game Id to be updated</param>
        /// <param name="gameInputModel">Game data to be updated</param>
        /// <response code="200">If the game is successfully updated</response>
        /// <response code="404">If have not game</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] GameInputModel body)
        {
            try
            {
                await _gameService.Update(id, body);
                return Ok();
            }
            catch (GameIsNotRegistered e)
            {
                return NotFound(new {message = e.Message});
            }
        }

        /// <summary>
        /// Insert a game on catalog
        /// </summary>
        /// <param name="gameInputModel">Game data to be entered</param>
        /// <response code="200">If the game is successfully entered</response>
        /// <response code="422">If there is already a game with the same name for the same producer</response>
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> Create([FromBody] GameInputModel body)
        {
            try
            {
                var game = await _gameService.Create(body);
                return Ok(game);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(new {message = e.Message});
            }
        }

        /// <summary>
        /// Remove game on catalog
        /// </summary>
        /// /// <param name="gameId">Game Id to be removed</param>
        /// <response code="200">If the game removed with success</response>
        /// <response code="404">If have not game</response>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Remove([FromRoute] int id)
        {
            try
            {
                await _gameService.Remove(id);
                return Ok();
            }
            catch (GameIsNotRegistered e)
            {
                return NotFound(new {message = e.Message});
            }
        }
    }
}