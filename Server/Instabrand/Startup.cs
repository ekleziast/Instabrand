using FluentValidation.AspNetCore;
using Instabrand.Extensions;
using Instabrand.Middlewares;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

namespace Instabrand
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var npgsqlConnectionString = Configuration.GetConnectionString("Boxis");

            services
                .AddControllers(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                    configuration.LocalizationEnabled = false;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwagger();


            services.AddHealthChecks()
                .AddNpgSql(npgsqlConnectionString);

            #region Authentication and Authorization

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("user", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Auth:UserJwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Auth:UserJwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Auth:UserJwt:SecretKey"])),
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization(options =>
            {
                var userPolicy = new AuthorizationPolicyBuilder("user").RequireAuthenticatedUser();
                options.AddPolicy("user", userPolicy.Build());
            });

            services.AddScoped<Domain.Authentication.UserAuthenticationService>();

            services.AddScoped<Domain.Authentication.IAccessTokenFactory, Infrastructure.Authentication.JwtAccessTokenFactory>();
            services.AddScoped<Domain.Authentication.IPasswordHasher, Infrastructure.PasswordHasher.PasswordHasher>();
            services.AddScoped<Domain.Authentication.IRefreshTokenStore, Infrastructure.RefreshTokenStore.RefreshTokenStore>();
            services.AddScoped<Domain.Authentication.IUserGetter, Infrastructure.Authentication.UserGetter>();

            services.Configure<Infrastructure.Authentication.JwtAuthOptions>(
                Configuration.GetSection("Auth:UserJwt"));

            services.AddNpgsqlDbContextPool<Infrastructure.Authentication.AuthenticationDbContext>(
                npgsqlConnectionString);

            #endregion

            #region Registration

            services.AddScoped<Domain.Registration.UserRegistrationService>();
            services.AddScoped<Domain.Registration.IUserRepository, Infrastructure.Registration.UserRepository>();
            services.AddScoped<Domain.Registration.IPasswordHasher, Infrastructure.PasswordHasher.PasswordHasher>();
            services.AddNpgsqlDbContextPool<Infrastructure.Registration.RegistrationDbContext>(npgsqlConnectionString);


            services.AddScoped<Domain.Registration.IConfirmationCodeProvider, Infrastructure.ConfirmationCodeProvider.ConfirmationCodeProvider>();
            services.Configure<Infrastructure.ConfirmationCodeProvider.ConfirmationCodeProviderOptions>(Configuration.GetSection("ConfirmationCode"));

            services.AddScoped<Domain.Registration.IConfirmationCodeSender, Infrastructure.EmailService.EmailService>();

            #endregion

            #region Email

            services.Configure<Infrastructure.EmailService.EmailServiceOptions>(Configuration.GetSection("EmailService"));

            #endregion

            #region RefreshTokenStore

            services.AddNpgsqlDbContextPool<Infrastructure.RefreshTokenStore.RefreshTokenStoreDbContext>(npgsqlConnectionString);

            #endregion

            #region Instapage

            services.AddScoped<Domain.Instapage.IInstapageRepository, Infrastructure.Instapages.InstapageRepository>();
            services.AddNpgsqlDbContextPool<Infrastructure.Instapages.InstapagesDbContext>(npgsqlConnectionString);

            #endregion

            #region Instagram

            services.Configure<Infrastructure.Instagram.InstagramApiOptions>(Configuration.GetSection("InstagramApi"));
            services.AddHttpClient<Infrastructure.Instagram.InstagramApi>(options =>
            {
                options.BaseAddress = new System.Uri(@"https://api.instagram.com/");
            });

            services.AddHttpClient<Infrastructure.Instagram.InstagramGraphApi>(options =>
            {
                options.BaseAddress = new System.Uri(@"https://graph.instagram.com/");
            });

            services.AddScoped<Infrastructure.Instagram.InstapageCreationService>();

            #endregion

            #region FileStorage

            services.AddSingleton<Domain.IFileStorage, Infrastructure.FileStorage.FileStorage>();
            services.Configure<Infrastructure.FileStorage.FilePathOptions>(Configuration.GetSection("FilePathOptions"));

            #endregion

            #region Instapages

            services.AddNpgsqlDbContextPool<Infrastructure.Instapages.InstapagesDbContext>(npgsqlConnectionString);
            services.AddScoped<Domain.Instapage.IInstapageRepository, Infrastructure.Instapages.InstapageRepository>();

            #endregion

            #region QueryHandlers

            services.AddQueryProcessor<Queries.Infrastructure.Samples.SampleQueryHandler>();

            services.AddNpgsqlDbContextPool<Queries.Infrastructure.Users.UsersDbContext>(npgsqlConnectionString);
            services.AddNpgsqlDbContextPool<Queries.Infrastructure.Instapages.InstapagesDbContext>(npgsqlConnectionString);

            #endregion

            #region DatabaseMigrations

            services.AddNpgsqlDbContext<DatabaseMigrations.InstabrandDbContext>(npgsqlConnectionString);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAspNetCorePathBase();

            app.UseRequestResponseLogging();

            app.UseDbConcurrencyExceptionHandling();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/swagger.json");
                endpoints.MapHealthChecks("hc");
            });

            app.UseSwagger(options => { options.RouteTemplate = "{documentName}/swagger.json"; });
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("../v1/swagger.json", "Boxis Api v1"); });

            app.UseRewriter(new RewriteOptions().AddRedirect(@"^$", "swagger", (int)HttpStatusCode.Redirect));

        }
    }
}
