using Tobias_Padle.Services;

// Players manager skapar en lista av 12 spelare samt sortera dem på random. 
var playersManager = new PlayersManager();

// Här är alla logik för skapa 66 olika par som splear i 11 rundor med 3 matcher var
var tournamentManager = new TournamentManger(playersManager);

tournamentManager.CreateTournamenet();

Console.ReadLine();