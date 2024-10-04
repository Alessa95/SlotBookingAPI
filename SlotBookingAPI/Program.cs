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
    options.RegisterServicesFromAssemblies(typeof(GetWeeklyAvailabilityQueryHandler).Assembly);
});

builder.Services.ConfigureJwtAuthentication(builder.Configuration, apiName);
builder.Services.ConfigureHttpClient(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
