using apiTEST;
using apiTEST.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetEmployeeEndpointName = "GetEmployee";

List<EmployeeDto> employees = [
    //Por ter declarado <EmployeeDto> na List, eu não preciso especificar que objeto estou criando 
    //Assim ficando apenas 'new()'
    new (1, "Rafael", "Gerente"),
    new (2, "Douglas", "Funcionário"),
    new (3, "Gabriel", "Funcionário"),
    new (4, "Paulo", "Funcionário")
];

// GET all employees
app.MapGet("employees", () => employees);

// Get specific employee
app.MapGet("employees/{id}", (int id) =>
{
    EmployeeDto? employee = employees.Find(employee => employee.Id == id);

    return employee is null ? Results.NotFound() : Results.Ok(employee);

}).WithName(GetEmployeeEndpointName);

// Add employee
// (CreateEmployeeDto newEmployee) será preenchido com os dados no corpo da requisição (dentro de {})
app.MapPost("employees", (CreateEmployeeDto newEmployee) =>
{
    // Criando objeto 'employee'
    EmployeeDto employee = new(employees.Count + 1, newEmployee.Name, newEmployee.Role);

    // Adicionando o employee à lista que será mostrada no método 'Get'
    employees.Add(employee);

    // Retorna uma resposta HTTP 201 (Created)
    // 'GetEmployeeEndpointName' é uma constante string que representa o nome da rota para obter um 'employee' 
    // 'new { id = employee.Id}' é um objeto anônimo que será usado para gerar a URL do novo recurso
    // 'employee' é o objeto que será enviado ao corpo da resposta
    return Results.CreatedAtRoute(GetEmployeeEndpointName, new { id = employee.Id }, employee);
});

// Update employee
// Quando usamos 2 parâmetros na arrow function, o ASP.NET Core automaticamente preenche o primeiro parâmetro
// com o {id} presente em employees/{id}, ou seja, no endereço. Já o segundo parâmetro é preenchido pelos
// dados dentro da requisição {} 
app.MapPut("employees/{id}", (int id, UpdateEmployeeDto updatedEmployee) =>
{
    var index = employees.FindIndex(employee => employee.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    employees[index] = new EmployeeDto(id, updatedEmployee.Name, updatedEmployee.Role);

    return Results.NoContent();
});

// Delete specific employee
// O parâmetro é preenchido pelos dados no corpo da requisição {}
app.MapDelete("employees/{id}", (int id) =>
{
    employees.RemoveAll(employee => employee.Id == id);

    return Results.NoContent();
});

app.Run();
