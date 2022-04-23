using Tobias_Padle.Models;

namespace Tobias_Padle.Services
{
    public class PlayersManager
    {
        public IEnumerable<Player> Players { get; private set; }
        public int PlayersCount { get; private set; }

        public PlayersManager()
        {
            PlayersCount = 12;
            Players = CreatePlayers();
        }

        private List<Player> CreatePlayers()
        {
            var players = new List<Player>();
            for (int i = 1; i <= PlayersCount; i++)
            {
                players.Add(new Player
                {
                    Id = i,
                    Name = "Player " + i.ToString(),
                });
            }

            return players;
        }
    }
}
