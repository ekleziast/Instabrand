using FluentValidation.AspNetCore;
using Instabrand.Extensions;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            var npgsqlConnectionString = Configuration.GetConnectionString("Instabrand");

            services
                .AddControllers()
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

            #region Authentication and Authorization

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
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

            #endregion

            #region RefreshTokenStore

            services.AddNpgsqlDbContextPool<Infrastructure.RefreshTokenStore.RefreshTokenStoreDbContext>(npgsqlConnectionString);

            #endregion

            #region QueryHandlers

            services.AddQueryProcessor<Queries.Infrastructure.Samples.SampleQueryHandler>();

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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instabrand v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
