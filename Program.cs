// First create a new instance of WebApplicationBuilder, the builder is used to set up and configure the ASP.NET Core application.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container (to handle incoming HTTP requests and generate appropriate responses).
builder.Services.AddControllers();

// AddEndpointsApiExplorer() adds API explorer services to generate OpenAPI descriptions for endpoints.
builder.Services.AddEndpointsApiExplorer();

// AddSwaggerGen() adds Swagger generator services to generate Swagger documents.
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
