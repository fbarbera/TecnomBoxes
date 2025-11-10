using System.Net.Http.Headers;
using System.Text;

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

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<TecnomBoxes.Services.IWorkshopsService, TecnomBoxes.Services.WorkshopsService>();
builder.Services.AddSingleton<TecnomBoxes.Services.IAppointmentsService, TecnomBoxes.Services.AppointmentsService>();
builder.Services.AddScoped<TecnomBoxes.Services.IWorkshopsService, TecnomBoxes.Services.WorkshopsService>();
builder.Services.AddHttpClient("TecnomCRM", client =>
{
    client.BaseAddress = new Uri("https://dev.tecnomcrm.com/api/v1/places/workshops");
    var byteArray = Encoding.ASCII.GetBytes("max@tecnom.com.ar:b0x3sApp");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


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
