using Tobias_Padle.Models;

namespace Tobias_Padle.Services
{
    public class TournamentManger
    {
        private readonly PlayersManager _playersManager;
        public TournamentManger(PlayersManager playersManager)
        {
            _playersManager = playersManager;
        }

        public Tournament Tournament { get; private set; }

        public void CreateTournamenet()
        {
            var tournament = new Tournament();
            tournament.Rounds = CreateTournamenetRounds();
            Tournament = tournament;
        }

        private List<Round> CreateTournamenetRounds()
        {
            var roundList = new List<Round>();

            // Number of rounds is n-1 where n is number of players
            var roundCount = _playersManager.PlayersCount - 1;

            for (var i = 1; i <= roundCount; i++)
            {
                var round = CreateRound(i);
                roundList.Add(round);
            }

            return roundList;
        }

        private Round CreateRound(int roundNumber)
        {
            Round round = new Round();
            round.RoundNumber = roundNumber;
            round.Matchs = CreateRoundMatches(roundNumber);

            foreach (var match in round.Matchs)
            {
                Console.WriteLine($"R{roundNumber}M{round.Matchs.IndexOf(match) + 1} {match.HomeTeam.Player1}-{match.HomeTeam.Player2} VS {match.AwayTeam.Player1}-{match.AwayTeam.Player2}");
            }
            Console.WriteLine("\n");
            return round;
        }

        private List<Match> CreateRoundMatches(int roundNumber)
        {
            var matches = new List<Match>();
            var numberOfPlayersPerMatch = 4;
            var numberOfMatchesPerRound = _playersManager.PlayersCount / numberOfPlayersPerMatch;
            // Here i create a new list to be able to manipulate the players to choose from
            // this round without effecting the main players list
            var playersListToChooseFromThisRound = new List<Player>();
            playersListToChooseFromThisRound.AddRange(_playersManager.Players);

            for (var i = 0; i < numberOfMatchesPerRound; i++)
            {
                var match = new Match();
                match.HomeTeam = CreateTeam(playersListToChooseFromThisRound, roundNumber);
                match.AwayTeam = CreateTeam(playersListToChooseFromThisRound, roundNumber);

                matches.Add(match);
            }

            return matches;
        }

        private Team CreateTeam(List<Player> playersListToChooseFromThisRound, int roundNumber)
        {
            var team = new Team();
            team.Player1 = GetRandomPlayer1(playersListToChooseFromThisRound, roundNumber);
            team.Player2 = GetRandomPlayer2(team.Player1, playersListToChooseFromThisRound, roundNumber);
            return team;
        }

        private Player GetRandomPlayer1(List<Player> playersListToChooseFromThisRound, int roundNumber)
        {
            var playersToChooseFrom = new List<Player>();
            playersToChooseFrom.AddRange(playersListToChooseFromThisRound.Where(p => p.AlreadyPlayedWithPlayers.Count < roundNumber));

            var random = new Random(Guid.NewGuid().GetHashCode());
            var randomPlayerIndex = random.Next(playersToChooseFrom.Count - 1);
            var randomPlayerId = playersToChooseFrom[randomPlayerIndex].Id;

            var playerFromRoundList = playersListToChooseFromThisRound.First(p => p.Id == randomPlayerId);
            playersListToChooseFromThisRound.Remove(playerFromRoundList);

            var player = _playersManager.Players.First(p => p.Id == randomPlayerId);
            return player;
        }

        private Player GetRandomPlayer2(Player player1, List<Player> playersListToChooseFromThisRound, int roundNumber)
        {
            var playersListWithoutPlayersThatPlayer1PlayedWith = playersListToChooseFromThisRound
                .Where(p => p.AlreadyPlayedWithPlayers.Count < roundNumber)
                .Where(p => !player1.AlreadyPlayedWithPlayers.Contains(p))
                .ToList();

            var random = new Random(Guid.NewGuid().GetHashCode());
            var randomPlayerIndex = random.Next(playersListWithoutPlayersThatPlayer1PlayedWith.Count - 1);
            var randomPlayerId = playersListWithoutPlayersThatPlayer1PlayedWith[randomPlayerIndex].Id;

            var playerFromCurrentRoundList = playersListToChooseFromThisRound.First(p => p.Id == randomPlayerId);
            playersListToChooseFromThisRound.Remove(playerFromCurrentRoundList);

            var player2 = _playersManager.Players.First(p => p.Id == randomPlayerId);
            player1.AlreadyPlayedWithPlayers.Add(player2);
            player2.AlreadyPlayedWithPlayers.Add(player1);

            return player2;
        }
    }
}
