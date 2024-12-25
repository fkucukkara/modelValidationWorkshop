# Model Validation: Controller-Based API vs Minimal API

This document highlights the key differences in **model validation** between Controller-Based APIs and Minimal APIs, focusing on how **ModelState** behaves.

## Sample Model
```csharp
public class Employee
{
    [Required(ErrorMessage = "ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
}
```

## Controller-Based API
In a Controller-Based API, **ModelState** is available and validation is automatic when using the `[ApiController]` attribute.

### Example:
```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    [HttpPost]
    public IActionResult AddEmployee(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Automatically populated.
        }

        return Ok(employee);
    }
}
```

## Minimal API
In Minimal APIs, **ModelState is not available**. Validation must be performed manually using tools like `Validator.TryValidateObject`.

### Example:
```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/employee", (Employee employee) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(employee);

    if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    return Results.Ok(employee);
});

app.Run();
```

## Comparison
| Feature                     | Controller-Based API                  | Minimal API                              |
|-----------------------------|---------------------------------------|-----------------------------------------|
| **ModelState Behavior**     | Available and automatic validation    | Not available; manual validation needed |
| **Ease of Use**             | Easier with `[ApiController]`         | Requires custom validation logic         |

## Summary
- **Controller-Based API**: Leverage automatic **ModelState** validation for simplicity.
- **Minimal API**: Handle validation manually as **ModelState is not available**.
