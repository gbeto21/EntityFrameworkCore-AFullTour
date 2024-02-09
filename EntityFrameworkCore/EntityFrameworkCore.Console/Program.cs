using EntityFrameWorkCore.Data;
using Microsoft.EntityFrameworkCore;

// First we need a instance of context.
using var context = new FootballLeageDbContext();

//Select all teams.
//await GetAllTeams();
async Task GetAllTeams()
{
    var teams = await context.Teams.ToListAsync();

    foreach (var team in teams)
    {
        Console.WriteLine(team.Name);
    }
}

//Select a single record.
//await GetSingleRecord();

async Task GetSingleRecord()
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

//Fillters to Queries
//await Filtering();

async Task Filtering()
{
    var teamsFiltered = await context.Teams
        .Where(q => q.Name == "Tivoli Gardens FC")
        .ToListAsync();

    Console.WriteLine("Enter search term");
    var desiredTeam = Console.ReadLine();

    var partialMatches = await context
        .Teams
        .Where(q => q.Name.Contains(desiredTeam))
        .ToListAsync();

    var partialMatchesTwo = await context
        .Teams
        .Where(q => EF.Functions.Like(q.Name, $"%{desiredTeam}%"))
        .ToListAsync();

}

//Aggregate Methods
//await AgregateMethods();

async Task AgregateMethods()
{
    //Count
    var numberOfTeams = await context.Teams.CountAsync();
    var numberOfTeamsFiltered = await context.Teams.CountAsync(q => q.Id == 1);

    //Max
    var maxTeams = await context.Teams.MaxAsync(q => q.Id);

    //Min
    var minTeams = await context.Teams.MinAsync(q => q.Id);

    //Average
    var aveTeams = await context.Teams.AverageAsync(q => q.Id);

    //Sum
    var sumTeams = await context.Teams.SumAsync(q => q.Id);

}


//Group by

async Task Grouping()
{

    var groupedTeams = await context.Teams
        //.Where(q => q.Name == "")  -> Translates to a WHERE clause
        .GroupBy(q => q.CreatedDate.Date).ToListAsync();
    //.Where(q => ); // -> Translates to a HAVING clause

    var groupedTeamsSomeGroups = context.Teams
        .GroupBy(q => new { q.CreatedDate.Date, q.Name });

    foreach (var group in groupedTeams)
    {
        Console.WriteLine(group.Key);
        foreach (var item in group)
        {
            Console.WriteLine(group.Key);
        }
    }
}

//Order by
async Task OrderByData()
{
    var orderedTeams = await context.Teams
        .OrderBy(q => q.Name)
        .ToListAsync();

    var orderedTeamsDes = await context.Teams
        .OrderByDescending(q => q.Name)
        .ToListAsync();

    //Gettint the record with a maximum/minimun value.
    var maxBy = context.Teams.MaxBy(q => q.Id);
    var minBy = context.Teams.MinBy(q => q.Id);

}