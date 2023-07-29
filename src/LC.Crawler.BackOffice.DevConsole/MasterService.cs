using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using LC.Crawler.BackOffice.Articles;
using LC.Crawler.BackOffice.Core;
using LC.Crawler.BackOffice.DataSources;
using LC.Crawler.BackOffice.Extensions;
using LC.Crawler.BackOffice.Medias;
using LC.Crawler.BackOffice.MessageQueue.Consumers.Etos;
using LC.Crawler.BackOffice.Payloads;
using LC.Crawler.BackOffice.ProductReviews;
using LC.Crawler.BackOffice.Products;
using LC.Crawler.BackOffice.WooCommerces;
using LC.Crawler.BackOffice.Wordpress;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WooCommerceNET;
using WordPressPCL;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace LC.Crawler.BackOffice.DevConsole;

public class MasterService : ITransientDependency
{
    public ILogger<MasterService> Logger { get; set; }

    private readonly ProductManagerAladin _productManagerAladin;
    private readonly ProductManagerLongChau _productManagerLongChau;
    private readonly ProductManagerSieuThiSongKhoe _productManagerSieuThiSongKhoe;
    
    private readonly ArticleManangerLongChau _articleManangerLongChau;
    private readonly ArticleManangerAladin _articleManangerAladin;
    private readonly ArticleManangerSieuThiSongKhoe _articleManangerSieuThiSongKhoe;
    private readonly ArticleManangerSongKhoeMedplus _articleManangerSongKhoeMedplus;
    private readonly ArticleManangerSucKhoeDoiSong _articleManangerSucKhoeDoiSong;
    private readonly ArticleManangerSucKhoeGiaDinh _articleManangerSucKhoeGiaDinh;
    private readonly ArticleManangerAloBacSi _articleManangerAloBacSi;
    private readonly ArticleManangerBlogSucKhoe _articleManangerBlogSucKhoe;
    
    private readonly WooManagerLongChau _wooManagerLongChau;
    private readonly MediaManagerLongChau _mediaManagerLongChau;

    private readonly WooManagerAladin _wooManagerAladin;
    
    private readonly WordpressManagerSieuThiSongKhoe _wordpressManagerSieuThiSongKhoe;

    private readonly WordpressManagerLongChau _wordpressManagerLongChau;
    private readonly WooManangerBase _wooManangerBase;
    private readonly WooApiConsumers _wooApiConsumers;
    private readonly IDataSourceRepository _dataSourceRepository;
    private readonly IArticleSucKhoeDoiSongRepository _articleSucKhoeDoiSongRepository;
    private readonly WordpressManagerBase _wordpressManagerBase;
    private readonly WordpressManagerSucKhoeDoiSong _wordpressManagerSucKhoeDoiSong;
    private readonly WordpressManagerSucKhoeGiaDinh _wordpressManagerSucKhoeGiaDinh;
    private readonly WordpressManagerSongKhoeMedplus _wordpressManagerSongKhoeMedplus;
    private readonly WooManagerSieuThiSongKhoe _wooManagerSieuThiSongKhoe;

    private readonly WordpressManagerAloBacSi _wordpressManagerAloBacSi;
    private readonly WordpressManagerBlogSucKhoe _wordpressManagerBlogSucKhoe;

