using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

//using Autofac.Extensions.DependencyInjection;

namespace Dcf.Wwp.Api
{
  public class Program
  {
    public static Version Version { get; } = new Version(2, 2, 2, 25);

    public static void Main(string[] args)
    {
      var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("hosting.json", optional: true)
                   .Build();

      var host = new WebHostBuilder()
                 .UseKestrel()
                 //.ConfigureServices(services => services.AddAutofac())
                 .UseConfiguration(config)
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .UseIISIntegration()
                 .UseStartup<Startup>()
                 //.UseStartup<StartupWithConfigureContainer>()
                 .Build();

      host.Run();
    }
  }
}
