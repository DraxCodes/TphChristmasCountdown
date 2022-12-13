using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BlazeRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("BlazeRPC.log")
                    .CreateLogger();


                Log.Information("Starting web host");

                var builder = Host.CreateDefaultBuilder(args)
                    .UseSerilog();

                builder.ConfigureServices(
                    services =>
                        services
                            .AddSingleton(new DiscordRpcClient("1012486137148883074"))
                            .AddSingleton<RPCClient>()
                        );

                var host = builder.Build();
                var client = host.Services.GetRequiredService<RPCClient>();
                client.RunSetup();

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}