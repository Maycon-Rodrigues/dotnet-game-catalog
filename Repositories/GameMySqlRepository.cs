using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCatalog.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace GameCatalog.Repositories
{
    public class GameMySqlRepository : IGameRepository
    {
        private readonly MySqlConnection _sqlConnection;

        public GameMySqlRepository(IConfiguration configuration)
        {
            _sqlConnection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Game>> GetAll(int page, int quantity)
        {
            var games = new List<Game>();
            var query = $"SELECT * FROM Games ORDER BY id LIMIT {(quantity)} OFFSET {((page - 1) * quantity)};";

            var query2 = $@"SELECT
                           (SELECT COUNT(*) FROM Games)/10 AS total
                           FROM Games LIMIT 0, 10";
            
            await _sqlConnection.OpenAsync();
            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync();

            while (mySqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (int)mySqlDataReader["Id"],
                    Title = (string)mySqlDataReader["Title"],
                    Producer = (string)mySqlDataReader["Producer"],
                    Price = (double)mySqlDataReader["Price"]
                });
            }

            await _sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> GetById(int id)
        {
            Game game = null;
            var query = $@"SELECT * FROM Games WHERE Id = '{id}';";

            await _sqlConnection.OpenAsync();

            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync();

            while(mySqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (int) mySqlDataReader["Id"],
                    Title = (string) mySqlDataReader["Title"],
                    Producer = (string) mySqlDataReader["Producer"],
                    Price = (double) mySqlDataReader["Price"]
                };
            };

            await _sqlConnection.CloseAsync();

            return game;
        }

        public async Task<Game> GetByNameAndProducer(string title, string producer)
        {
            Game game = null;
            var query = $@"SELECT * FROM Games WHERE Title = '{title.ToLower()}' AND Producer = '{producer.ToLower()}';";

            await _sqlConnection.OpenAsync();

            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            var mySqlDataReader = await mySqlCommand.ExecuteReaderAsync();

            while (mySqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (int) mySqlDataReader["Id"],
                    Title = (string) mySqlDataReader["Title"],
                    Producer = (string) mySqlDataReader["Producer"],
                    Price = (double) mySqlDataReader["Price"]
                };
            }

            await _sqlConnection.CloseAsync();

            return game;
        }

        public async Task Create(Game entity)
        {
            var query = $@"INSERT INTO Games (Title, Producer, Price) VALUES (
                         '{entity.Title}', '{entity.Producer}',
                         '{entity.Price.ToString().Replace(",", ".")}'
                        );";
            
            await _sqlConnection.OpenAsync();

            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _sqlConnection.CloseAsync();
        }

        public async Task Update(Game entity)
        {
            var query = $@"UPDATE Games SET Title = '{entity.Title}',
                        Producer = '{entity.Producer}',
                        Price = '{entity.Price.ToString().Replace(",", ".")}'
                        WHERE Id = '{entity.Id}';";

            await _sqlConnection.OpenAsync();

            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _sqlConnection.CloseAsync();
        }

        public async Task Remove(int id)
        {
            var query = $@"DELETE FROM Games WHERE Id = '{id}';";

            await _sqlConnection.OpenAsync();

            var mySqlCommand = new MySqlCommand(query, _sqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            _sqlConnection?.Close();
            _sqlConnection?.Dispose();
        }
    }
}