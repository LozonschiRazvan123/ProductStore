using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.Framework.Configuration;
using ProductStore.Framework.Services;
using ProductStore.Infrastructure.Repository;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using Quartz;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System;
using ProductStore.Localize;
using Microsoft.AspNetCore.Mvc.Razor;
using ProductStore.GraphQL.GraphQLSchema;
using GraphQL.Server;
using GraphQL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GraphQL.Types;
using ProductStore.GraphQL.GraphQLQueries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddControllersWithViews()
           .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
           .AddDataAnnotationsLocalization();

var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr"),
        };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr")
                };

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;


});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICustomerRepository, CostumerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryProductRepository, CategoryProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IServicePagination<>), typeof(PaginationRepository<>));
builder.Services.AddScoped<ICreateJWT,CreateJWT>();
builder.Services.AddScoped<IGetDataExcel,GetDataExcel>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IImportDataExcel,ImportDataExcel>();
builder.Services.AddScoped<ISorting,Sorting>();
builder.Services.AddScoped<IPDFService,GeneratePDF>();
builder.Services.AddScoped<MessageHub>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<AppSchema>();
builder.Services.AddSingleton<AppMutation>();
builder.Services.AddGraphQL(b => b
    .AddSchema<AppSchema>()
    .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = builder.Environment.IsDevelopment())
    .AddGraphTypes(Assembly.GetExecutingAssembly())
    .AddSystemTextJson());
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
JobKey jobKey = new JobKey("my-job");
builder.Services.AddSignalR();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    // Add each job's configuration like this
    q.AddJob<SendingEmail>(config => config.WithIdentity(jobKey));
    // Add each job's trigger like this and bind it to the job by the key
    q.AddTrigger(config => config
        .WithIdentity("my-job-trigger")
        .WithCronSchedule("0 0 10 ? * * *")
        .ForJob(jobKey));
});
// Let Quartz know that it should finish all the running jobs slowly before shutting down.
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddIdentity<User,IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        var n = builder.Configuration.GetSection("JwtSettings:Token").Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {

           
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(n)),
            //builder.Configuration.GetSection("JwtSettings:Token").Value)
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    }
    );

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    Seed.SeedData(app);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CORSPolicy");

app.UseRouting();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGraphQL();

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/productoffers");
    endpoints.MapGraphQLPlayground("/graphql/playground");
});


app.MapControllers();

app.MapGet("options", (IOptions<JwtSettings> options) =>
{
    var response = new
    {
        options.Value.Token
    };
    return Results.Ok(response);
});
app.UseStaticFiles();

app.Run();
