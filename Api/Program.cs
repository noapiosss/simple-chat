using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddWebSocketManager();
builder.Services.AddSingleton<IUserHandler, UserHandler>();
builder.Services.AddSingleton<IMessageBuffer, MessageBuffer>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseWebSockets();
app.MapSockets("/ws", app.Services.GetRequiredService<WebScoketMessageHandler>());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=ChatPage}/{id?}");

app.Run();
