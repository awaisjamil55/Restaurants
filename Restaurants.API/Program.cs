using Restaurants.API.Exceptions;
using Restaurants.API.Extensions;
using Restaurants.API.Logging;
using Restaurants.API.Startup;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

await seeder.Seed();

// Configure the HTTP request pipeline.
app.UseErrorHandling();
app.UseLogging();

//app.UseSerilogRequestLogging();

app.UseRestaurantsSwaggerUI();

app.UseHttpsRedirection();

app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
