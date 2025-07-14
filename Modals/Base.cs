using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Base
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
}

