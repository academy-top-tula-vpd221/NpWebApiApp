using NpClientWebApiApp;
using System.Net.Http.Json;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

HttpClient client = new();
string server = "https://localhost:7093/api/empls";


await ViewAll();
Console.WriteLine();

//await ViewOne();
//Console.WriteLine();

//await AddNew();
//Console.WriteLine();

//await Update();
//Console.WriteLine();

await Delete();
Console.WriteLine();

async Task ViewAll()
{
    List<Employee>? employees = await client.GetFromJsonAsync<List<Employee>>(server);
    if(employees is not null)
    {
        foreach (var e in employees)
            Console.WriteLine($"Id: {e.Id}, Name: {e.Name}, Age: {e.Age}");
    }
}

async Task ViewOne()
{
    Console.Write("Input id: ");
    int id = Int32.Parse(Console.ReadLine());

    using var response = await client.GetAsync(server + $"/{id}");

    if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        NpClientWebApiApp.Error? error = await response.Content.ReadFromJsonAsync<NpClientWebApiApp.Error>();
        Console.WriteLine($"Error: {error?.Message}");
    }
    else if(response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        Employee? employee = await response.Content.ReadFromJsonAsync<Employee>();
        Console.WriteLine($"Employee: Id-{employee.Id}, Name:{employee.Name}, Age: {employee.Age}");
    }
}

async Task AddNew()
{
    string name = "";
    int age;

    Console.Write("Input Name: ");
    name = Console.ReadLine();
    Console.Write("Input Age: ");
    age = Int32.Parse(Console.ReadLine());

    Employee? employee = new() { Name = name, Age = age };

    using var response = await client.PostAsJsonAsync(server, employee);
    employee = await response.Content.ReadFromJsonAsync<Employee>();

    await ViewAll();
}

async Task Update()
{
    Console.Write("Input id: ");
    int id = Int32.Parse(Console.ReadLine());


    string name = "";
    int age;

    Console.Write("Input Name: ");
    name = Console.ReadLine();
    Console.Write("Input Age: ");
    age = Int32.Parse(Console.ReadLine());

    Employee? employee = new() { Id = id, Name = name, Age = age };

    using var response = await client.PutAsJsonAsync(server, employee);

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        NpClientWebApiApp.Error? error = await response.Content.ReadFromJsonAsync<NpClientWebApiApp.Error>();
        Console.WriteLine($"Error: {error?.Message}");
    }
    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        employee = await response.Content.ReadFromJsonAsync<Employee>();
        Console.WriteLine($"Employee: Id-{employee.Id}, Name:{employee.Name}, Age: {employee.Age}");
    }

    await ViewAll();
}

async Task Delete()
{
    Console.Write("Input id: ");
    int id = Int32.Parse(Console.ReadLine());

    //Employee? employee = await client.DeleteFromJsonAsync<Employee>(server + $"/{id}");

    using var response = await client.DeleteAsync(server + $"/{id}");
    

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        NpClientWebApiApp.Error? error = await response.Content.ReadFromJsonAsync<NpClientWebApiApp.Error>();
        Console.WriteLine($"Error: {error?.Message}");
    }
    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        Employee? employee = await response.Content.ReadFromJsonAsync<Employee>();
        Console.WriteLine($"Employee: Id-{employee.Id}, Name:{employee.Name}, Age: {employee.Age}");
    }

    await ViewAll();

}