using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Home_Security.Context;
public class HomeSecurityContextFactory : IDesignTimeDbContextFactory<HomeSecurityContext>
{
    public HomeSecurityContext CreateDbContext(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        var connectionString = config.GetConnectionString("HomeSecurityContext");
        var optionsBuilder = new DbContextOptionsBuilder<HomeSecurityContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new HomeSecurityContext(optionsBuilder.Options);
    }
}