using System;

namespace GameCatalog.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Producer { get; set; }
        public double Price { get; set; }
    }
}