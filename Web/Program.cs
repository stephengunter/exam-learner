using ApplicationCore.Consts;
using ApplicationCore.DataAccess;
using ApplicationCore.DI;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Settings;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Helpers;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .CreateBootstrapLogger();

try
{
   Log.Information("Starting web application");
   var builder = WebApplication.CreateBuilder(args);
   var Configuration = builder.Configuration;
   builder.Host.UseSerilog((context, services, configuration) => configuration
         .ReadFrom.Configuration(context.Configuration)
         .ReadFrom.Services(services)
         .Enrich.FromLogContext());

   #region Autofac
   builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
   builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
   {
      builder.RegisterModule<ApplicationCoreModule>();
   });

   #endregion
   var services = builder.Services;

   #region Add Configurations
   services.Configure<AppSettings>(Configuration.GetSection(SettingsKeys.App));
   services.Configure<AdminSettings>(Configuration.GetSection(SettingsKeys.Admin));
   services.Configure<AuthSettings>(Configuration.GetSection(SettingsKeys.Auth));
   #endregion

   string connectionString = Configuration.GetConnectionString("Default")!;
   bool usePostgreSql = Configuration[$"{SettingsKeys.Db}:Provider"].EqualTo(DbProvider.PostgreSql);
   if (usePostgreSql)
   {
      builder.Services.AddDbContext<DefaultContext>(options =>
                  options.UseNpgsql(connectionString));
   }
   else
   {
      builder.Services.AddDbContext<DefaultContext>(options =>
                  options.UseSqlServer(connectionString));
   }

   #region AddIdentity
   builder.Services.AddIdentity<User, Role>(options =>
   {
      options.User.RequireUniqueEmail = true;
      options.Password.RequireUppercase = true;        // 要求大寫字母
      options.Password.RequireNonAlphanumeric = true;  // 要求特殊符號
      options.Password.RequiredLength = 8;              // 密碼最少長度      
   })
   .AddEntityFrameworkStores<DefaultContext>()
   .AddDefaultTokenProviders();
   #endregion


   string key = Configuration[$"{SettingsKeys.App}:Key"]!;
   if (String.IsNullOrEmpty(key))
   {
      throw new Exception("app key not been set.");
   }
   
   services.AddScoped<ICryptoService>(provider => new AesGcmCryptoService(key.DeriveKeyFromString()));

   services.AddCorsPolicy(Configuration);
   services.AddJwtBearer(Configuration);
   services.AddAuthorizationPolicy();
   //services.AddMapster(config =>
   //{
      
   //});
   services.AddControllers()
      .AddJsonOptions(options =>
      {
         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      });
   services.AddSwagger(Configuration);

   var app = builder.Build();
   if (usePostgreSql)
   {
      AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
   }

   app.UseSerilogRequestLogging();

   if (app.Environment.IsDevelopment())
   {
      if (Configuration[$"{SettingsKeys.Developing}:SeedDatabase"].ToBoolean())
      {
         // Seed Database
         //using (var scope = app.Services.CreateScope())
         //{
         //   try
         //   {
         //      await SeedData.EnsureSeedData(scope.ServiceProvider, Configuration);
         //   }
         //   catch (Exception ex)
         //   {
         //      Log.Fatal(ex, "SeedData Error");
         //   }
         //}
      }
      app.UseSwagger();
      app.UseSwaggerUI();
   }
   else
   {

   }

   app.UseHttpsRedirection();

   app.UseStaticFiles(); 
   app.UseRouting();

   app.UseCors("Api");
   
   app.UseAuthentication();
   app.UseAuthorization();

   app.MapControllers();
   app.Run();
}
catch (Exception ex)
{
   Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
   Log.Information("finally");
   Log.CloseAndFlush();
}