    private readonly IProductReviewLongChauRepository _productReviewLongChauRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    public MasterService(ProductManagerLongChau productManagerLongChau, WooManagerLongChau wooManagerLongChau, MediaManagerLongChau mediaManagerLongChau, WordpressManagerSieuThiSongKhoe wordpressManagerSieuThiSongKhoe, ArticleManangerLongChau articleManangerLongChau,
        WordpressManagerLongChau wordpressManagerLongChau,
        WooManagerAladin wooManagerAladin,
        ProductManagerSieuThiSongKhoe productManagerSieuThiSongKhoe,
        ArticleManangerSongKhoeMedplus articleManangerSongKhoeMedplus, WooManangerBase wooManangerBase, IDataSourceRepository dataSourceRepository, WooApiConsumers wooApiConsumers,
        IArticleSucKhoeDoiSongRepository articleSucKhoeDoiSongRepository,
        WordpressManagerBase wordpressManagerBase,
        WordpressManagerSucKhoeDoiSong wordpressManagerSucKhoeDoiSong,
        WooManagerSieuThiSongKhoe wooManagerSieuThiSongKhoe,
        WordpressManagerAloBacSi wordpressManagerAloBacSi,
        IProductReviewLongChauRepository productReviewLongChauRepository, ProductManagerAladin productManagerAladin, ArticleManangerAladin articleManangerAladin, ArticleManangerSieuThiSongKhoe articleManangerSieuThiSongKhoe, ArticleManangerSucKhoeDoiSong articleManangerSucKhoeDoiSong, ArticleManangerSucKhoeGiaDinh articleManangerSucKhoeGiaDinh, ArticleManangerAloBacSi articleManangerAloBacSi, ArticleManangerBlogSucKhoe articleManangerBlogSucKhoe, WordpressManagerBlogSucKhoe wordpressManagerBlogSucKhoe, WordpressManagerSongKhoeMedplus wordpressManagerSongKhoeMedplus, WordpressManagerSucKhoeGiaDinh wordpressManagerSucKhoeGiaDinh, IAuditLogRepository auditLogRepository)
    {
        _productManagerLongChau = productManagerLongChau;
        _wooManagerLongChau = wooManagerLongChau;
        _mediaManagerLongChau = mediaManagerLongChau;
        _wordpressManagerSieuThiSongKhoe = wordpressManagerSieuThiSongKhoe;
        _articleManangerLongChau = articleManangerLongChau;
        _wordpressManagerLongChau = wordpressManagerLongChau;
        _wooManagerAladin = wooManagerAladin;
        _productManagerSieuThiSongKhoe = productManagerSieuThiSongKhoe;
        _articleManangerSongKhoeMedplus = articleManangerSongKhoeMedplus;
        _wooManangerBase = wooManangerBase;
        _dataSourceRepository = dataSourceRepository;
        _wooApiConsumers = wooApiConsumers;
        _articleSucKhoeDoiSongRepository = articleSucKhoeDoiSongRepository;
        _wordpressManagerBase = wordpressManagerBase;
        _wordpressManagerSucKhoeDoiSong = wordpressManagerSucKhoeDoiSong;
        _wooManagerSieuThiSongKhoe = wooManagerSieuThiSongKhoe;
        _wordpressManagerAloBacSi = wordpressManagerAloBacSi;
        _productReviewLongChauRepository = productReviewLongChauRepository;
        _productManagerAladin = productManagerAladin;
        _articleManangerAladin = articleManangerAladin;
        _articleManangerSieuThiSongKhoe = articleManangerSieuThiSongKhoe;
        _articleManangerSucKhoeDoiSong = articleManangerSucKhoeDoiSong;
        _articleManangerSucKhoeGiaDinh = articleManangerSucKhoeGiaDinh;
        _articleManangerAloBacSi = articleManangerAloBacSi;
        _articleManangerBlogSucKhoe = articleManangerBlogSucKhoe;
        _wordpressManagerBlogSucKhoe = wordpressManagerBlogSucKhoe;
        _wordpressManagerSongKhoeMedplus = wordpressManagerSongKhoeMedplus;
        _wordpressManagerSucKhoeGiaDinh = wordpressManagerSucKhoeGiaDinh;
        _auditLogRepository = auditLogRepository;
        Logger = NullLogger<MasterService>.Instance;
    }

    public async Task ProcessDataAsync()
    {
         using StreamReader file = File.OpenText(@"C:\Users\huynn\Downloads\LongChau.txt");
         JsonSerializer serializer = new JsonSerializer();
         var crawlResultEtos = (CrawlEcommercePayload)serializer.Deserialize(file, typeof(CrawlEcommercePayload));
         if (crawlResultEtos != null)
         {
             await _productManagerLongChau.ProcessingDataAsync(crawlResultEtos);
         }
    }

    public async Task ProcessLongChauArticleDataAsync()
    {
        using StreamReader file = File.OpenText(@"C:\Users\huynn\Downloads\LongChauArticles.txt");
        
        JsonSerializer serializer = new JsonSerializer();
        var crawlResultEtos = (CrawlArticlePayload)serializer.Deserialize(file, typeof(CrawlArticlePayload));
        if (crawlResultEtos != null)
        {
            await _articleManangerLongChau.ProcessingDataAsync(crawlResultEtos.ArticlesPayload);
        }
        
    }

    public async Task DownLoadMediaAsync()
    {
        await _mediaManagerLongChau.ProcessDownloadMediasAsync();
    }
    //
    // public async Task DownLoadMediaSieuThiSucKhoeAsync()
    // {
    //     await _mediaManagerSieuThiSongKhoe.ProcessDownloadMediasAsync();
    // }
    //
    // public async Task DownLoadMediaSongKhoeMedplusAsync()
    // {
    //     await _mediaManagerSongKhoeMedplus.ProcessDownloadMediasAsync();
    // }

