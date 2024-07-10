namespace CoreMe.Domain.Events.Permissions;

public record DeletedPermissionEvent(long PermissionId) : IDomainEvent;
