using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Exceptions;
public class VideoNotFoundException : NotFoundException
{
    public VideoNotFoundException(Guid videoId) :
        base($"The video with id: {videoId} doesn't exist in the database.")
    { }

    public VideoNotFoundException() :
        base($"The video requested doesn't exist in the database.")
    { }
}
