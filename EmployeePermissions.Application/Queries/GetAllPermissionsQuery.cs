using EmployeePermissions.Application.Dtos;

namespace EmployeePermissions.Application.Queries;

using MediatR;
using System.Collections.Generic;
using EmployeePermissions.Domain;

public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
{
    // Parameters for the query can be added here
}