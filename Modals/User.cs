using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;

public class User : Base
{
    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    public string LastName { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("phoneNo")]
    public PhoneNumber PhoneNo { get; set; }
}