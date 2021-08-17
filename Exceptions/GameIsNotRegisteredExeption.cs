using System;

namespace GameCatalog.Exceptions
{
    public class GameIsNotRegistered : Exception
    {
        public GameIsNotRegistered() : base("Unregistered Game") {}
    }
}