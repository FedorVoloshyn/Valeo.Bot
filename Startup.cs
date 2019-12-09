using System;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Extentions;
using IBWT.Framework.Middleware;
using IBWT.Framework.Scheduler;
using IBWT.Framework.State.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Valeo.Bot.Configuration;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Data.Repository;
using Valeo.Bot.Handlers;
using Valeo.Bot.Services;
using Valeo.Bot.Services.HelsiAuthorization;

namespace Valeo.Bot
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public static IConfiguration StaticConfiguration { get; private set; }
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            StaticConfiguration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddConfigurationProvider(Configuration, env);

            services.AddScoped<IDataRepository<Order>, OrderRepository>();

            if (env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase")));
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

            services.AddScoped<IDataRepository<Order>, OrderRepository>();
            services.AddScoped<IDataRepository<ValeoUser>, UserRepository>();
            services.AddScoped<IDataRepository<Registration>, RegistrationRepository>();
            services.AddScoped<IAuthorization, AuthorizationService>();
            services.AddSingleton<IMailingService, MailingService>();

            // Save history of telegram user movements throw the bots' menus
            services.AddBotStateCache<InMemoryStateProvider>(ConfigureBot());

            services.AddTelegramBot()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateLogger>()
                .AddScoped<AuthorizationHandler>()
                .AddScoped<DefaultHandler>()
                .AddScoped<StartCommand>()
                .AddScoped<AboutQueryHandler>()
                .AddScoped<LocationsQueryHandler>()
                .AddScoped<FeedbackQueryHandler>()
                .AddScoped<DoctorsQueryHandler>()
                .AddScoped<ContactsQueryHandler>();


            // services.AddSingleton<IScheduledTask, NotifyWeatherTask>();
            // services.AddScheduler((sender, args) =>
            // {
            //     Console.Write(args.Exception.Message);
            //     args.SetObserved();
            // });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMvc();
                app.UseTelegramBotLongPolling(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                app.UseMvc();
                app.UseTelegramBotWebhook(ConfigureBot());
                app.EnsureWebhookSet();
            }

        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .Use<UpdateLogger>()
                //.Use<AuthorizationHandler>()
                .MapWhen(When.State("default"), cmdBranch => cmdBranch
                    .MapWhen(When.NewMessage, msgBranch => msgBranch
                        .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                            .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                                .UseCommand<StartCommand>("start")
                            )
                        //.Use<NLP>()
                        )

                    )
                    .Use<DefaultHandler>()
                )
                .MapWhen(When.State("doctors"), defaultBranch => defaultBranch
                    //.MapWhen<DoctorsQueryHandler>(When.CallbackQuery)
                    .Use<DoctorsQueryHandler>()
                )
                .MapWhen(When.State("locations"), defaultBranch => defaultBranch
                    .Use<LocationsQueryHandler>()
                )
                .MapWhen(When.State("contacts"), defaultBranch => defaultBranch
                    .Use<ContactsQueryHandler>()
                )
                .MapWhen(When.State("about"), defaultBranch => defaultBranch
                    .Use<AboutQueryHandler>()
                )
                .MapWhen(When.State("feedback"), defaultBranch => defaultBranch
                    .Use<FeedbackQueryHandler>()
                )
            // .MapWhen(When.State("pagination"), defaultBranch => defaultBranch
            //     .MapWhen<PaginationHandler>(When.CallbackQuery)
            // )
            // .Use<UnhandledUpdateReporter>()
            ;
        }
    }
}