using EmployeePermissions.Application.Commands;
using EmployeePermissions.Application.Queries;
using EmployeePermissions.Domain;
using EmployeePermissions.Infrastructure;
using MediatR;
using Nest;

namespace EmployeePermissions.Application.Handlers;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IElasticClient _elasticClient;
    private readonly KafkaProducerService _kafkaProducer;

    public CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IElasticClient elasticClient, KafkaProducerService kafkaProducer)
    {
        _unitOfWork = unitOfWork;
        _elasticClient = elasticClient;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<Guid> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        // Use your repository or a similar persistence mechanism to retrieve entities
        Employee? employee = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(request.EmployeeId);
        PermissionType? permissionType = await _unitOfWork.GetRepository<PermissionType>().GetByIdAsync(request.PermissionTypeId);
        
        // Validate that the entities were found
        if (employee == null || permissionType == null)
        {
            throw new ArgumentException("Invalid EmployeeId or PermissionTypeId.");
        }

        // Create the Permission using the provided constructor
        var permission = new Permission(
            Guid.NewGuid(), // Assuming we're creating a new GUID for the permission
            request.EmployeeId,
            employee,
            request.PermissionTypeId,
            permissionType
        );
        
        
        // Add permission to the repository
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            await _unitOfWork.GetRepository<Permission>().AddAsync(permission, cancellationToken);
            // Perform additional related operations...

            await _unitOfWork.CommitTransactionAsync();
            var document = new PermissionDocument(
                permission.Id,
                permission.EmployeeId,
                permission.Employee.Name,
                permission.PermissionTypeId,
                permission.PermissionType.Name,
                DateTime.Today
            );
            var response = await _elasticClient.IndexDocumentAsync(document);
            if(!response.IsValid)
            {
                // Handle the error of document indexing...
            }
            await _kafkaProducer.SendMessageAsync("OperationKey", "OperationCompleted");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw; // Consider logging the exception or handling it as needed
        }

        return permission.Id;
    }
}