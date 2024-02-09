using EntityFrameWorkCore.Data;

// First we need a instance of context.
var context = new FootballLeageDbContext();

//Select all teams.
var teams = context.Teams.ToList();

foreach (var team in teams)
{
    Console.WriteLine(team.Name);
}