using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Data.Constants;

namespace TestSolution.Data.EF
{
    public class NTSolutionDbContextFactory : IDesignTimeDbContextFactory<NTDbContext>
    {
        public NTDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var connectionString = configuration.GetConnectionString(SystemConstants.ConnectionString);
                
            var optionsBuilder = new DbContextOptionsBuilder<NTDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NTDbContext(optionsBuilder.Options);
        }
    }
}
