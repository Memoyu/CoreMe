using CoreMe.Domain.Events.Users;

namespace CoreMe.Application.Users.Events;

public class DeletedUserEventHadler() : INotificationHandler<DeletedUserEvent>
{
    public Task Handle(DeletedUserEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
