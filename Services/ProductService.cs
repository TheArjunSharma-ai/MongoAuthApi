using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _products = database.GetCollection<Product>("Products");
    }

    public async Task<List<Product>> GetAllAsync() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<Product> GetByIdAsync(ObjectId id) =>
        await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product product) =>
        await _products.InsertOneAsync(product);

    public async Task UpdateAsync(ObjectId id, Product productIn) =>
        await _products.ReplaceOneAsync(p => p.Id == id, productIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _products.DeleteOneAsync(p => p.Id == id);
}

