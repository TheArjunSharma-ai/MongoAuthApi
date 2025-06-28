using Newtonsoft.Json;

namespace AuthApi.Modals;
public class CustomJwtParser
{
    public class JwtData
    {
        public Dictionary<string, object> Header { get; set; }
        public Dictionary<string, object> Payload { get; set; }
        public string User { get; set; }
        public DateTime? Expiration { get; set; }
    }

    public static JwtData Parse(string rawToken)
    {
        try
        {
            // Remove outer braces and split by period
            //string cleaned = rawToken.Trim('{', '}');
            string[] parts = rawToken.Split('.', 2);

            if (parts.Length != 2)
                throw new Exception("Token format is invalid.");

            // Parse header and payload JSON
            var header = JsonConvert.DeserializeObject<Dictionary<string, object>>(parts[0]);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(parts[1]);


            // Extract expiration if present
            DateTime? expDate = null;
            if (payload.ContainsKey("exp") && long.TryParse(payload["exp"].ToString(), out long exp))
            {
                expDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            }
            string payloadUser = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            string user = payload.ContainsKey(payloadUser) ? payload[payloadUser].ToString() : null;
            return new JwtData
            {
                Header = header,
                Payload = payload,
                Expiration = expDate,
                User = user
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing token: {ex.Message}");
            return null;
        }
    }
}
