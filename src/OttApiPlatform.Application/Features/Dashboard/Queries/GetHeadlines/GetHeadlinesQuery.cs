namespace OttApiPlatform.Application.Features.Dashboard.Queries.GetHeadlines;

public class GetHeadlinesQuery : IRequest<Envelope<HeadlinesResponse>>
{
    #region Public Classes

    public class GetHeadlinesQueryHandler : IRequestHandler<GetHeadlinesQuery, Envelope<HeadlinesResponse>>
    {
        #region Public Methods

        public Task<Envelope<HeadlinesResponse>> Handle(GetHeadlinesQuery request, CancellationToken cancellationToken)
        {
            var headlinesResponse = new HeadlinesResponse();
            return Task.FromResult(Envelope<HeadlinesResponse>.Result.Ok(headlinesResponse));
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}