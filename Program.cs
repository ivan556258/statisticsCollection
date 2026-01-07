using Microsoft.Extensions.FileProviders;
using WebApplication1.Statistics.RepositoryHttp.Interfaces;
using WebApplication1.Statistics.RepositoryHttp.Queries;
using WebApplication1.Statistics.Services;
using dotenv.net;

// Load environment variables from .env file
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var connectionString = $"server={Environment.GetEnvironmentVariable("DB_SERVER")};port={Environment.GetEnvironmentVariable("DB_PORT")};user={Environment.GetEnvironmentVariable("DB_USER")};password={Environment.GetEnvironmentVariable("DB_PASSWORD")};database={Environment.GetEnvironmentVariable("DB_DATABASE")}";

// Add connection string to configuration
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.WebHost.UseUrls($"http://{Environment.GetEnvironmentVariable("APP_HOST")}:{Environment.GetEnvironmentVariable("APP_PORT")}");
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<TrackerServices>();
builder.Services.AddScoped<TrackerHttpService>();
builder.Services.AddScoped<PosthogInterface, PosthogQuery>();

builder.Services.AddScoped<ValidateTutorSlotAttribute>(provider =>
    new ValidateTutorSlotAttribute(builder.Configuration.GetConnectionString("DefaultConnection"))
);


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

// http://localhost:5005/api/v1/Students?tutorId=1&offset=0&limit=5
app.MapControllerRoute(
    name: "GetListStudent",
    pattern: "api/v1/Students/{tutorId?}/{offset?}/{limit?}",
    defaults: new { controller = "Student", action = "GetList" }
);

app.MapControllerRoute(
    name: "CreateStudent",
    pattern: "api/v1/Student/Create",
    defaults: new { controller = "Student", action = "CreateStudent" }
);

app.MapControllerRoute(
    name: "GetListStudentCount",
    pattern: "api/v1/StudentsCount/{tutorId?}",
    defaults: new { controller = "Student", action = "GetListCount" }
);


app.MapControllerRoute(
    name: "GetListTutors",
    pattern: "api/v1/TutorsList/{offset?}/{limit?}",
    defaults: new { controller = "Tutor", action = "GetList" }
);


app.MapControllerRoute(
    name: "GetListTariffs",
    pattern: "api/v1/TariffsList",
    defaults: new { controller = "Tariff", action = "GetList" }
);

app.MapControllerRoute(
    name: "GetListTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/get",
    defaults: new { controller = "Tutor", action = "GetListTimeFree" }
);

app.MapControllerRoute(
    name: "AddTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/add",
    defaults: new { controller = "Tutor", action = "AddTimeFree" }
);

app.MapControllerRoute(
    name: "ChangeTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/change",
    defaults: new { controller = "Tutor", action = "ChangeTimeFree" }
);

app.MapControllerRoute(
    name: "DeleteTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/delete",
    defaults: new { controller = "Tutor", action = "DeleteTimeFree" }
);

app.MapControllerRoute(
    name: "UnconfirmTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/unconfirm",
    defaults: new { controller = "Tutor", action = "UnconfirmTimeFree" }
);

app.MapControllerRoute(
    name: "ConfirmTimeFreeTutor",
    pattern: "api/v1/TimeFreeTutor/confirm",
    defaults: new { controller = "Tutor", action = "ConfirmTimeFree" }
);

app.Run();