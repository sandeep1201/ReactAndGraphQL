using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Dcf.Wwp.Api.Common;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Errors;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Api.Library.Auth.Handlers;
using Dcf.Wwp.Api.Library.Auth.Policies;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Middleware;
using Dcf.Wwp.ConnectedServices;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.Model.Services;
using DCF.Common.Configuration;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Sinks.MSSqlServer;

namespace Dcf.Wwp.Api
{
    public class StartupWithConfigureContainer
    {
        #region Properties

        public static StringBuilder      sb = new StringBuilder();
        public static LoggingLevelSwitch levelSwitch { get; set; } = new LoggingLevelSwitch();

        private readonly        SymmetricSecurityKey _signingKey;
        private static readonly StringWriter         sw = new StringWriter(sb);

        public IConfigurationRoot Configuration { get; }

        #endregion

        #region Methods

        public StartupWithConfigureContainer(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(env.ContentRootPath)
                          .AddJsonFile("appsettings.json",                        true, true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                          .AddEnvironmentVariables();

            Configuration = builder.Build();

            var secretKey = Environment.GetEnvironmentVariable("TokenSecretKey");

            if (string.IsNullOrEmpty(secretKey))
            {
                secretKey = "A19ACB534DC4A4BD5FA16AF2B7CFB7BB1D42B48D5CF7EF1A5D72BCA8EF";
            }

            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            //var autoFacConfig = new ConfigurationBuilder()
            //                     .SetBasePath(Directory.GetCurrentDirectory())
            //                     //.AddJsonFile("appsettings.json")
            //                     .AddJsonFile("webServices.json");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection. Don't build or return
            // any IServiceProvider or the ConfigureContainer method
            // won't get called.

            //services.AddAutofac();

            // Add framework services.
            // the following is for Core < 2.0 to 2.1.x
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            //manager.ApplicationParts.Add(new AssemblyPart(typeof(Project1.Project1Type).Assembly));
            services.AddSingleton(manager);

            var jsonSerializerSettings = new JsonSerializerSettings
                                         {
                                             ContractResolver     = new CamelCasePropertyNamesContractResolver(),
                                             NullValueHandling    = NullValueHandling.Include,
                                             DateFormatHandling   = DateFormatHandling.IsoDateFormat,
                                             DateTimeZoneHandling = DateTimeZoneHandling.Local
                                         };

            services.AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc()
                    .AddJsonOptions(o =>
                                    {
                                        o.SerializerSettings.ContractResolver     = jsonSerializerSettings.ContractResolver;
                                        o.SerializerSettings.NullValueHandling    = jsonSerializerSettings.NullValueHandling;
                                        o.SerializerSettings.DateFormatHandling   = jsonSerializerSettings.DateFormatHandling;
                                        o.SerializerSettings.DateTimeZoneHandling = jsonSerializerSettings.DateTimeZoneHandling;
                                    });

            services.AddCors(options =>
                             {
                                 options.AddPolicy("AllowAll",
                                                   builder => { builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });
                             });

            services.AddScoped<IAuthUser, AuthUser>();
            services.AddScoped<DatabaseConfiguration>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<EntSecAuthenticationService>();
            services.AddScoped(x => ApplicationContext.Current);
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<ITimelimitService, TimelimitService>();
            services.AddScoped<ITokenProvider, JwtSecurityTokenProvider>();
            services.AddScoped<IDb2TimelimitService, Db2TimelimitService>();
            services.AddScoped<IAuthenticationService, EntSecAuthenticationService>();

            //services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
            services.AddScoped<SingleInstanceFactory>(c => { return t => { return c.GetService(t); }; });

            services.AddTransient<WwpEntities>();
            services.AddTransient<IAuthorizationPolicyProvider, WwpAuthorizationPolicyProvider>();

            services.AddSingleton(Configuration);
            services.AddSingleton<DatabaseConfiguration>();
            services.AddSingleton<IAuthorizationHandler, ApiKeyHandler>();
            services.AddSingleton<IApiExceptionHandler, ApiExceptionHandler>();
            services.AddSingleton<IErrorInfoConverter, DefaultErrorInfoConverter>();
            services.AddSingleton<Func<WwpEntities>>(c => () => c.GetService<WwpEntities>());
            services.AddSingleton<JsonSerializerSettings, JsonSerializerSettings>(provider => jsonSerializerSettings);

            services.AddSingleton(s => new TokenProviderOptions
                                       {
                                           Path               = "/api/token",
                                           Audience           = "WWPUser",
                                           Issuer             = "DCF",
                                           SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                                           Expiration         = TimeSpan.FromHours(24) // TODO:get from Config
                                       });

            services.AddSingleton<IReadOnlyCollection<ICountyAndTribe>>(ctx =>
                                                                        {
                                                                            var db = ctx.GetService<WwpEntities>();

                                                                            var rs = (db.CountyAndTribes
                                                                                        .Where(i => !i.IsDeleted)
                                                                                        .OrderBy(i => i.CountyName)
                                                                                        //.Select(i => new { Id = i.Id, Num = i.CountyNumber, Name = i.CountyName, IsCounty = i.IsCounty }).OrderBy(i => i.Name)
                                                                                        .ToList().AsReadOnly());

                                                                            return (rs);
                                                                        });

            services.AddSingleton(s => new TokenValidationParameters
                                       {
                                           // The signing key must match!
                                           ValidateIssuerSigningKey = true,
                                           IssuerSigningKey         = _signingKey,

                                           // Validate the JWT Issuer (iss) claim
                                           ValidateIssuer = true,
                                           ValidIssuer    = "DCF",

                                           // Validate the JWT Audience (aud) claim
                                           ValidateAudience = true,
                                           ValidAudience    = "WWPUser",

                                           // Validate the token expiry
                                           ValidateLifetime = true,

                                           // If you want to allow a certain amount of clock drift, set that here:
                                           ClockSkew = TimeSpan.FromMinutes(5)
                                       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            var dbConfiguration = app.ApplicationServices.GetService<DatabaseConfiguration>();

            DbConfiguration.SetConfiguration(new WwpEnttitesDbConfiguration());

            ConfigureAuth(app, env);

            var connectionString = WwpEntities.CreateSqlConnectionString(dbConfiguration.Server, dbConfiguration.Catalog, dbConfiguration.UserId, dbConfiguration.Password, "WWP-Logging", dbConfiguration.MaxPoolSize);

            var columnOptions = new ColumnOptions();
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.Properties.UsePropertyKeyAsElementName = true;
            columnOptions.Level.StoreAsEnum                      = true;

            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .Enrich.WithCorrelationId()
                         .MinimumLevel.ControlledBy(levelSwitch)
                         .Destructure.ToMaximumDepth(15)
#if !DEBUG
                .WriteTo.RollingFile("logs/log-{Hour}.txt", fileSizeLimitBytes:500*1024*1024,levelSwitch:Startup.levelSwitch)
#endif
                         .WriteTo.LiterateConsole()
                         .WriteTo.MSSqlServer(connectionString, "LogEvent", columnOptions: columnOptions, schemaName: "wwp")
                         .CreateLogger();

            var filterSetting = new FilterLoggerSettings
                                {
                                    { "Microsoft", LogLevel.Error },
                                    { "System", LogLevel.Error }
                                };

            loggerFactory = loggerFactory.WithFilter(filterSetting);

            loggerFactory.AddConsole(new ConsoleLoggerSettings { IncludeScopes = true })
                         .AddDebug()
                         .AddEventLog(LogLevel.Critical);

            SelfLog.Enable(sw);

            //appLifetime.ApplicationStopped.Register(() => { Log.CloseAndFlush(); });
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            Log.Logger.Debug("Logging configured!");

            app.UseMvc();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCors("AllowAll");
            app.UseApplicationContext();
            app.UseNoCacheCacheControl();
            app.UseDeveloperExceptionPage();
            app.UseGlobalExceptionHandler();
            app.UseRequestResponseLogging();

            app.Use(async (context, next) =>
                    {
                        await next();

                        if (context.Response.StatusCode == 404 && context.Request.Headers["Accept"].Any(x => x.ToLower().Contains("text/html")) && !Path.HasExtension(context.Request.Path.Value))
                        {
                            context.Request.Path = "/index.html";
                            await next();
                        }
                    });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("connectedServices.Dev.json");

            var module  = new ConfigurationModule(config.Build());
            builder.RegisterModule(module);
            //builder.RegisterModule(new WebServicesModule());
        }

        #endregion

        #region Authentication

        private void ConfigureAuth(IApplicationBuilder app, IHostingEnvironment env)
        {
            var tokenValidationParameters = app.ApplicationServices.GetService<TokenValidationParameters>();

            //TODO: Change events to log something helpful somewhere
            var jwtEvents = new JwtBearerEvents();

            jwtEvents.OnAuthenticationFailed = context =>
                                               {
                                                   Debug.WriteLine("JWT Authentication failed.");

                                                   return Task.WhenAll();

                                                   //context.HandleResponse();
                                                   //context.Response.StatusCode = 500;
                                                   //context.Response.ContentType = "application/json";

                                                   //if (env.IsDevelopment())
                                                   //{
                                                   //    return
                                                   //        context.Response.WriteAsync(
                                                   //            Newtonsoft.Json.JsonConvert.SerializeObject(new {error = context.Exception.ToString()}));
                                                   //}
                                                   //else
                                                   //{
                                                   //    return
                                                   //        context.Response.WriteAsync(
                                                   //            Newtonsoft.Json.JsonConvert.SerializeObject(new { error = "An error occurred processing your authentication. (invalid ticket)" }));
                                                   //}
                                               };

            jwtEvents.OnChallenge = context =>
                                    {
                                        Debug.WriteLine("JWT Authentication challenged.");

                                        return Task.WhenAll();
                                    };

            jwtEvents.OnMessageReceived = context =>
                                          {
                                              Debug.WriteLine("JWT Message received.");

                                              return Task.WhenAll();
                                          };

            jwtEvents.OnTokenValidated = context =>
                                         {
                                             Debug.WriteLine("JWT Message Token validated.");

                                             return Task.WhenAll();
                                         };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
                                           {
                                               AutomaticAuthenticate     = true,
                                               AutomaticChallenge        = true,
                                               TokenValidationParameters = tokenValidationParameters,
                                               SaveToken                 = true,
                                               IncludeErrorDetails       = true,
                                               Events                    = jwtEvents
                                           });

            app.UseMiddleware<AuthenticationMiddleware>();

            //* We are going to user Bearer schema in the header instead of cookies
            //* The app will maintain the token in local storage and send with each 
            //* request

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AutomaticAuthenticate = false,
            //    AutomaticChallenge = false,
            //    AuthenticationScheme = "Cookie",
            //    CookieName = "access_token",
            //    TicketDataFormat = new CustomJwtDataFormat(
            //        SecurityAlgorithms.HmacSha256,
            //        tokenValidationParameters)
            //});
        }

        #endregion Authentication
    }
}
