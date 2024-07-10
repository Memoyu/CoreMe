namespace CoreMe.Domain.Events.Users;

public record DeletedUserEvent(long UserId) : IDomainEvent;
