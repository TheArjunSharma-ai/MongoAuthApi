using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class BankDetail : Base
{
    [BsonElement("bankName")]
    public string BankName { get; set; }

    [BsonElement("accountNumber")]
    public string AccountNumber { get; set; }

    [BsonElement("ifsc")]
    public string IFSC { get; set; }

    [BsonElement("accountHolder")]
    public string AccountHolder { get; set; }
}

