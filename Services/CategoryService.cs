using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class CategoryService
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _categories = database.GetCollection<Category>("Categories");
    }

    public async Task<List<Category>> GetAllAsync() =>
        await _categories.Find(_ => true).ToListAsync();

    public async Task<Category> GetByIdAsync(ObjectId id) =>
        await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Category category) =>
        await _categories.InsertOneAsync(category);

    public async Task UpdateAsync(ObjectId id, Category categoryIn) =>
        await _categories.ReplaceOneAsync(c => c.Id == id, categoryIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _categories.DeleteOneAsync(c => c.Id == id);
}

