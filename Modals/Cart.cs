using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Cart : Base
{
    [BsonElement("userId")]
    public ObjectId UserId { get; set; }

    [BsonElement("productId")]
    public ObjectId ProductId { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}
