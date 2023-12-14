namespace EmployeePermissions.Application.Commands;

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreatePermissionCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; }
    public Guid PermissionTypeId { get; }

    public CreatePermissionCommand( Guid employeeId, Guid permissionTypeId)
    {
        EmployeeId = employeeId;
        PermissionTypeId = permissionTypeId;

    }
}