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
using Valeo.Bot.Services.HelsiAPI;
using Valeo.Bot.Services.HelsiAuthorization;
using Valeo.Bot.Services.ReviewCashService;

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
            services.AddScoped<IDataRepository<Feedback>, FeedbackRepository>();

            services.AddScoped<IHelsiAPIService, HelsiAPIService>();
            services.AddScoped<IAuthorization, AuthorizationService>();
            services.AddSingleton<IMailingService, MailingService>();

            services.AddSingleton<IReviewCacheService, ReviewCacheService>();
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
                .AddScoped<FeedbackCollectHandler>()
                .AddScoped<DoctorsListHandler>()
                .AddScoped<DoctorsQueryHandler>()
                .AddScoped<HelsiDoctorsQueryHandler>()
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
                    .UseWhen(When.NewMessage, msgBranch => msgBranch
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
                   .MapWhen(WhenCopy.HasData, doctorsBranch => doctorsBranch
                        //.MapWhen(WhenCopy.Data(""), branch => branch.Use<SafonovHandler>())
                        .Use<DoctorsListHandler>()
                        .Use<DoctorsListHandler>()
                    )
                    //.Use<DoctorsQueryHandler>()
                    .Use<HelsiDoctorsQueryHandler>()
                )
                .MapWhen(When.State("doctors-time"), defaultBranch => defaultBranch
                   .MapWhen(WhenCopy.HasData, doctorsBranch => doctorsBranch
                        .Use<HelsiDoctorTimesHandler>()
                    )
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
                    .MapWhen<FeedbackQueryHandler>(When.CallbackQuery)
                    .Use<FeedbackCollectHandler>()
                )
            // .MapWhen(When.State("pagination"), defaultBranch => defaultBranch
            //     .MapWhen<PaginationHandler>(When.CallbackQuery)
            // )
            // .Use<UnhandledUpdateReporter>()
            ;
        }

        // TODO: Move to framework
        public static class WhenCopy 
        {
            public static Predicate<IUpdateContext> Data(string data) => (IUpdateContext context) => context.Items["Data"].Equals(data);

            public static bool HasData(IUpdateContext context) => context.Items["Data"] != null && !string.IsNullOrEmpty(context.Items["Data"].ToString());
        }

    }
}