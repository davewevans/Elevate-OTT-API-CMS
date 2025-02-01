using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.CreateAsset;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Commands.UpdateAsset;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssetById;
using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssets;
using OttApiPlatform.Domain.Entities.ContentManagement;

namespace OttApiPlatform.Application.Mappings;

public class MappingProfiles
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<AssetModel, GetAssetByIdResponse>();
            CreateMap<AssetModel, AssetItem>();
            CreateMap<AssetModel, CreateAssetResponse>();
            CreateMap<AssetModel, UpdateAssetResponse>();
        }
    }

    public class GetAssetByIdResponseProfile : Profile
    {
        public GetAssetByIdResponseProfile()
        {
            CreateMap<AssetModel, GetAssetByIdResponse>();
        }
    }

    public class GetAssetsResponseProfile : Profile
    {
        public GetAssetsResponseProfile()
        {
            CreateMap<AssetModel, AssetItem>();
        }
    }

    public class CreateAssetResponseProfile : Profile
    {
        public CreateAssetResponseProfile()
        {
            CreateMap<AssetModel, CreateAssetResponse>();
        }
    }
}
