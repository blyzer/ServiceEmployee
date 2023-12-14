namespace EmployeePermissions.Domain;

using System;
using System.Collections.Generic;

public class Employee
{
    private Guid _id;
    private string _name;
    private ICollection<EmployeePermission> _employeePermissions;

    public Employee(Guid id, string name, ICollection<EmployeePermission> employeePermissions)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        EmployeePermissions = employeePermissions ?? throw new ArgumentNullException(nameof(employeePermissions));
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public ICollection<EmployeePermission> EmployeePermissions
    {
        get => _employeePermissions;
        set => _employeePermissions = value;
    }
}