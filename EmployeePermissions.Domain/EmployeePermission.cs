namespace EmployeePermissions.Domain;

public class EmployeePermission
{
    private Guid _employeeId;
    private Employee _employee;
    private Guid _permissionTypeId;
    private PermissionType _permissionType;

    public EmployeePermission(Guid employeeId, Employee employee, Guid permissionTypeId, PermissionType permissionType)
    {
        EmployeeId = employeeId;
        Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        PermissionTypeId = permissionTypeId;
        PermissionType = permissionType ?? throw new ArgumentNullException(nameof(permissionType));
    }

    public Guid EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    }

    public Employee Employee
    {
        get => _employee;
        set => _employee = value;
    }

    public Guid PermissionTypeId
    {
        get => _permissionTypeId;
        set => _permissionTypeId = value;
    }

    public PermissionType PermissionType
    {
        get => _permissionType;
        set => _permissionType = value;
    }
}