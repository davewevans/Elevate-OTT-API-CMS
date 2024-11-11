using OttApiPlatform.Application.Common.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Infrastructure.Repository;
public sealed class RepositoryManager : IRepositoryManager
{
    private readonly IApplicationDbContext _repositoryContext;
    private readonly Lazy<IAuthorRepository> _authorRepository;
    private readonly Lazy<IVideoRepository> _videoRepository;
    private readonly Lazy<ILiveStreamRepository> _liveStreamRepository;
    private readonly Lazy<IPodcastRepository> _podcastRepository;
    private readonly Lazy<IAssetImageRepository> _assetImageRepository;

    private readonly Lazy<ICollectionRepository> _collectionRepository;
    private readonly Lazy<ICategoryRepository> _categoryRepository;
    private readonly Lazy<IContentFeedRepository> _contentFeedRepository;


    //private readonly Lazy<ISubtitleRepository> _subtitleRepository;
    //private readonly Lazy<ISeoMetaDataRepository> _seoMetaDateRepository;
    //private readonly Lazy<IExtraRepository> _extraRepository;
    //private readonly Lazy<ICommentRepository> _commentRepository;
    //private readonly Lazy<ISubscriptionRepository> _subscriptionRepository;
    //private readonly Lazy<IProductItemRepository> _productItemRepository;
    //private readonly Lazy<IItemPriceRepository> _itemPricedRepository;
    //private readonly Lazy<IProductFamilyRepository> _productFamilyRepository;
    public RepositoryManager(ApplicationDbContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _authorRepository = new Lazy<IAuthorRepository>(() => new AuthorRepository(repositoryContext));
        //_videoRepository = new Lazy<IVideoRepository>(() => new VideoRepository(repositoryContext));
        _liveStreamRepository = new Lazy<ILiveStreamRepository>(() => new LiveStreamRepository(repositoryContext));
        _collectionRepository = new Lazy<ICollectionRepository>(() => new CollectionRepository(repositoryContext));
        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(repositoryContext));
        _contentFeedRepository = new Lazy<IContentFeedRepository>(() => new ContentFeedRepository(repositoryContext));

        _podcastRepository = new Lazy<IPodcastRepository>(() => new PodcastRepository(repositoryContext));
        _assetImageRepository = new Lazy<IAssetImageRepository>(() => new AssetImageRepository(repositoryContext));


        //         _subtitleRepository = new Lazy<ISubtitleRepository>(() => new SubtitleRepository(repositoryContext));
        //         _seoMetaDateRepository = new Lazy<ISeoMetaDataRepository>(() => new SeoMetaDataRepository(repositoryContext));
        //         _extraRepository = new Lazy<IExtraRepository>(() => new ExtraRepository(repositoryContext));
        //         _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(repositoryContext));
        //         _subscriptionRepository = new Lazy<ISubscriptionRepository>(() => new SubscriptionRepository(repositoryContext));
        //         _productItemRepository = new Lazy<IProductItemRepository>(() => new ProductItemRepository(repositoryContext));
        //         _itemPricedRepository = new Lazy<IItemPriceRepository>(() => new ItemPriceRepository(repositoryContext));
        //         _productFamilyRepository = new Lazy<IProductFamilyRepository>(() => new ProductFamilyRepository(repositoryContext));

    }

    public IAuthorRepository Author => _authorRepository.Value;
    //public IVideoRepository Video => _videoRepository.Value;
    public ILiveStreamRepository LiveStream => _liveStreamRepository.Value;
    public IPodcastRepository Podcast => _podcastRepository.Value;
    public IAssetImageRepository AssetImage => _assetImageRepository.Value;
    public ICategoryRepository Category => _categoryRepository.Value;
    public ICollectionRepository Collection => _collectionRepository.Value;
    public IContentFeedRepository ContentFeed => _contentFeedRepository.Value;

    //      public ICommentRepository Comment => _commentRepository.Value;
    //      public IExtraRepository Extra => _extraRepository.Value;
    //      public ISeoMetaDataRepository SeoMetaData => _seoMetaDateRepository.Value;
    //      public ISubtitleRepository Subtitle => _subtitleRepository.Value;
    //      public ISubscriptionRepository Subscription => _subscriptionRepository.Value;
    //      public IProductItemRepository ProductItem => _productItemRepository.Value;
    //      public IItemPriceRepository ItemPrice => _itemPricedRepository.Value;
    //      public IProductFamilyRepository ProductFamily => _productFamilyRepository.Value;

    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}
