using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Valeo.Bot.Models;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.BotBuilderMiddleware.BotBuilder;
using ValeoBot.Configuration;
using ValeoBot.Configuration.Entities;
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
        public static IConfiguration StaticConfiguration { get; private set;}
        private IHostingEnvironment _envLocal;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            StaticConfiguration = configuration;
            _envLocal = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddConfigurationProvider(Configuration);

            services.AddScoped<IDataRepository<Order>, OrderRepository>();
            services.AddScoped<IDataRepository<ValeoUser>, UserReposiroty>();
            services.AddScoped<IDataRepository<Registration>, RegistrationRepository>();
            services.AddScoped<IAuthorization, AuthorizationService>();

            if (_envLocal.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase"))
                   //options.UseInMemoryDatabase("Test")
                );
            }
            else
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RemoteDatabase")));
            }

            services
                .AddScoped<ResponseController>()
                .AddScoped<SessionService>()
                .AddScoped<AuthorizationService>()
                .AddScoped<ValeoKeyboardsService>()
                //.AddScoped<IValeoAPIService, ValeoAPIMockService>();
                .AddTransient<IValeoAPIService, ValeoAPIService>();
                var tt = ValeoAPIService.Auth;
            services.AddTransient<ValeoLifeBot>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<AuthorizationHandler>()
                .AddScoped<StartCommand>()
                .AddScoped<OrderUpdater>()
                .AddScoped<CallbackQueryHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseTelegramBotLongPolling<ValeoLifeBot>(ConfigureBot(), startAfter : TimeSpan.FromSeconds(2));
                app.UseMvc();
            }
            else
            {
                //UseMvc and Use Webhook order important!!
                app.UseMvc();
                app.UseTelegramBotWebhook<ValeoLifeBot>(ConfigureBot());
                app.EnsureWebhookSet<ValeoLifeBot>();
            }

        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .Use<AuthorizationHandler>()
                //.UseWhen<WebhookLogger>(When.Webhook)
                //.UseWhen<UpdateMembersList>(When.MembersChanged)
                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                        .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<StartCommand>("start")
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