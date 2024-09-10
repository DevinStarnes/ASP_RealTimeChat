using BlazorSignalRApp.Client.Pages;
using BlazorSignalRApp.Components;
using BlazorSignalRApp.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// [Devin] - START : Add SignalR and Response Compression Middleware services
builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});
// [Devin] - END

var app = builder.Build();

// [Devin] - START : Use Response Compression
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
}
// [Devin] - END

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorSignalRApp.Client._Imports).Assembly);

// [Devin] - START : Map ChatHub
app.MapHub<ChatHub>("/chathub");
// [Devin] - END

app.Run();