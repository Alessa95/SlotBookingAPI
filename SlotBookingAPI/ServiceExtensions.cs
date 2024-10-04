using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SlotBooking.Infrastructure.HttpClients;
using SlotBookingAPI.Options;
using SlotBookingAPI.Services;
using System.Text;

namespace SlotBookingAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration, string apiName)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = apiName,
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Config setting is required: Jwt:Issuer"),
                    ValidAudience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Config setting is required: Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("Config setting is required: Jwt:Key")))
                };
            });
        }

        public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ApiClient>((serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["AvailabilityApi:ApiBaseUrl"] ?? throw new InvalidOperationException("Config setting is required: AvailabilityApi:ApiBaseUrl");
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddTransient(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(nameof(ApiClient));
                var configuration = provider.GetRequiredService<IConfiguration>();
                var username = configuration["AvailabilityApi:ApiUsername"] ?? throw new InvalidOperationException("Config setting is required: AvailabilityApi:ApiUsername");
                var password = configuration["AvailabilityApi:ApiPassword"] ?? throw new InvalidOperationException("Config setting is required: AvailabilityApi:ApiPassword");
                return new ApiClient(httpClient, username, password);
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IAuthService, AuthService>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenOptions>(configuration.GetSection("Jwt"));
            services.Configure<TokenOptions>(configuration.GetSection("AvailabilityApi"));
        }
    }
}
