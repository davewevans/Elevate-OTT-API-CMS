using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OttApiPlatform.Application.Common.Contracts.Content;
using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Application.Services.Content;

public class VideoService : IVideoService
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;

    #endregion

    #region Constructors

    public VideoService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    } 

    #endregion

    #region Public Methods

    public Task<VideoModel> GetVideo(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<Guid> CreateVideo(VideoModel video, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void UpdateVideo()
    {
        throw new NotImplementedException();
    }
    public void DeleteVideo()
    {
        throw new NotImplementedException();
    }
 
    #endregion
}
