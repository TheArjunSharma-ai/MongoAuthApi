using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Receiver : Base
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("mobileNo")]
    public PhoneNumber MobileNo { get; set; }

    [BsonElement("addressId")]
    public ObjectId AddressId { get; set; }

    [BsonElement("note")]
    public string Note { get; set; }
}

public class PhoneNumber
{
    [BsonElement("countryCode")]
    public string CountryCode { get; set; }

    [BsonElement("phoneNumber")]
    public string Number { get; set; }
}

