using AutoMapper;
using Chinook;
using Chinook.Areas.Identity;
using Chinook.CustomConfiguration;
using Chinook.Database.Repositories;
using Chinook.Database.Repositories.Contract;
using Chinook.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ChinookContext>(opt => opt.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Automapper config
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDefaultIdentity<ChinookUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ChinookContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ChinookUser>>();

//Repository
builder.Services.AddScoped<IArtistRepository,ArtistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITracksRepository, TracksRepository>();
builder.Services.AddScoped<IPlayListRepository, PlayListRepository>();
builder.Services.AddScoped<IUserPlayListRepository, UserPlayListRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