    public async Task DoSyncProductToWooAsync()
    {
        await _wooManagerAladin.DoSyncCategoriesAsync();
        await _wooManagerAladin.DoSyncProductToWooAsync();
        
        
        // await _wooManagerAladin.DoSyncProductToWooAsync();
       // await _wooManagerSieuThiSongKhoe.DoSyncUpdateProduct();
       // await _wooManagerSieuThiSongKhoe.DoSyncProductToWooAsync();
    }

    public async Task DoSyncArticleToWooAsync()
    {
        //var suckhoeArticle = await _articleSucKhoeDoiSongRepository.GetAsync(x => x.Title == "Lợi ích không ngờ của hành tây với sức khỏe");
        //var text = _wordpressManagerSucKhoeDoiSong.DoSyncCategoriesAsync();
        // await _wordpressManagerLongChau.DoSyncCategoriesAsync();
        // await _wordpressManagerLongChau.DoSyncPostAsync();
        //await _wordpressManagerSucKhoeDoiSong.DoSyncPostAsync();
        await _wordpressManagerSucKhoeDoiSong.DoSyncPostAsync();
    }

    public async Task DoReSyncArticles()
    {
        //await _wordpressManagerSucKhoeDoiSong.DoSyncCategoriesAsync();
        //await _wordpressManagerSucKhoeDoiSong.DoReSyncPostAsync();
    }
    
    // public async Task DoSyncSongKhoeMedplusArticles()
    // {
    //     await _wordpressManagerSongKhoeMedplus.DoSyncToWordpress();
    // }

    public async Task LongHandle()
    {
        var client = new WordPressClient($"https://matico.batdongsanaumy.com/wp-json/");
        client.Auth.UseBasicAuth("admin", "123456");
        var fileBytes = await FileExtendHelper.DownloadFile("https://nhathuoclongchau.com/upload/post/46637/images/bi-muoi-dot-nhieu-co-sao-khong%203.jpg");
        if (fileBytes is null) return;

        using var stream = new MemoryStream(fileBytes);
        var img = Image.FromStream(stream);
        
        var mediaResult = await client.Media.CreateAsync(stream, "test-sync-image.jpg", null);
        Console.WriteLine(mediaResult.SourceUrl);

        img.Save(@"D:\myImage1.jpg", ImageFormat.Jpeg);
    }

    public async Task SyncData()
    {
        using StreamReader file = File.OpenText(@"D:\longchau.txt");
        JsonSerializer serializer = new JsonSerializer();
        var crawlResultEtos = (CrawlEcommercePayload)serializer.Deserialize(file, typeof(CrawlEcommercePayload));
        if (crawlResultEtos != null)
        {
            await _productManagerLongChau.ProcessingDataAsync(crawlResultEtos);
        }
    }
    
    public async Task SyncArticle()
    {
        using StreamReader file = File.OpenText(@"D:\path.txt");
        JsonSerializer serializer = new JsonSerializer();
        var crawlResultEtos = (CrawlArticlePayload)serializer.Deserialize(file, typeof(CrawlArticlePayload));
        if (crawlResultEtos != null)
        {
            await _articleManangerAloBacSi.ProcessingDataAsync(crawlResultEtos.ArticlesPayload.ToList());
        }
    }
    
    public async Task SyncJsonArticle()
    {
        using StreamReader file = File.OpenText(@"D:\CrawlSucKhoeDoiSongApiService_04_12_2022.json");
        JsonSerializer serializer = new JsonSerializer();
        var crawlResultEtos = (CrawlResultEto)serializer.Deserialize(file, typeof(CrawlResultEto));
        if (crawlResultEtos?.ArticlePayloads != null)
        {
            await _articleManangerSucKhoeDoiSong.ProcessingDataAsync(crawlResultEtos.ArticlePayloads.ArticlesPayload.ToList());
        }
    }

    public async Task DeleteDuplicateWooProduct(string site)
    {
        var dataSource = await _dataSourceRepository.GetAsync(_ => _.Url.Contains(site));
        await _wooManangerBase.DeleteDuplicateWooProduct(dataSource);
    }
    
    public async Task TestProductApi(string site)
    {
        var dataSource = await _dataSourceRepository.GetAsync(_ => _.Url.Contains(site));
        await _wooApiConsumers.GetProductBrandApi(dataSource);
    }
    
