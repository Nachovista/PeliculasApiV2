using Microsoft.EntityFrameworkCore;
using PeliculasApi.Data; // <-- tu namespace real
var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: durante cursada, permisivo
const string AnyCors = "AnyCors";
builder.Services.AddCors(o =>
{
    o.AddPolicy(AnyCors, p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(_ => true) // TODO: luego restringir a tu dominio
        .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(AnyCors);
app.MapControllers();
app.Run();
