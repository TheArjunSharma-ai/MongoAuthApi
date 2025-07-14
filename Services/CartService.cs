using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class CartService
{
    private readonly IMongoCollection<Cart> _carts;

    public CartService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _carts = database.GetCollection<Cart>("Carts");
    }

    public async Task<List<Cart>> GetAllAsync() =>
        await _carts.Find(_ => true).ToListAsync();

    public async Task<Cart> GetByIdAsync(ObjectId id) =>
        await _carts.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task<List<Cart>> GetByUserIdAsync(ObjectId userId) =>
        await _carts.Find(c => c.UserId == userId).ToListAsync();

    public async Task CreateAsync(Cart cart) =>
        await _carts.InsertOneAsync(cart);

    public async Task UpdateAsync(ObjectId id, Cart cartIn) =>
        await _carts.ReplaceOneAsync(c => c.Id == id, cartIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _carts.DeleteOneAsync(c => c.Id == id);
}

