using ExpenseTracker.Api.BackgroundServices;
using ExpenseTracker.Api.Extensions;
using ExpenseTracker.Api.Middleware;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Presentation;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.ConfigureCors();

// Infrastructure (DbContext, Repositories, Services, Security)
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Controllers with Presentation assembly
builder.Services.AddControllers()
  .AddApplicationPart(typeof(PresentationReference).Assembly);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    Description = "Enter your JWT token"
  });
  options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
  {
    {
      new Microsoft.OpenApi.Models.OpenApiSecurityScheme
      {
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
          Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});

// Background Service
builder.Services.AddHostedService<TokenCleanupBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
