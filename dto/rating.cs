using Newtonsoft.Json;
namespace dto {
public class Rating
{
  
    [JsonProperty("id")]
    public string id { get; set; }
    public string userId { get; set; }
    
    [JsonProperty("partitionKey")]
    public string productId { get; set; }
    
    public string timestamp { get; set; }
    public string locationName { get; set; }
    public int rating { get; set; }
    public string userNotes { get; set; }
}
}