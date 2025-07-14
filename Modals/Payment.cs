using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Payment : Base
{
    [BsonElement("paymentId")]
    public string PaymentId { get; set; }

    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonElement("method")]
    public string Method { get; set; }

    [BsonElement("type")]
    public PaymentType Type { get; set; }

    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }
}

public enum PaymentType
{
    UPI,
    Online,
    Card,
    Cash,
    NetBanking,
    Wallet,
    Cheque,
    Other
}

