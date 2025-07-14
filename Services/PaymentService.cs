using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class PaymentService
{
    private readonly IMongoCollection<Payment> _payments;

    public PaymentService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _payments = database.GetCollection<Payment>("Payments");
    }

    public async Task<List<Payment>> GetAllAsync() =>
        await _payments.Find(_ => true).ToListAsync();

    public async Task<Payment> GetByIdAsync(ObjectId id) =>
        await _payments.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Payment payment) =>
        await _payments.InsertOneAsync(payment);

    public async Task UpdateAsync(ObjectId id, Payment paymentIn) =>
        await _payments.ReplaceOneAsync(p => p.Id == id, paymentIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _payments.DeleteOneAsync(p => p.Id == id);
}