    public async Task TestArticleApi(string site)
    {
        var dataSource = await _dataSourceRepository.GetAsync(_ => _.Url.Contains(site));
        await _wooApiConsumers.GetArticleBrandApi(dataSource);
    }

    public async Task TestSyncReviews()
    {
        await _wooManagerLongChau.DoSyncReviews();
    }
    
    public async Task CountProductAndArticleByCategory()
    {
        var productAladinResult = await _productManagerAladin.CountProductByCategory();
        PrintConsole(productAladinResult, "Product", "Aladin");
    
        var productLongChauResult = await _productManagerLongChau.CountProductByCategory();
        PrintConsole(productLongChauResult, "Product", "LongChau");
        
        var productSieuThiSongKhoeResult = await _productManagerSieuThiSongKhoe.CountProductByCategory();
        PrintConsole(productSieuThiSongKhoeResult, "Product", "SieuThiSongKhoe");
        
        var articleAladinResult = await _articleManangerAladin.CountArticleByCategory();
        PrintConsole(articleAladinResult, "Article", "Aladin");
        
        var articleLongChauResult = await _articleManangerLongChau.CountArticleByCategory();
        PrintConsole(articleLongChauResult, "Article", "LongChau");
        
        var articleSieuThiSongKhoeResult = await _articleManangerSieuThiSongKhoe.CountArticleByCategory();
        PrintConsole(articleSieuThiSongKhoeResult, "Article", "SieuThiSucKhoe");
        
        var articleAloBacSiResult = await _articleManangerAloBacSi.CountArticleByCategory();
        PrintConsole(articleAloBacSiResult, "Article", "AloBacSi");
        
        var articleBlogSucKhoeResult = await _articleManangerBlogSucKhoe.CountArticleByCategory();
        PrintConsole(articleBlogSucKhoeResult, "Article", "BlogSucKhoe");
        
        var articleSongKhoeMedplusResult = await _articleManangerSongKhoeMedplus.CountArticleByCategory();
        PrintConsole(articleSongKhoeMedplusResult, "Article", "SongKhoeMedplus");
        
        var articleSucKhoeDoiSongResult = await _articleManangerSucKhoeDoiSong.CountArticleByCategory();
        PrintConsole(articleSucKhoeDoiSongResult, "Article", "SucKhoeDoiSong");
        
        var articleSucKhoeGiaDinhResult = await _articleManangerSucKhoeGiaDinh.CountArticleByCategory();
        PrintConsole(articleSucKhoeGiaDinhResult, "Article", "SucKhoeGiaDinh");
    
        var lines = new List<string>();
        lines.Add("----------------------------Product Aladin----------------------");
        lines.AddRange(productAladinResult.Select(item => $"{item.Key} ----- {item.Value}").ToList());
        lines.Add("----------------------------Product LongChau----------------------");
        lines.AddRange(productLongChauResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Product SieuThiSongKhoe----------------------");
        lines.AddRange(productSieuThiSongKhoeResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article Aladin----------------------");
        lines.AddRange(articleAladinResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article LongChau----------------------");
        lines.AddRange(articleLongChauResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article SieuThiSongKhoe----------------------");
        lines.AddRange(articleSieuThiSongKhoeResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article AloBacSi----------------------");
        lines.AddRange(articleAloBacSiResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article BlogSucKhoe----------------------");
        lines.AddRange(articleBlogSucKhoeResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article SongKhoeMedplus----------------------");
        lines.AddRange(articleSongKhoeMedplusResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article SucKhoeDoiSong----------------------");
        lines.AddRange(articleSucKhoeDoiSongResult.Select(item => $"{item.Key} ----- {item.Value}"));
        lines.Add("----------------------------Article SucKhoeGiaDinh----------------------");
        lines.AddRange(articleSucKhoeGiaDinhResult.Select(item => $"{item.Key} ----- {item.Value}"));
    
        await File.WriteAllLinesAsync("category-result.txt", lines);
    }

    private void PrintConsole(List<KeyValuePair<string, int>> result, string type, string site)
    {
        Console.WriteLine($"{type} {site} Categories Count: {result.Count}");
        Console.WriteLine($"{type} {site} Count: {result.Sum(_ => _.Value)}");
        foreach (var item in result)
        {
            Console.WriteLine($"{type} Count: {item.Key} -------------{item.Value}");
        }
    }

    public async Task CleanPostDuplicate()
    {
        while (true)
        {
            var dataSources = await _dataSourceRepository.GetListAsync();
            foreach (var dataSource in dataSources)
            {
                try
                {
                    Console.WriteLine($"Site {dataSource.Url}");
                    await _wordpressManagerBase.CleanDuplicatePostsAsync(dataSource);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }
            Console.WriteLine($"Chờ 1 tiếng chạy lại");
            await Task.Delay(TimeSpan.FromHours(1));
        }
    }

    public Task<WordPressClient> InitClient(DataSource dataSource)
    {
        //pass the Wordpress REST API base address as string
        var client = new WordPressClient($"{dataSource.PostToSite}/wp-json/");
        client.Auth.UseBasicAuth(dataSource.Configuration.Username, dataSource.Configuration.Password);
        return Task.FromResult(client);
    }

    public async Task CleanData()
    {
        var date = DateTime.UtcNow;
        var lines = new List<string>();
        
        Console.WriteLine("------------------Aladin--------------------");
        var aladinErrors = await _articleManangerAladin.GetErrorEncodeData();
        if (aladinErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Aladin--------------------");
            lines.AddRange(aladinErrors);
        }
        
        Console.WriteLine("------------------Longchau--------------------");
        var longchauErrors = await _articleManangerLongChau.GetErrorEncodeData();
        if (longchauErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Longchau--------------------");
            lines.AddRange(longchauErrors);
        }
        
        Console.WriteLine("------------------Alobacsi--------------------");
        var alobacsiErrors = await _articleManangerAloBacSi.GetErrorEncodeData();
        if (alobacsiErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Alobacsi--------------------");
            lines.AddRange(alobacsiErrors);
        }
        
        Console.WriteLine("------------------Blogsuckhoe--------------------");
        var blogsuckhoeErrors = await _articleManangerBlogSucKhoe.GetErrorEncodeData();
        if (blogsuckhoeErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Blogsuckhoe--------------------");
            lines.AddRange(blogsuckhoeErrors);
        }
        
        Console.WriteLine("------------------SongkhoeMedplus--------------------");
        var songkhoeMedplusErrors = await _articleManangerSongKhoeMedplus.GetErrorEncodeData();
        if (songkhoeMedplusErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------SongkhoeMedplus--------------------");
            lines.AddRange(songkhoeMedplusErrors);
        }
        
        Console.WriteLine("------------------Sieuthisongkhoe--------------------");
        var sieuthisongkhoeErrors = await _articleManangerSieuThiSongKhoe.GetErrorEncodeData();
        if (sieuthisongkhoeErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Sieuthisongkhoe--------------------");
            lines.AddRange(sieuthisongkhoeErrors);
        }
        
        Console.WriteLine("------------------Suckhoedoisong--------------------");
        var suckhoedoisongErrors = await _articleManangerSucKhoeDoiSong.GetErrorEncodeData();
        if (suckhoedoisongErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Suckhoedoisong--------------------");
            lines.AddRange(suckhoedoisongErrors);
        }
        
        Console.WriteLine("------------------Suckhoegiadinh--------------------");
        var suckhoegiadinhErrors = await _articleManangerSucKhoeGiaDinh.GetErrorEncodeData();
        if (suckhoegiadinhErrors.IsNotNullOrEmpty())
        {
            lines.Add("------------------Suckhoegiadinh--------------------");
            lines.AddRange(suckhoegiadinhErrors);
        }
        
        var logFileName = $"C:\\Work\\ErrorLogs\\All\\all-error-records_{date:dd-MM-yyyy}.txt";
        await File.WriteAllLinesAsync(logFileName, lines);
    }

    public async Task GetAllProduct()
    {
        var dataSource = await _dataSourceRepository.GetAsync(_ => _.Url.Contains("longchau"));
        var rest = new CustomRestAPI($"{dataSource.PostToSite}/wp-json/wc/v3/", dataSource.Configuration.ApiKey, dataSource.Configuration.ApiSecret);
        var wcObject = new WCObject(rest);
        //var products = await wcObject.Product.GetAll();
        var checkProduct = (await wcObject.Product.GetAll(new Dictionary<string, string>()
        {
            { "sku", "00014414" }
        })).FirstOrDefault();
    }

    public T CustomDeserializeJSon<T>(string jsonString)
    {
        if (jsonString.Trim().StartsWith("<"))
            jsonString = jsonString.Substring(jsonString.IndexOf("{"), jsonString.Length - jsonString.IndexOf("{"));
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}
