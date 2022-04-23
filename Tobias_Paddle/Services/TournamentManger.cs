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
            tournament.Rounds = CreateTournamenetRounds(_playersManager.Players);
            Tournament = tournament;
        }

        public List<Round> CreateTournamenetRounds(IList<Player> players)
        {
            var rounds = new List<Round>();

            // Här kommer det vara en lista av alla möjliga par de kommer innehålla vilket runda det är, samt paret
            // Tuple av (roundNumber, Player1, Player2)
            var pairs = new List<(int, Player, Player)>();
            if (players == null || players.Count < 2)
            {
                return rounds;
            }

            var restTeams = new List<Player>(players.Skip(1));
            var teamsCount = players.Count;

            // Jag erkänner att inte förstår allt som händer här, men länkar nere till en Wikipeida om det så du kanske förstår mer än mig. 
            // Koden hittade jag på stacköverflow och anpassade i slutet för att passa mina modeller
            for (var round = 0; round < teamsCount - 1; round++)
            {
                if (restTeams[round % restTeams.Count]?.Equals(default) == false)
                {
                    pairs.Add((round, players[0], restTeams[round % restTeams.Count]));
                }

                for (var index = 1; index < teamsCount / 2; index++)
                {
                    var firstTeam = restTeams[(round + index) % restTeams.Count];
                    var secondTeam = restTeams[(round + restTeams.Count - index) % restTeams.Count];
                    if (firstTeam?.Equals(default) == false && secondTeam?.Equals(default) == false)
                    {
                        pairs.Add((round, firstTeam, secondTeam));
                    }
                }
            }

            // Här loopar jag genom alla par som skapades men jag gruppera dem enligt första värdet (roundanummer)
            // där får man 6 par med varje gruppering dvs 6 lag vilker resultera i 3 matcher
            foreach (var tournamentRound in pairs.GroupBy(p => p.Item1))
            {
                var round = new Round();
                round.RoundNumber = tournamentRound.Key + 1;
                round.Matchs = new List<Match>();

                for (int i = 0; i < players.Count / 4; i++)
                {
                    var match = new Match();
                    var teams = tournamentRound.Skip(i * 2).Take(2).Select(p => new Team { Player1 = p.Item2, Player2 = p.Item3 }).ToList();
                    match.HomeTeam = teams[0];
                    match.AwayTeam = teams[1];
                    round.Matchs.Add(match);
                }

                foreach (var match in round.Matchs)
                {
                    Console.WriteLine($"R{round.RoundNumber}M{round.Matchs.IndexOf(match) + 1} {match.HomeTeam.Player1}-{match.HomeTeam.Player2} VS {match.AwayTeam.Player1}-{match.AwayTeam.Player2}");
                }
                Console.WriteLine("\n");

                rounds.Add(round);
            }
            return rounds;
        }

        // Man kan använda round robin algoritm för lösa detta problem
        // https://en.wikipedia.org/wiki/Round-robin_tournament
        // https://stackoverflow.com/a/60059572
        // https://sv.wikipedia.org/wiki/Modul%C3%A4r_aritmetik
    }
}
