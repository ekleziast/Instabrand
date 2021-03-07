using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Instabrand.Extensions
{
    internal static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Instabrand Api"
                });


                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "<p>You need to set Bearer Token to get access to API</p>" +
                  "<p><b>Example:</b> <br />" +
                  "Authorization: Bearer 584f0eee-1e66-43b9-9192-08c0c65c0770</p>",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                };


                options.AddSecurityDefinition("Bearer", securityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                options.CustomSchemaIds(type => type.FullName);

                options.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid", Default = new OpenApiString(Guid.NewGuid().ToString()) });

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(basePath, "Instabrand.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "Instabrand.Queries.xml"));
            });
        }
    }
}
