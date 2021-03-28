using FluentValidation.AspNetCore;
using Instabrand.Extensions;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddScoped<Domain.Authentication.IRefreshTokenStore, Infrastructure.RefreshTokenStore.RefreshTokenStore>();

            #endregion

            #region Registration

            services.AddScoped<Domain.Registration.UserRegistrationService>();
            services.AddScoped<Domain.Registration.IUserRepository, Infrastructure.Registration.UserRepository>();
            services.AddScoped<Domain.Registration.IPasswordHasher, Infrastructure.PasswordHasher.PasswordHasher>();
            services.AddNpgsqlDbContextPool<Infrastructure.Registration.UsersDbContext>(npgsqlConnectionString);

            #endregion

            #region RefreshTokenStore

            services.AddNpgsqlDbContextPool<Infrastructure.RefreshTokenStore.RefreshTokenStoreDbContext>(npgsqlConnectionString);

            #endregion

            services.AddQueryProcessor<Queries.Infrastructure.Samples.SampleQueryHandler>();

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
