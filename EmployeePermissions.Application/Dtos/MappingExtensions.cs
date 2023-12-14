namespace EmployeePermissions.Application.Dtos;

using Mapster;
using EmployeePermissions.Domain;

public static class MappingExtensions
{
    // Extension method to convert Permission to PermissionDto
    public static PermissionDto ToDto(this Permission permission)
    {
        var config = TypeAdapterConfig<Permission, PermissionDto>.NewConfig()
            .Map(dest => dest.EmployeeName, src => src.Employee.Name)
            .Map(dest => dest.PermissionTypeName, src => src.PermissionType.Name)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeId, src => src.EmployeeId);

        return permission.Adapt<PermissionDto>();
    }

    // Add more extensions if needed
}