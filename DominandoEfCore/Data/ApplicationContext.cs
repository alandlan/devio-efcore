using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEfCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEfCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strCon = "Server=localhost,1433;Database=DominandoEfCoreDb;User=SA;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False;Pooling=true;";
            optionsBuilder
                .UseSqlServer(strCon)
                .EnableSensitiveDataLogging()       
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}