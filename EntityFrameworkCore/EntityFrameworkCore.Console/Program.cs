using EntityFrameWorkCore.Data;
using Microsoft.EntityFrameworkCore;

// First we need a instance of context.
using var context = new FootballLeageDbContext();

//Select all teams.
//GetAllTeams();

//Select a single record.
GetSingleRecord();

async void GetSingleRecord()
{
    //First one in the list
    var teamOne = await context.Teams.FirstAsync();//Throws an error if no data is founded.
    var teamOneDefault = await context.Teams.FirstOrDefaultAsync();//Doesnt throw any error.

    //First one in the list that meets a condition
    var team2 = await context.Teams.FirstAsync(team => team.Id == 1);
    var team2Default = await context.Teams.FirstOrDefaultAsync(team => team.Id == 1);

    //Only one record should be returned
    var team3 = await context.Teams.SingleAsync();
    var team3Condition = await context.Teams.SingleAsync(team => team.Id == 2);
    var team3ConditionDefault = await context.Teams.SingleOrDefaultAsync(team => team.Id == 2);

    // Selecting based on Id
    var team4 = await context.Teams.FindAsync(5);
}

void GetAllTeams()
{
    var teams = context.Teams.ToList();

    foreach (var team in teams)
    {
        Console.WriteLine(team.Name);
    }
}
