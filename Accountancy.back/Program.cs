using Accountancy.Startup.Installers;

var builder = WebApplication.CreateBuilder(args);

SwaggerInstaller.ConfigureServices(builder.Services);
MvcInstaller.ConfigureServices(builder.Services);
DatabaseInstaller.ConfigureServices(builder.Services, builder.Configuration.GetSection("ConnectionStrings"));
DocumentIntaller.ConfigureServices(builder.Services);
SecurityInstaller.ConfigureServices(builder.Services);

var app = builder.Build();

SwaggerInstaller.Configure(app);
ExceptionInstaller.Configure(app);
SecurityInstaller.Configure(app);
MvcInstaller.Configure(app);

app.Run();