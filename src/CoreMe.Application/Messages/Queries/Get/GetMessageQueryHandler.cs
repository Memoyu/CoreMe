namespace CoreMe.Application.Messages.Queries.Get;

public class GetMessageQueryHandler(
    ) : IRequestHandler<GetMessageQuery, Result>
{
    public Task<Result> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
       throw new NotImplementedException();
    }
}
