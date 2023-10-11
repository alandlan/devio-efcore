// See https://aka.ms/new-console-template for more information
using DominandoEfCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

// HealthCheck(args).Wait();

//CreateTable(args).Wait();


new DominandoEfCore.Data.ApplicationContext().Departamentos.AsNoTracking().Any();
GenerateStateConnection(true).Wait();
GenerateStateConnection(false).Wait();

static async Task CreateTable(string[] args)
{
    await using var db = new ApplicationContext();
    await using var db2 = new ApplicationContextCidade();
    // await db.Database.EnsureDeletedAsync();

    await db.Database.EnsureCreatedAsync();
    await db2.Database.EnsureCreatedAsync();

    var generator = db2.GetService<IRelationalDatabaseCreator>();
    generator.CreateTables();

    //await db.Database.MigrateAsync();
}

static async Task HealthCheck(string[] args)
{
    await using var db = new ApplicationContext();
    var canConnect = await db.Database.CanConnectAsync();
    if (canConnect)
    {
        Console.WriteLine("Can connect");
    }
    else
    {
        Console.WriteLine("Can't connect");
    }
}

static async Task GenerateStateConnection(bool state){
    var _count = 0;
    await using var db = new ApplicationContext();
    var time = System.Diagnostics.Stopwatch.StartNew();

    var conection = db.Database.GetDbConnection();
    conection.StateChange += (_, _) => ++ _count;
    if(state){
        conection.Open();
    }

    for (int i = 0; i < 200; i++)
    {
        db.Departamentos.AsNoTracking().Any();
    }

    time.Stop();
    var mensagem = $"Tempo: {time.Elapsed.ToString()}, State: {state.ToString()}, Count: {_count}";

    Console.WriteLine(mensagem);
}