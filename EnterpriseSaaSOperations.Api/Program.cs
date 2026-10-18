
using EnterpriseSaaSOperations.Api.Authorization;
using EnterpriseSaaSOperations.Application.Authentication;
using EnterpriseSaaSOperations.Application.OperationalRequests;
using EnterpriseSaaSOperations.Application.Organizations;
using EnterpriseSaaSOperations.Application.Teams;
using EnterpriseSaaSOperations.Domain.Enums;
using EnterpriseSaaSOperations.Infrastructure.Identity;
using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace EnterpriseSaaSOperations.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddPersistence(builder.Configuration);
            // Identity registration - configures account-management services and connects them to the db context.
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer();

            builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                            .Configure<IOptions<JwtOptions>>((options, jwtOptionsAccessor) =>
                            {
                                var jwtOptions = jwtOptionsAccessor.Value;
                                var signingKeyBytes = Convert.FromBase64String(jwtOptions.SigningKey);

                                // Keep JWT claim names such as "sub" unchanged.
                                options.MapInboundClaims = false;

                                options.TokenValidationParameters = new()
                                {
                                    ValidateIssuer = true,
                                    ValidIssuer = jwtOptions.Issuer,

                                    ValidateAudience = true,
                                    ValidAudience = jwtOptions.Audience,

                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),

                                    ValidateLifetime = true,
                                    RequireExpirationTime = true,
                                    ClockSkew = TimeSpan.FromSeconds(30),

                                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
                                };
                            });

            builder.Services.AddScoped<IAuthorizationHandler, DomainRoleAuthorizationHandler>();

            builder.Services.AddAuthorizationBuilder()
                            .AddPolicy(AuthorizationPolicies.AdministratorOrOwner, 
                                       policy =>
                                       {
                                           policy.RequireAuthenticatedUser();
                                           policy.AddRequirements(new DomainRoleRequirement(UserRole.Owner,
                                                                                            UserRole.Administrator));
                                       })
                            .AddPolicy(AuthorizationPolicies.OwnerOnly, 
                                       policy =>
                                       {
                                           policy.RequireAuthenticatedUser();
                                           policy.AddRequirements(new DomainRoleRequirement(UserRole.Owner));
                                       });

            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null, false));
                            });

            // Register the Application use cases.
            builder.Services.AddScoped<LoginService>()
                            .AddScoped<OrganizationOnboardingService>()
                            .AddScoped<OperationalRequestService>()
                            .AddScoped<TeamManagementService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
