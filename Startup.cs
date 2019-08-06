using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.BotBuilderMiddleware.BotBuilder;
using ValeoBot.Configuration;
using ValeoBot.Data.DataManager;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using ValeoBot.Middleware.BotBuilderMiddleware.Extensions;
using ValeoBot.Middleware.Connection;
using ValeoBot.Models;
using ValeoBot.Models.Commands;
using ValeoBot.Services;
using ValeoBot.Services.ValeoApi;

namespace ValeoBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment _envLocal;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _envLocal = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationProvider(Configuration);

            services.AddScoped<IDataRepository<Order>, OrderManager>();
            services.AddScoped<IDataRepository<User>, UserManager>();

            if (_envLocal.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase")));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RemoteDatabase")));
            }

            services.AddSingleton<SessionService>()
                .AddSingleton<ValeoKeyboardsService>()
                .AddTransient<IValeoAPIService, ValeoAPIService>();

            services.AddTransient<ValeoLifeBot>()
                .Configure<BotOptions<ValeoLifeBot>>(Configuration.GetSection("ValeoBot"))
                .AddScoped<ExceptionHandler>()
                .AddScoped<StartCommand>()
                .AddScoped<OrderUpdater>()
                .AddScoped<CallbackQueryHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseTelegramBotLongPolling<ValeoLifeBot>(ConfigureBot(), startAfter : TimeSpan.FromSeconds(2));
            }
            else
            {
                app.UseTelegramBotWebhook<ValeoLifeBot>(ConfigureBot());
                app.EnsureWebhookSet<ValeoLifeBot>();
            }
        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                //.UseWhen<WebhookLogger>(When.Webhook)
                //.UseWhen<UpdateMembersList>(When.MembersChanged)
                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                        .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<StartCommand>("start")
                            .UseCommand<PingCommand>("ping")
                        )
                        .Use<OrderUpdater>()
                        //.Use<NLP>()
                    )
                    //.MapWhen<StickerHandler>(When.StickerMessage)
                    //.MapWhen<WeatherReporter>(When.LocationMessage)
                )

                .MapWhen<CallbackQueryHandler>(When.CallbackQuery)

            //.Use<UnhandledUpdateReporter>()
            ;
        }
    }
}