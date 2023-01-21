using IdentityInfrastructure;
using IdentityInfrastructure.Model;
using IdentityInfrastructure.Persistence;
using IdentityServer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<IdentityContext>();


builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources())
    .AddInMemoryApiScopes(Config.ApiScopes())
    .AddInMemoryApiResources(Config.ApiResources())
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<UserEntity>()
    .AddProfileService<CustomProfileService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();