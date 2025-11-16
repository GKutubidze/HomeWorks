using Microsoft.EntityFrameworkCore;
using Week_19.Data; // Make sure this matches your namespace

var builder = WebApplication.CreateBuilder(args);

// ✅ Add DbContext with connection string from appsettings.json
builder.Services.AddDbContext<PersonContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// ✅ Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal APIs / controller routes
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI
    app.UseSwagger();            // Generate Swagger JSON
    app.UseSwaggerUI(c =>        // Enable interactive UI
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Week_19 API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at root: http://localhost:5000/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();