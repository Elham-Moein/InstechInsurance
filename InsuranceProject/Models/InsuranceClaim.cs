using Newtonsoft.Json;
namespace InsuranceProject.Models
{
    public class InsuranceClaim
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int Year { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name{ get; set; }
        public Type Type{ get; set; }
        public decimal DamageCost{ get; set; }
    }
}