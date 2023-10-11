// See https://aka.ms/new-console-template for more information
using DominandoEfCore.Data;
using DominandoEfCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

//HealthCheck(args).Wait();

//CreateTable(args).Wait();

// new DominandoEfCore.Data.ApplicationContext().Departamentos.AsNoTracking().Any();
// GenerateStateConnection(true).Wait();
// GenerateStateConnection(false).Wait();

//ExecuteSql().Wait();

// SqlInjection().Wait();

//PendiongMigrations().Wait();

//AplicationMigration().Wait();

AllMigrations().Wait();

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

static async Task ExecuteSql(){
    await using var db = new ApplicationContext();
    await db.Database.ExecuteSqlRawAsync("SELECT 1");

    await db.Database.ExecuteSqlInterpolatedAsync($"SELECT 1");
}

static async Task SqlInjection(){
    using var db = new ApplicationContext();
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    await db.Departamentos.AddRangeAsync(
        new Departamento { Descricao = "Departamento 01" },
        new Departamento { Descricao = "Departamento 02" }
    );

    await db.SaveChangesAsync();

    var departamentoAlterado = "Teste ' or 1='1";
    await db.Database.ExecuteSqlRawAsync($"UPDATE Departamentos SET Descricao = 'AtaqueSqlInjection' WHERE Descricao = '0'", departamentoAlterado);

    foreach (var departamento in db.Departamentos.AsNoTracking())
    {
        Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
    }
}

static async Task PendiongMigrations(){
    using var db = new ApplicationContext();

    var migracoesPendentes = await db.Database.GetPendingMigrationsAsync();

    Console.WriteLine($"Total: {migracoesPendentes.Count()}");

    foreach (var migracao in migracoesPendentes)
    {
        Console.WriteLine($"Migração: {migracao}");
    }
}

static async Task AplicationMigration(){
    using var db = new ApplicationContext();

    await db.Database.MigrateAsync();
}

static async Task AllMigrations(){
    using var db = new ApplicationContext();

    var migracoes = db.Database.GetMigrations();

    Console.WriteLine($"Total: {migracoes.Count()}");

    foreach (var migracao in migracoes)
    {
        Console.WriteLine($"Migração: {migracao}");
    }
}