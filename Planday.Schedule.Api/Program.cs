using Planday.Schedule.Commands;
using Planday.Schedule.Infrastructure.Commands;
using Planday.Schedule.Infrastructure.Factories;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Queries;

var builder = WebApplication.CreateBuilder(args);

#region Service Registration

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionStringProvider>(new ConnectionStringProvider(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<ISqliteConnectionFactory, SqliteConnectionFactory>();
// builder.Services.AddScoped(config => new ShiftQueryBase(config.GetService<IConnectionStringProvider>()));

// Register Queries
builder.Services.AddScoped<IGetAllShiftsQuery, GetAllShiftsQuery>();
builder.Services.AddScoped<IGetShiftByIdQuery, GetShiftByIdQuery>();
builder.Services.AddScoped<IGetEmployeeByIdQuery, GetEmployeeByIdQuery>();
builder.Services.AddScoped<IGetEmployeeShiftQuery, GetEmployeeShiftQuery>();

// Register Commands
builder.Services.AddScoped<ICreateOpenShiftCommand, CreateOpenShiftCommand>();
builder.Services.AddScoped<IAssignShiftToEmployeeCommand, AssignShiftToEmployeeCommand>();

#endregion

var app = builder.Build();

#region Pipeline Configurations

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#endregion

#region Application Run

app.Run();

#endregion
