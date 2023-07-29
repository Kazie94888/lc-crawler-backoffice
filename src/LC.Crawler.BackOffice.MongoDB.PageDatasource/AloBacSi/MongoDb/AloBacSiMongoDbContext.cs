using LC.Crawler.BackOffice.Articles;
using LC.Crawler.BackOffice.Categories;
using LC.Crawler.BackOffice.Medias;
using LC.Crawler.BackOffice.ProductAttributes;
using LC.Crawler.BackOffice.Products;
using LC.Crawler.BackOffice.ProductVariants;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace LC.Crawler.BackOffice.PageDatasource.AloBacSi.MongoDb;

[ConnectionStringName("AloBacSi")]
public class AloBacSiMongoDbContext : AbpMongoDbContext
{
    public IMongoCollection<Media> Medias => Collection<Media>();
    public IMongoCollection<Article> Articles => Collection<Article>();
    public IMongoCollection<Category> Categories => Collection<Category>();

    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);


        modelBuilder.Entity<Category>(b => { b.CollectionName = BackOfficeConsts.DbTablePrefix + "Categories"; });

        modelBuilder.Entity<Article>(b => { b.CollectionName = BackOfficeConsts.DbTablePrefix + "Articles"; });

        modelBuilder.Entity<Media>(b => { b.CollectionName = BackOfficeConsts.DbTablePrefix + "Medias"; });
    }
}