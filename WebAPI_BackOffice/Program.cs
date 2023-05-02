using MauiAuth0App.Auth0;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using System.Text.Json;
using WebAPI_BackOffice;
using WebAPI_BackOffice.DB;
using WebAPI_BackOffice.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
       builder =>
       {
           builder.WithOrigins("https://localhost:44302")
                  .WithHeaders("Authorization");
       });
});

//builder.Services.AddSingleton(new DBConfiguration(builder.Configuration["ConnectionStrings:DefaultConnection"].ToString()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
            options.Audience = builder.Configuration["Auth0:Audience"];

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.Response.OnStarting(async () =>
                    {
                        await context.Response.WriteAsync(
                              JsonSerializer.Serialize(new ApiResponse("You are not authorized!"))
                              );
                    });

                    return Task.CompletedTask;
                }
            };
        });

builder.Services.AddSingleton(new Auth0Client(new()
{
    Domain = "dev-17683470.okta.com",
    ClientId = "0oa912ox83mA6vxCh5d7",
    Scope = "openid profile",
    Audience = "https://dev-17683470.okta.com",
#if WINDOWS
            RedirectUri = "https://da70-2402-8100-2569-6467-90d2-b4b-380f-48b6.ngrok-free.app"
#else
    RedirectUri = "myapp://callback"
#endif
}));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
          policy.RequireAssertion(context =>
              context.User.HasClaim(c =>
                  (c.Type == "permissions" &&
                  c.Value == "read:admin-messages") &&
                  c.Issuer == $"https://{builder.Configuration["Auth0:Domain"]}/")));

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

IdentityModelEventSource.ShowPII = true; //Add this line


app.Run();