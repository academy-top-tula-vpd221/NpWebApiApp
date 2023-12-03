using NpServerWebApiApp;

int idCounter = 1;
List<Employee> employees = new List<Employee>()
{
    new(){ Id = idCounter++, Name = "Bobby", Age = 31 },
    new(){ Id = idCounter++, Name = "Sammy", Age = 27 },
    new(){ Id = idCounter++, Name = "Leopold", Age = 19 },
    new(){ Id = idCounter++, Name = "Jotheph", Age = 34 },
    new(){ Id = idCounter++, Name = "Tommy", Age = 25 },
};


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/empls", () => employees);

app.MapGet("/api/empls/{id}", (int id) =>
{
    Employee? employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null) return Results.NotFound(new { Message = "Employee not found" });

    return Results.Json(employee);
});

app.MapDelete("/api/empls/{id}", (int id) =>
{
    Employee? employee = employees.FirstOrDefault(e => e.Id == id);

    if (employee == null) return Results.NotFound(new { Message = "Employee not found" });

    employees.Remove(employee);
    return Results.Json(employee);
});

app.MapPost("/api/empls", (Employee employee) =>
{
    employee.Id = idCounter++;

    employees.Add(employee);
    return Results.Json(employee);
});

app.MapPut("/api/empls", (Employee employeeClient) =>
{
    Employee? employee = employees.FirstOrDefault(e => e.Id == employeeClient.Id);

    if (employee == null) return Results.NotFound(new { Message = "Employee not found" });

    employee.Name = employeeClient.Name;
    employee.Age = employeeClient.Age;

    return Results.Json(employee);
});

app.Run();
