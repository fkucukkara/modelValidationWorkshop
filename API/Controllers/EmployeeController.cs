using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    public static readonly List<Employee> InMemoryEmployeeRepository =
    [
        new Employee { Id = 1, Name = "Brad Pitt" },
        new Employee { Id = 1, Name = "Tom Cruise" }
    ];

    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok(InMemoryEmployeeRepository);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = InMemoryEmployeeRepository.FirstOrDefault(e => e.Id == id);

        if (employee == null)
            return NotFound($"Employee with ID {id} not found.");

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult AddEmployee(Employee employee)
    {
        // When invalid employee object is passed, return 400 Bad Request by default
        // Sample invalid request is given in API.hhtp file

        InMemoryEmployeeRepository.Add(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }
}

public class Employee
{
    [Required(ErrorMessage = "ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
}
