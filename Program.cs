using apiTEST.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Utilização do 'extension method' definido na classe 'EmployeeEndpoints'
app.MapEmployeesEndpoints();

app.Run();
