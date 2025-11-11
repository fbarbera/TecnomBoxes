using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using TecnomBoxes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<TecnomCRMSettings>(
    builder.Configuration.GetSection("TecnomCRM"));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IWorkshopsService, WorkshopsService>();
builder.Services.AddSingleton<IAppointmentsService, AppointmentsService>();
builder.Services.AddScoped<IWorkshopsService, WorkshopsService>();
builder.Services.AddHttpClient("TecnomCRM", (sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<TecnomCRMSettings>>().Value;

    client.BaseAddress = new Uri(config.BaseUrl);

    var byteArray = Encoding.ASCII.GetBytes($"{config.User}:{config.Password}");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var httpFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

    try
    {
        var client = httpFactory.CreateClient("TecnomCRM");
        var response = await client.GetAsync(string.Empty); // Usa BaseAddress
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Conectado correctamente a TecnomCRM ({StatusCode})", response.StatusCode);
        }
        else
        {
            logger.LogWarning("No se pudo autenticar con TecnomCRM. Código: {StatusCode}", response.StatusCode);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error conectando con TecnomCRM al iniciar la aplicación");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
