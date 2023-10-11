using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEfCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEfCore.Data
{
    public class ApplicationContextCidade : DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strCon = "Server=localhost,1433;Database=DominandoEfCoreDb;User=SA;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False";
            optionsBuilder
                .UseSqlServer(strCon)
                .EnableSensitiveDataLogging()       
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}