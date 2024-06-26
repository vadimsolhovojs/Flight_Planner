using System.Reflection;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Handlers;
using FlightPlanner.Services;
using FlightPlanner.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// configure basic authentication 
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<FlightPlannerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("flight-planner"));
});
builder.Services.AddTransient<IFlightPlannerDbContext, FlightPlannerDbContext>();
builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<IEntityService<Airport>, EntityService<Airport>>();
builder.Services.AddTransient<IEntityService<Flight>, EntityService<Flight>>();
builder.Services.AddTransient<IFlightService, FlightService>();
builder.Services.AddTransient<IAirportService, AirportService>();

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();