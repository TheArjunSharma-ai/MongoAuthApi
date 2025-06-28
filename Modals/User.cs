using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;

public class User
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
}