namespace EmployeePermissions.Domain;

public class Permission
{
    private Guid _id;
    private Guid _employeeId;
    private Employee _employee;
    private Guid _permissionTypeId;
    private PermissionType _permissionType;

    public Permission(Guid id, Guid employeeId, Employee? employee, Guid permissionTypeId, PermissionType permissionType)
    {
        _id = id;
        _employeeId = employeeId;
        _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        _permissionTypeId = permissionTypeId;
        _permissionType = permissionType ?? throw new ArgumentNullException(nameof(permissionType));
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
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