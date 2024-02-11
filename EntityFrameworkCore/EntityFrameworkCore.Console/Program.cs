using EntityFrameWorkCore.Data;
using EntityFrameWorkCore.Domain;
using Microsoft.EntityFrameworkCore;

// First we need a instance of context.
using var context = new FootballLeageDbContext();

#region Using Entity Framework core to query a database.

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

//Skip and take
async Task SkipAndTake()
{
    var recordCount = 3;
    var page = 0;
    var teams = await context
        .Teams
        .Skip(page * recordCount)
        .Take(recordCount)
        .ToListAsync();
}

//Projections and custom types
async Task ProjectionsAndCustomTypes()
{
    var teamsNames = await context.Teams
        .Select(q => q.Name)
        .ToListAsync();

    var projectedTeams2 = await context.Teams
        .Select(q => new { q.Name, q.CreatedDate })
        .ToListAsync();

    var projectedTeams3 = await context.Teams
        .Select(q => new TeamInfo { Name = q.Name, TeamId = q.Id })
        .ToListAsync();

}

//Tracking vs No Tracking
async Task Tracking()
{
    var teams = await context.Teams
        .AsNoTracking()
        .ToListAsync();

    var teamsTracking = await context.Teams
        .AsTracking()
        .ToListAsync();
}

#endregion

#region Using Entity Framework Core to Manipulate Data

//Insert
async Task InsertQueries()
{

    //Simple
    var newCoach = new Coach
    {
        Name = "Jose Mourinho",
        CreatedDate = DateTime.Now
    };

    await context.Coaches.AddAsync(newCoach);
    await context.SaveChangesAsync();

    //Loop
    var newCoach1 = new Coach
    {
        Name = "Jose Mourinho",
        CreatedDate = DateTime.Now
    };

    List<Coach> coaches = new List<Coach> { newCoach, newCoach1 };

    foreach (var item in coaches)
    {
        await context.Coaches.AddAsync(item);
    }
    Console.WriteLine(context.ChangeTracker.DebugView);
    await context.SaveChangesAsync();
    Console.WriteLine(context.ChangeTracker.DebugView);


    //Batch
    await context.Coaches.AddRangeAsync(coaches);
    await context.SaveChangesAsync();
}

//Update
async Task UpdateOperation()
{
    //When tracking is enabled:
    //(FindAsync needs tracking)
    var coach = await context.Coaches.FindAsync(9);
    coach.Name = "Trevoir Williams";
    coach.CreatedDate = DateTime.Now;
    await context.SaveChangesAsync();

    //When no tracking is enabled:
    var coachNoTrack = await context.Coaches
        .AsNoTracking()
        .FirstOrDefaultAsync(q => q.Id == 9);
    coachNoTrack.Name = "No tracking";
    //Update option #1
    context.Update(coachNoTrack);

    //Update option #2
    context.Entry(coachNoTrack).State = EntityState.Modified;

    await context.SaveChangesAsync();
}

//Delete
async Task DeleteOperation()
{
    var coach = await context.Coaches.FindAsync(9);
    context.Remove(coach);
    await context.SaveChangesAsync();
}

//ExecuteUpdate and ExecuteDelete operations
async Task ExecuteOperations()
{
    /* Execute delete */
    //This operation executes immediately against the DB,
    //rather than being deferred until SaveChanges() is called.
    await context.Coaches
        .Where(q => q.Name == "Theodore Whitmore")
        .ExecuteDeleteAsync();

    /* Execute Update */
    await context.Coaches
        .Where(q => q.Name == "Jose Mourinho")
        .ExecuteUpdateAsync(set => set
            .SetProperty(prop => prop.Name, "Pep Guardiola")
            .SetProperty(prop => prop.CreatedDate, DateTime.Now)
         );
}
#endregion

#region Interacting with related records

//Insert record with FK
async Task RecordWithFK()
{
    var match = new Match
    {
        AwayTeamId = 1,
        HomeTeamId = 2,
        HomeTeamScore = 0,
        AwayTeamScore = 0,
        Date = new DateTime(2024, 10, 1),
        TicketPrice = 20
    };

    await context.AddAsync(match);
    await context.SaveChangesAsync();
}

//Insert Parent/Child
async Task parentChild()
{
    var coah = new Coach
    {
        Name = "Hohnson"
    };

    var team = new Team
    {
        Name = "New team",
        Coach = coah
    };

    var team2 = new Team
    {
        Name = "New team",
        Coach = new Coach
        {
            Name = "Johnson"
        }
    };

    await context.AddAsync(team);
    await context.SaveChangesAsync();
}

// Insert parent with children
async Task parentWithChildren()
{
    var league = new League
    {
        Name = "New Leage",
        Teams = new List<Team>
        {
            new Team
            {
                Name = "Juventus",
                Coach = new Coach
                {
                    Name = "Juve Coach"
                }
            },
            new Team
            {
                Name = "AC Milan",
                Coach = new Coach
                {
                    Name = "Milan Coach"
                }
            }
        }
    };
    await context.AddAsync(league);
    await context.SaveChangesAsync();
}

#endregion