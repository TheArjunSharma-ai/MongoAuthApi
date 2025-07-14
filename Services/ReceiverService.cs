using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class ReceiverService
{
    private readonly IMongoCollection<Receiver> _receivers;

    public ReceiverService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _receivers = database.GetCollection<Receiver>("Receivers");
    }

    public async Task<List<Receiver>> GetAllAsync() =>
        await _receivers.Find(_ => true).ToListAsync();

    public async Task<Receiver> GetByIdAsync(ObjectId id) =>
        await _receivers.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Receiver receiver) =>
        await _receivers.InsertOneAsync(receiver);

    public async Task UpdateAsync(ObjectId id, Receiver receiverIn) =>
        await _receivers.ReplaceOneAsync(r => r.Id == id, receiverIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _receivers.DeleteOneAsync(r => r.Id == id);
}

