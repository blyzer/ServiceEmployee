namespace EmployeePermissions.Domain;

public class PermissionDocument
{
    private Guid _id;
    private Guid _employeeId;
    private string _employeeName;
    private Guid _permissionTypeId;
    private string _permissionTypeName;
    private DateTime _dateGranted;

    public PermissionDocument(Guid id, Guid employeeId, string employeeName, Guid permissionTypeId,
        string permissionTypeName, DateTime dateGranted)
    {
        Id = id;
        EmployeeId = employeeId;
        EmployeeName = employeeName ?? throw new ArgumentNullException(nameof(employeeName));
        PermissionTypeId = permissionTypeId;
        PermissionTypeName = permissionTypeName ?? throw new ArgumentNullException(nameof(permissionTypeName));
        DateGranted = dateGranted;
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
    } // Unique identifier for the permission

    public Guid EmployeeId
    {
        get => _employeeId;
        set => _employeeId = value;
    } // Identifier for the associated employee

    public string EmployeeName
    {
        get => _employeeName;
        set => _employeeName = value;
    } // Name of the employee

    public Guid PermissionTypeId
    {
        get => _permissionTypeId;
        set => _permissionTypeId = value;
    } // Identifier for the type of permission

    public string PermissionTypeName
    {
        get => _permissionTypeName;
        set => _permissionTypeName = value;
    } // Name or description of the permission type

    // You can add more properties as needed for the Elasticsearch index
    // For example, you may want to index the date the permission was granted or any other relevant data
    public DateTime DateGranted
    {
        get => _dateGranted;
        set => _dateGranted = value;
    }

    // If you have additional data that will be helpful for searching or aggregating, include them here as well
}