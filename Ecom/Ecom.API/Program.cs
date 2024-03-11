using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.API.Middleware;
using Ecom.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Note this line to configure 400 for Valdiation Bad Request string Id instad of integer
//builder.Services.AddControllers().ConfigureApiBehaviorOptions(
//    opt => opt.InvalidModelStateResponseFactory = context =>
//    {
//        var errorResponse = new ApiValidationErrorResponse
//        {
//            Errors = context.ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors)
//            .Select(x => x.ErrorMessage).ToArray()
//        };
//        return new BadRequestObjectResult(errorResponse);
//    }
//    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(s =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWt Auth Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    s.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
    s.AddSecurityRequirement(securityRequirement);
});
// add the service of the InfrastructureConfiguration
builder.Services.InfrastructureConfiguration(builder.Configuration);

builder.Services.AddApiRegestration();

// Configure Redis

builder.Services.AddSingleton<IConnectionMultiplexer>(o=>
{
    var config = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"),ignoreUnknown:true);
    return ConnectionMultiplexer.Connect(config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
//Note this middleware it will ditect any error will happen (not found Error for not exisit API)
// and it will redierct to my endpoint of Errors
app.UseStatusCodePagesWithReExecute("/errors/{0}");// 0 to pass the status code to the my error endpoint
app.UseHttpsRedirection();

// Enable Ehe Image Serve
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
InfrasturctureRegistration.InfrastructureConfigMiddleware(app);
app.Run();
