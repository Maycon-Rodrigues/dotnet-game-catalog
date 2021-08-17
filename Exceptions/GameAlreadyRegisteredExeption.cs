using System;

namespace GameCatalog.Exceptions
{
    public class GameAlreadyRegisteredExeption : Exception
    {
        public GameAlreadyRegisteredExeption() : base("Game Already Registered"){}
    }
}