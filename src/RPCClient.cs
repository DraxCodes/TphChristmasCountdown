using DiscordRPC;
using Serilog;
using System;

namespace BlazeRPC
{
    public class RPCClient
    {
        private readonly DiscordRpcClient _client;
        private readonly ILogger _logger;
        private readonly Config _config;

        public RPCClient(DiscordRpcClient client, Config config)
        {
            _client = client;
            _config = config;
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
            var buttons = new List<Button>();
            
            if (_config.Buttons.Count >= 1)
            {
                foreach (var button in _config.Buttons)
                {
                    buttons.Add(new Button
                    {
                        Label = button.Name,
                        Url = button.Url,
                    });
                }
            }

            _client.SetPresence(new RichPresence()
            {
                Details = _config.Details,
                State = $"Christmas in {DaysUnitilChristmas()}",
                Assets = new Assets()
                {
                    LargeImageKey = _config.LargeImageName,
                    LargeImageText = _config.LargeImageAlt,
                    SmallImageKey = _config.SmallImageName,
                    SmallImageText = _config.SmallImageAlt,
                },
                Buttons = buttons.ToArray(),
            });

            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) =>
            {
                _client.SetPresence(new RichPresence()
                {
                    Details = _config.Details,
                    State = $"Christmas in {DaysUnitilChristmas()}",
                    Assets = new Assets()
                    {
                        LargeImageKey = _config.LargeImageName,
                        LargeImageText = _config.LargeImageAlt,
                        SmallImageKey = _config.SmallImageName,
                        SmallImageText = _config.SmallImageAlt,
                    },
                    Buttons = buttons.ToArray(),
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