using MicroLine.Services.Airline.Application;
using MicroLine.Services.Airline.Infrastructure;
using MicroLine.Services.Airline.WebApi;
using MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddWebApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContextInitializer = scope.ServiceProvider.GetRequiredService<IAirlineDbContextInitializer>();

        await dbContextInitializer.MigrateAsync();
        await dbContextInitializer.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.MapGet("/", () => "MicroLine.Services.Airline");

app.Run();