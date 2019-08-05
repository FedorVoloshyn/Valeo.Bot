using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValeoBot.Configuration.Entities;

namespace ValeoBot.Models
{
    public class ValeoLifeBotLP : IBot
    {
        private const string BaseUrl = "https://api.telegram.org/bot";
        public ValeoLifeBotLP(ITelegramBotClient client, string username)
        {
            this.Client = client;
            this.Username = username;

        }
        public ITelegramBotClient Client { get; }

        public string Username { get; }

        public ValeoLifeBotLP(IOptions<BotOptions<ValeoLifeBot>> options, IOptions<BotConfig> botConfig)
        {
            Username = options.Value.Username;
            if (!string.IsNullOrEmpty(botConfig.Value.Proxy))
            {
                WebProxy proxy = new WebProxy("http://proxy.isd.dp.ua:8080/", true);
                //Client = new TelegramBotClient(options.Value.ApiToken, proxy);
                string _baseRequestUrl = $"{BaseUrl}{options.Value.ApiToken}/";
                try
                {
                    WebProxy webProxy = new WebProxy("http://proxy.isd.dp.ua:8080/", true);
                    //Update[] updates = await Client.MakeRequestAsync(requestParams, cancellationToken);

                    var httpClientHander = new HttpClientHandler
                    {
                        Proxy = webProxy,
                        UseProxy = true
                    };
                    var _httpClient = new HttpClient(httpClientHander);

                    var request = new GetUpdatesRequest
                    {
                        Offset = 0,
                        Timeout = 500,
                        AllowedUpdates = new UpdateType[0],
                    };
                    string url = _baseRequestUrl + request.MethodName;
                    var httpRequest = new HttpRequestMessage(request.Method, "https://www.instagram.com")
                    {
                        Content = request.ToHttpContent()
                    };

                    var httpResponse = _httpClient.SendAsync(httpRequest).Result;
                    Console.Write(httpResponse.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {

                }

            }
            else
            {
                Client = new TelegramBotClient(options.Value.ApiToken);
            }

        }

        private async Task RunAsync(GetUpdatesRequest requestParams = default,
            CancellationToken cancellationToken = default)
        {
            requestParams = requestParams ?? new GetUpdatesRequest
            {
            Offset = 0,
            Timeout = 500,
            AllowedUpdates = new UpdateType[0],
            };

        }

    }
}