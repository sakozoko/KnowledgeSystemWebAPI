using IdentityServer;
using IdentityServer.Models;
using IdentityServer.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IdentityServerDbContext>(options => 
    options.UseSqlite("Data Source=identity.db"));

builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        }
    )
    .AddEntityFrameworkStores<IdentityServerDbContext>();

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