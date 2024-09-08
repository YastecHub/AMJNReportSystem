using AMJNReportSystem.Application.Abstractions;
using AMJNReportSystem.Application.Abstractions.Repositories;
using AMJNReportSystem.Application.Abstractions.Services;
using AMJNReportSystem.Application.Identity.Roles;
using AMJNReportSystem.Application.Identity.Tokens;
using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using AMJNReportSystem.Persistence.Auth;
using AMJNReportSystem.Persistence.Auth.Jwt;
using AMJNReportSystem.Persistence.Auth.Permissions;
using AMJNReportSystem.Persistence.Common.Services;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Persistence.Encryption;
using AMJNReportSystem.Persistence.Identity;
using AMJNReportSystem.Persistence.Middleware;
using AMJNReportSystem.Persistence.OpenApi;
using AMJNReportSystem.Persistence.Repositories;
using AMJNReportSystem.Persistence.SecurityHeaders;
using ZymLabs.NSwag.FluentValidation;
using AMJNReportSystem.Domain.Repositories;
using AMJNReportSystem.Infrastructure.Repositories;
using AMJNReportSystem.Application.Validation;
using FluentValidation;
using AMJNReportSystem.Application.Models.RequestModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;

namespace AMJNReportSystem.IOC.ServiceCollections
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddScoped<IReportSubmissionRepository, ReportSubmissionRepository>()
                .AddScoped<IReportTypeRepository, ReportTypeRepository>()
                .AddScoped<IReportSectionRepository, ReportSectionRepository>()
                .AddScoped<IReportResponseRepository, ReportResponseRepository>()
                .AddScoped<IQuestionRepository, QuestionRepository>()
                .AddScoped<IQuestionOptionRepository, QuestionOptionRepository>()
                .AddScoped<ISubmissionWindowRepository, SubmissionWindowRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddScoped<IEncryptionService, EncryptionService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IReportSubmissionService, ReportSubmissionService>()
                .AddScoped<ISerializerService, NewtonSoftService>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<ISubmissionWindowService, SubmissionWindowService>()
                .AddScoped<IReportSectionService, ReportSectionService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<IReportResponseService, ReportResponseService>()
                .AddScoped<IReportTypeService, ReportTypeService>();
        }

        public static IServiceCollection AddFluentValidators(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddScoped<IValidator<CreateReportSubmissionRequest>, ReportSubmissionValidator>()
                .AddScoped<IValidator<CreateReportTypeRequest>, ReportTypeRequestValidator>()
				.AddScoped<IValidator<CreateQuestionRequest>, CreateQuestionRequestValidator>()
				.AddScoped<IValidator<UpdateQuestionRequest>, UpdateQuestionRequestValidator>()
				.AddScoped<IValidator<CreateSubmissionWindowRequest>, CreateSubmissionWindowRequestValidator>()
				.AddScoped<IValidator<UpdateSubmissionWindowRequest>, UpdateSubmissionWindowRequestValidator>();
        }


        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }


        internal static IServiceCollection AddIdentity(this IServiceCollection services) =>
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .Services;

        internal static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services.AddOptions<JwtSettings>()
                .BindConfiguration($"SecuritySettings:{nameof(JwtSettings)}")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            return services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!)
                .Services;

        }

        internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddCurrentUser()
                .AddPermissions()

                // Must add identity before adding auth!
                .AddIdentity();
            services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
            return config["SecuritySettings:Provider"]!.Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
                ? services.AddAzureAdAuth(config)
                : services.AddJwtAuth();
        }

        internal static IServiceCollection AddAzureAdAuth(this IServiceCollection services, IConfiguration config)
        {
            // var logger = Log.ForContext(typeof(AzureAdJwtBearerEvents));

            services
                .AddAuthorization()
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });
            /*.AddMicrosoftIdentityWebApi(
                jwtOptions => jwtOptions.Events = new AzureAdJwtBearerEvents(logger, config),
                msIdentityOptions => config.GetSection("SecuritySettings:AzureAd").Bind(msIdentityOptions));*/

            return services;
        }

        internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
            app.UseMiddleware<CurrentUserMiddleware>();

        private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
            services
                .AddScoped<CurrentUserMiddleware>()
                .AddScoped<ICurrentUser, CurrentUser>()
                .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

        private static IServiceCollection AddPermissions(this IServiceCollection services) =>
            services
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();



        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddApiVersioning()
                .AddAuth(config)
                /* .AddBackgroundJobs(config)
                 .AddCaching(config)
                 .AddCorsPolicy(config)*/
                .AddExceptionMiddleware()
                .AddHealthCheck()
                 /* .AddPOLocalization(config)
                  .AddMailing(config)
                  .AddMultitenancy()
                  .AddNotifications(config)*/
                 .AddOpenApiDocumentation(config)
                .AddRequestLogging(config)
                .AddRouting(options => options.LowercaseUrls = true);
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

        private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
            services.AddHealthChecks().Services;

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder
                .UseRequestLocalization()
                .UseStaticFiles()
                .UseSecurityHeaders(config)
                //.UseFileStorage()
                .UseExceptionMiddleware()
                .UseRouting()
                //.UseCorsPolicy()
                .UseCurrentUser()
                .UseRequestLogging(config)
                //.UseHangfireDashboard(config)
                .UseOpenApiDocumentation(config);

        /* public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
         {
             builder.MapControllers().RequireAuthorization();
             builder.MapHealthCheck();
             builder.MapNotifications();
             return builder;
         }
     */
        private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
            endpoints.MapHealthChecks("/api/health");

        /* internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app) =>
             app.UseStaticFiles(new StaticFileOptions()
             {
                 FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                 RequestPath = new PathString("/Files")
             });*/

        internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
            services.AddScoped<ExceptionMiddleware>();

        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();

        internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                services.AddSingleton<RequestLoggingMiddleware>();
                services.AddScoped<ResponseLoggingMiddleware>();
            }

            return services;
        }

        internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
                app.UseMiddleware<ResponseLoggingMiddleware>();
            }

            return app;
        }

        private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
            config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>()!;

        internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection(nameof(Persistence.OpenApi.SwaggerSettings)).Get<Persistence.OpenApi.SwaggerSettings>();
            if (settings == null) return services;
            if (settings.Enable)
            {
                services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
                services.AddEndpointsApiExplorer();

                services.AddScoped<FluentValidationSchemaProcessor>(provider =>
                {
                    var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
                    var loggerFactory = provider.GetService<ILoggerFactory>();

                    return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
                });

                _ = services.AddOpenApiDocument((document, serviceProvider) =>
                {
                    document.PostProcess = doc =>
                    {
                        doc.Info.Title = settings.Title;
                        doc.Info.Version = settings.Version;
                        doc.Info.Description = settings.Description;
                        doc.Info.Contact = new()
                        {
                            Name = settings.ContactName,
                            Email = settings.ContactEmail,
                            Url = settings.ContactUrl
                        };
                        doc.Info.License = new()
                        {
                            Name = settings.LicenseName,
                            Url = settings.LicenseUrl
                        };
                    };

                    if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                    {
                        document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.OAuth2,
                            Flow = OpenApiOAuth2Flow.AccessCode,
                            Description = "OAuth2.0 Auth Code with PKCE",
                            Flows = new()
                            {
                                AuthorizationCode = new()
                                {
                                    AuthorizationUrl = config["SecuritySettings:Swagger:AuthorizationUrl"],
                                    TokenUrl = config["SecuritySettings:Swagger:TokenUrl"],
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { config["SecuritySettings:Swagger:ApiScope"]!, "access the api" }
                                    }
                                }
                            }
                        });
                    }
                    else
                    {
                        document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Description = "Input your Bearer token to access this API",
                            In = OpenApiSecurityApiKeyLocation.Header,
                            Type = OpenApiSecuritySchemeType.Http,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            BearerFormat = "JWT",
                        });
                    }

                    document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                    document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());

                    //document.TypeMappers.Add(new PrimitiveTypeMapper(typeof(TimeSpan), schema =>
                    //{
                    //    schema.Type = NJsonSchema.JsonObjectType.String;
                    //    schema.IsNullableRaw = true;
                    //    schema.Pattern = @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$";
                    //    schema.Example = "02:00:00";
                    //}));

                    document.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());

                    var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
                    //document.SchemaProcessors.Add(fluentValidationSchemaProcessor);
                });
            }

            return services;
        }

        internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
        {
            if (config.GetValue<bool>("SwaggerSettings:Enable"))
            {
                app.UseOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DefaultModelsExpandDepth = -1;
                    options.DocExpansion = "none";
                    options.TagsSorter = "alpha";
                    if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                    {
                        options.OAuth2Client = new OAuth2ClientSettings
                        {
                            AppName = "Full Stack Hero Api Client",
                            ClientId = config["SecuritySettings:Swagger:OpenIdClientId"],
                            ClientSecret = string.Empty,
                            UsePkceWithAuthorizationCodeGrant = true,
                            ScopeSeparator = " "
                        };
                        options.OAuth2Client.Scopes.Add(config["SecuritySettings:Swagger:ApiScope"]);
                    }
                });
            }

            return app;
        }

        internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
        {
            var settings = config.GetSection(nameof(SecurityHeaderSettings)).Get<SecurityHeaderSettings>();

            if (settings?.Enable is true)
            {
                app.Use(async (context, next) =>
                {
                    if (!context.Response.HasStarted)
                    {
                        if (!string.IsNullOrWhiteSpace(settings.Headers.XFrameOptions))
                        {
                            context.Response.Headers.Add(AppHeaderNames.XFRAMEOPTIONS, settings.Headers.XFrameOptions);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.XContentTypeOptions))
                        {
                            context.Response.Headers.Add(AppHeaderNames.XCONTENTTYPEOPTIONS, settings.Headers.XContentTypeOptions);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.ReferrerPolicy))
                        {
                            context.Response.Headers.Add(AppHeaderNames.REFERRERPOLICY, settings.Headers.ReferrerPolicy);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.PermissionsPolicy))
                        {
                            context.Response.Headers.Add(AppHeaderNames.PERMISSIONSPOLICY, settings.Headers.PermissionsPolicy);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.SameSite))
                        {
                            context.Response.Headers.Add(AppHeaderNames.SAMESITE, settings.Headers.SameSite);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.Headers.XXSSProtection))
                        {
                            context.Response.Headers.Add(AppHeaderNames.XXSSPROTECTION, settings.Headers.XXSSProtection);
                        }
                    }

                    await next();
                });
            }

            return app;
        }
    }
}
