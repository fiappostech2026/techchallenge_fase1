using FCG.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddSwaggerConfig();
builder.AddDatabase();
builder.AddJwt();
builder.Services.AddDependencies();

var app = builder.Build();

app.MigrateDatabase();
app.UseSwaggerConfig();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();