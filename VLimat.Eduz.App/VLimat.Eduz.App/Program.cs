using Microsoft.EntityFrameworkCore;
using VLimat.Eduz.Application.DependencyInjection;
using VLimat.Eduz.Infrastructure.DependencyInjection; // if you created DI extension in Infrastructure
using VLimat.Eduz.Infrastructure.Persistence;
using VVLimat.Eduz.App.Middleware;

var builder = WebApplication.CreateBuilder(args);
// read connection string
//var conn = builder.Configuration.GetConnectionString("DefaultConnection");
//const string dapperConnectionName = "DapperConnection";
//var dapperConn = builder.Configuration.GetConnectionString(dapperConnectionName);


//if (string.IsNullOrWhiteSpace(conn))
//{
//    throw new InvalidOperationException("Missing 'DefaultConnection' in appsettings.json or environment.");
//}

//if (string.IsNullOrWhiteSpace(dapperConn))
//{
//    throw new InvalidOperationException($"Missing '{dapperConnectionName}' in appsettings.json or environment.");
//}

// register DbContext (replace ApplicationDbContext with your context type)
// register DbContext (replace ApplicationDbContext with your context type)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
        // .AllowCredentials(); // enable if your Angular app sends credentials (cookies/auth)
    });
});
// Enable CORS middleware using the named policy

builder.Services.AddControllers().Services.AddControllers();
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
    //    options.JsonSerializerOptions.DictionaryKeyPolicy = null; // Use PascalCase for dictionaries
    //}); ;


//Schould Enable when need to  returns dataset or datatable from sql directly
//.AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

// Register MediatR to scan Application assembly for handlers/requests
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("VLimat.Eduz.Application")));
builder.Services.AddApplicationMediator();
// Add IHttpContextAccessor BEFORE registering infrastructure services that depend on it
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureRepositories(builder.Configuration);

// ✅ Add authentication (if using JWT, cookies, etc.)
builder.Services.AddAuthentication(); // optional, if using any auth scheme

// ✅ Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// ✅ Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapGet("/", () => Results.Redirect("/swagger"));
}

//app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
// ✅ Add authentication/authorization in correct order
app.UseMiddleware<HeaderToClaimsMiddleware>();
app.UseAuthentication(); // optional, but must come before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
