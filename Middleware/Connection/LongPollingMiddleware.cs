using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using ValeoBot.Services;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace ValeoBot.Middleware.Connection
{
    static class LongPollingMiddleware
    {
        public static IApplicationBuilder UseTelegramBotLongPolling<TBot>(this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default) where TBot : BotBase
        {
            if (startAfter == default)
            {
                startAfter = TimeSpan.FromSeconds(2);
            }

            var updateManager = new UpdatePollingManager<TBot>(botBuilder, new BotServiceProvider(app));

            Task.Run(async() =>
                {
                    await Task.Delay(startAfter, cancellationToken);
                    await updateManager.RunAsync(cancellationToken: cancellationToken);
                }, cancellationToken)
                .ContinueWith(t =>
                { 
                    // TODO: use logger
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(t.Exception);
                    Console.ResetColor();
                    throw t.Exception;
                }, TaskContinuationOptions.OnlyOnFaulted);

            return app;
        }
    }
}