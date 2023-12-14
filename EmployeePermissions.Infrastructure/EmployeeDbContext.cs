using EmployeePermissions.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeePermissions.Infrastructure;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
    {
    }

    protected EmployeeDbContext(DbSet<Employee> employees, DbSet<Permission> permissions,
        DbSet<EmployeePermission> employeePermissions, DbSet<PermissionType> permissionTypes)
    {
        Employees = employees ?? throw new ArgumentNullException(nameof(employees));
        Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        EmployeePermissions = employeePermissions ?? throw new ArgumentNullException(nameof(employeePermissions));
        PermissionTypes = permissionTypes ?? throw new ArgumentNullException(nameof(permissionTypes));
    }

    public EmployeeDbContext(DbContextOptions options, DbSet<Employee> employees, DbSet<Permission> permissions,
        DbSet<EmployeePermission> employeePermissions, DbSet<PermissionType> permissionTypes) :
        base(options)
    {
        Employees = employees ?? throw new ArgumentNullException(nameof(employees));
        Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        EmployeePermissions = employeePermissions ?? throw new ArgumentNullException(nameof(employeePermissions));
        PermissionTypes = permissionTypes ?? throw new ArgumentNullException(nameof(permissionTypes));
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<EmployeePermission> EmployeePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the primary keys
        modelBuilder.Entity<Employee>().HasKey(e => e.Id);
        modelBuilder.Entity<Permission>().HasKey(p => p.Id);
        modelBuilder.Entity<PermissionType>().HasKey(pt => pt.Id);
        modelBuilder.Entity<EmployeePermission>().HasKey(ep => new { ep.EmployeeId, ep.PermissionTypeId });

        // Define the relationships
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.EmployeePermissions)
            .WithOne()
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust the deletion behavior as needed

        modelBuilder.Entity<PermissionType>()
            .HasMany<Permission>()
            .WithOne(p => p.PermissionType)
            .HasForeignKey(p => p.PermissionTypeId);

        // If EmployeePermission represents a join table
        // for a many-to-many relationship between Employees and PermissionTypes:
        modelBuilder.Entity<EmployeePermission>()
            .HasOne(ep => ep.Employee)
            .WithMany(e => e.EmployeePermissions)
            .HasForeignKey(ep => ep.EmployeeId);

        modelBuilder.Entity<EmployeePermission>()
            .HasOne(ep => ep.PermissionType)
            .WithMany() // Assuming PermissionType does not have a navigation property back to EmployeePermission
            .HasForeignKey(ep => ep.PermissionTypeId);

        // Alternatively, if PermissionType has a navigation property for EmployeePermissions, define the inverse relationship
        // modelBuilder.Entity<EmployeePermission>()
        //    .HasOne(ep => ep.PermissionType)
        //    .WithMany(pt => pt.EmployeePermissions)
        //    .HasForeignKey(ep => ep.PermissionTypeId);

        // Configure additional properties using the Fluent API if needed
        // For example, you can configure table names, column types, constraints, etc.

        base.OnModelCreating(modelBuilder);
    }
}