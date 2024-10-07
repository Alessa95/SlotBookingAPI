using FluentValidation;
using MediatR;
using SlotBooking.Application;
using SlotBooking.Application.Slot.Commands;
using SlotBooking.Application.Slot.Queries;
using SlotBookingAPI;
using SlotBookingAPI.Middleware;

var apiName = "SlotBooking API";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(TakeSlotHandler).Assembly);
});

builder.Services.ConfigureJwtAuthentication(builder.Configuration, apiName);
builder.Services.ConfigureHttpClient(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureAplicationServices();
builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// TODO: REMOVE Swagger in Production Environment (JUST FOR TEST)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
