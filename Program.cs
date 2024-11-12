using Microsoft.Extensions.FileProviders;
using WebApplication1.Statistics.RepositoryHttp.Interfaces;
using WebApplication1.Statistics.RepositoryHttp.Queries;
using WebApplication1.Statistics.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5005");
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<TrackerServices>();
builder.Services.AddScoped<TrackerHttpService>();
builder.Services.AddScoped<PosthogInterface, PosthogQuery>();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Link")),
    RequestPath = "/Storage/Link"
});

app.MapControllerRoute(
    name: "default",
    pattern: "api/v1/Link/{lang?}/{to?}/{from?}",
    defaults: new { controller = "Tracker", action = "SetStat" }
);

app.MapControllerRoute(
    name: "CreateAudioLink",
    pattern: "api/v1/AudioLink",
    defaults: new { controller = "AudioLink", action = "Create" }
);

app.Run();