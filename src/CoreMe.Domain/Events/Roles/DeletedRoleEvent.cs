namespace CoreMe.Domain.Events.Roles;

public record DeletedRoleEvent(long RoleId) : IDomainEvent;
