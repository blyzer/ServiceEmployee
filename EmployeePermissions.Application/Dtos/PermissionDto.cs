namespace EmployeePermissions.Application.Dtos;

public class PermissionDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } // Assuming we want to include the employee's name
    public string PermissionTypeName { get; set; } // Assuming we want to include the permission type's name

    // You can add more properties that make sense for your client's needs
}