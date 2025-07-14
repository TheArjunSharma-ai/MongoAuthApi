using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Modals;
public class Address : Base
{
    [BsonElement("landMark")]
    public string LandMark { get; set; } // locality, area

    [BsonElement("street")]
    public string Street { get; set; } // area, gali, road

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("state")]
    public string State { get; set; }

    [BsonElement("postalCode")]
    public string PostalCode { get; set; }

    [BsonElement("country")]
    public string Country { get; set; }
}

