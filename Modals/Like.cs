using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Like : Base
{
    [BsonElement("productId")]
    public ObjectId ProductId { get; set; }

    [BsonElement("userId")]
    public ObjectId UserId { get; set; }

    [BsonElement("likedAt")]
    public DateTime LikedAt { get; set; }
}

public class LikeCount
{
    [BsonElement("productId")]
    public ObjectId ProductId { get; set; }

    [BsonElement("symbol")]
    [BsonRepresentation(BsonType.String)]
    public LikeSymbol Symbol { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }
}

public enum LikeSymbol
{
    Heart,
    ThumbsUp,
    Star,
    Smile,
    Fire
}

