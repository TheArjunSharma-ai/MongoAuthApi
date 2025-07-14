using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class AddressService
{
    private readonly IMongoCollection<Address> _addresses;

    public AddressService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _addresses = database.GetCollection<Address>("Addresses");
    }

    public async Task<List<Address>> GetAllAsync() =>
        await _addresses.Find(_ => true).ToListAsync();

    public async Task<Address> GetByIdAsync(ObjectId id) =>
        await _addresses.Find(a => a.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Address address) =>
        await _addresses.InsertOneAsync(address);

    public async Task UpdateAsync(ObjectId id, Address addressIn) =>
        await _addresses.ReplaceOneAsync(a => a.Id == id, addressIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _addresses.DeleteOneAsync(a => a.Id == id);
}
