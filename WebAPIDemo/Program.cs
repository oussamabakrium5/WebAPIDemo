using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using WebAPIDemo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// AddControllers registers those dependent services in the container for map controllers to use and then whenever the mapping controllers require those dependent services asp.net will create the instances or directly take the instance from the container and then provide those instances to the mapping mechanism, so with all of these changes pretty simple changes we are able to make the api work now
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//var serviceProvider = builder.Services.BuildServiceProvider();
//var dbContext = serviceProvider.GetRequiredService<DataContext>();
//ShirtRepository.Initialize(dbContext);
builder.Services.AddScoped<ShirtRepository>();

var app = builder.Build();

// those app.UseSomething configuration is middleware components, it does the first one then the second one then the third one etc, it is a middleware pipeline
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
