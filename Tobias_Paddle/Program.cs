using Tobias_Padle.Services;

var playersManager = new PlayersManager();
var tournamentManager = new TournamentManger(playersManager);

tournamentManager.CreateTournamenet();


var tor = tournamentManager.Tournament;

Console.ReadLine();