using DiscordRPC;
using Serilog;
using System;

namespace BlazeRPC
{
    public class RPCClient
    {
        private readonly DiscordRpcClient _client;
        private readonly ILogger _logger;

        public RPCClient(DiscordRpcClient client)
        {
            _client = client;
            _logger = Log.ForContext<RPCClient>();

            SetupEvents();
        }

        private void SetupEvents()
        {
            _client.OnReady += (sender, e) =>
            {
                _logger.Information("Received Ready from user {0}", e.User.Username);
            };

            _client.OnPresenceUpdate += (sender, e) =>
            {
                _logger.Information("Received Update! {0}", e.Presence);
            };

        }

        public void RunSetup()
        {
            _logger.Information("Client Initializing...");
            _client.Initialize();
            _logger.Information("Client Initialized: {0}", _client.IsInitialized);

            _logger.Information("Setting up buttons");

            _client.SetPresence(new RichPresence()
            {
                Details = "Member @ TPH",
                State = $"Christmas in {DaysUnitilChristmas()}",
                Assets = new Assets()
                {
                    LargeImageKey = "tph_christmas",
                    LargeImageText = "TPH Christmas Logo",
                    SmallImageKey = "blaze",
                    SmallImageText = "Blaze Logo",
                },
                Buttons = new Button[]
                {
                    new Button { Label= "Join The Programmers Hangout", Url = "https://discord.gg/programming"},
                    new Button { Label = "View The Website", Url = "https://theprogrammershangout.com/"}
                },
            });

            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) =>
            {
                _client.SetPresence(new RichPresence()
                {
                    Details = "Member @ TPH",
                    State = $"Christmas in {DaysUnitilChristmas()}",
                    Assets = new Assets()
                    {
                        LargeImageKey = "tph_christmas",
                        LargeImageText = "TPH Christmas Logo",
                        SmallImageKey = "blaze",
                        SmallImageText = "Blaze Logo",
                    },
                    Buttons = new Button[]
                    {
                        new Button { Label= "Join The Programmers Hangout", Url = "https://discord.gg/programming"},
                        new Button { Label = "View The Website", Url = "https://theprogrammershangout.com/"}
                    },
                });
            };

            timer.Start();

        }

        private string DaysUnitilChristmas()
        {
            var christmasDay = new DateTime(DateTime.UtcNow.Year, 12, 25, 0, 0, 0);
            var timeLeft = christmasDay - DateTime.UtcNow;
            return $"{timeLeft.Days} days {timeLeft.ToString(@"hh\:mm\:ss")}";
        }
    }
}