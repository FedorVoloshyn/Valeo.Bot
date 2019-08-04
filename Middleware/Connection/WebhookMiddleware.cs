﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ValeoBot.Configuration.Entities;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace ValeoBot.Middleware.Connection
{
    static class WebhookMiddleware
    {
    //     public static IApplicationBuilder UseTelegramBotWebhook<TBot>(
    //         this IApplicationBuilder app,
    //         IBotBuilder botBuilder
    //     )
    //     where TBot : BotBase
    //     {
    //         var updateDelegate = botBuilder.Build();

    //         var options = app.ApplicationServices.GetRequiredService<IOptions<BotOptions<TBot>>>();
    //         app.Map(
    //             options.Value.WebhookPath,
    //             builder => builder.UseMiddleware<TelegramBotMiddleware<TBot>>(updateDelegate)
    //         );

    //         return app;
    //     }

        public static IApplicationBuilder EnsureWebhookSet<TBot>(this IApplicationBuilder app) where TBot : IBot
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var bot = scope.ServiceProvider.GetRequiredService<TBot>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<BotOptions<TBot>>>();
                var url = new Uri(new Uri("https://nickweiss.info"), options.Value.WebhookPath);

                logger.LogInformation("Setting webhook for bot \"{0}\" to URL \"{1}\"", typeof(TBot).Name, url);

                bot.Client.SetWebhookAsync(url.AbsoluteUri)
                    .GetAwaiter().GetResult();
            }

            return app;
        }
    }
}