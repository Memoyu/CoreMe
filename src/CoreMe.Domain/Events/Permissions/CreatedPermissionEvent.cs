namespace CoreMe.Domain.Events.Permissions;

public record CreatedPermissionEvent(long PermissionId) : IDomainEvent;
