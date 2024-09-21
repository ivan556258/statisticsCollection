using WebApplication1.RepositoryHttp.Interfaces;
using WebApplication1.RepositoryHttp.Queries;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.MapControllerRoute(
    name: "default",
    pattern: "api/v1/Link/{lang?}/{to?}/{from?}",
    defaults: new { controller = "Tracker", action = "SetStat" }
);


app.Run();