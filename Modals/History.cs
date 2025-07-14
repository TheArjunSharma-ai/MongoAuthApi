using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class History : Base
{
    [BsonElement("action")]
    [BsonRepresentation(BsonType.String)]
    public ActionType Action { get; set; }

    [BsonElement("method")]
    public string Method { get; set; }

    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("details")]
    public string Details { get; set; }
}

public enum ActionType
{
    Add,
    Update,
    Delete,
    View,
    Login,
    Logout
}
