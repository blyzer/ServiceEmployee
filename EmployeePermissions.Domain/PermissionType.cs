namespace EmployeePermissions.Domain;

public class PermissionType
{
    private Guid _id;
    private string _name;

    public PermissionType(Guid id, string name)
    {
        _id = id;
        _name = name ?? throw new ArgumentNullException(nameof(name));
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
}