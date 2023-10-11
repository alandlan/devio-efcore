// See https://aka.ms/new-console-template for more information
using DominandoEfCore.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

CreateTable(args).Wait();

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

