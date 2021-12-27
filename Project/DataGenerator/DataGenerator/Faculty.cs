using System.Text.Json.Serialization;

namespace DataGenerator
{
    internal class Faculty
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("university_id")]
        public string UniversityId { get; set; }
    }
}