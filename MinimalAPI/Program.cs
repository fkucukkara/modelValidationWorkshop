using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<Employee> inMemoryEmployeeRepository =
[
    new Employee { Id = 1, Name = "Brad Pitt" },
    new Employee { Id = 1, Name = "Tom Cruise" }
];

app.MapGet("/employee", () =>
{
    return inMemoryEmployeeRepository;
});

app.MapGet("/employee/{id}", (int id) =>
{
    return inMemoryEmployeeRepository;
});

app.MapPost("/employee", (Employee employee) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(employee);
    if (!Validator.TryValidateObject(employee, validationContext, validationResults, validateAllProperties: true))
    {
        var errors = validationResults.ToDictionary(v => v.MemberNames.FirstOrDefault() ?? "Error", v => new[] { v.ErrorMessage });
        return Results.ValidationProblem(errors!);
    }

    inMemoryEmployeeRepository.Add(employee);
    return TypedResults.Created($"/employee/{employee.Id}", employee);
});

app.Run();

public class Employee
{
    [Required(ErrorMessage = "ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
}
