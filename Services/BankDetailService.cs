using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Services;

public class BankDetailService
{
    private readonly IMongoCollection<BankDetail> _bankDetails;

    public BankDetailService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _bankDetails = database.GetCollection<BankDetail>("BankDetails");
    }

    public async Task<List<BankDetail>> GetAllAsync() =>
        await _bankDetails.Find(_ => true).ToListAsync();

    public async Task<BankDetail> GetByIdAsync(ObjectId id) =>
        await _bankDetails.Find(b => b.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(BankDetail bankDetail) =>
        await _bankDetails.InsertOneAsync(bankDetail);

    public async Task UpdateAsync(ObjectId id, BankDetail bankDetailIn) =>
        await _bankDetails.ReplaceOneAsync(b => b.Id == id, bankDetailIn);

    public async Task DeleteAsync(ObjectId id) =>
        await _bankDetails.DeleteOneAsync(b => b.Id == id);
}

