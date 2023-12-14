using EmployeePermissions.Application.Dtos;
using EmployeePermissions.Application.Queries;
using EmployeePermissions.Domain;
using EmployeePermissions.Infrastructure;
using Mapster;
using MediatR;

namespace EmployeePermissions.Application.Handlers;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPermissionsQueryHandler( IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _unitOfWork.GetRepository<Permission>().GetAllAsync(cancellationToken);

        // Map the Permissions to PermissionDtos
        var permissionDtos = permissions.Adapt<IEnumerable<PermissionDto>>();

        return permissionDtos;
    }
}